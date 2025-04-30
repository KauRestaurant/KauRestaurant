using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.Admin
{
    // Restrict access to users with roles A1 or A2
    [Authorize(Roles = "A1,A2")]
    public class MealManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MealManagementController> _logger;

        // Constructor dependency injection for DB context, hosting environment, and logger
        public MealManagementController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ILogger<MealManagementController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // Fetches a list of meals from the database and displays them
        public async Task<IActionResult> Index()
        {
            try
            {
                // Retrieve meals, including their reviews, ordered by category, type, then name
                var meals = await _context.Meals
                    .Include(m => m.Reviews)
                    .OrderBy(m => m.MealCategory)
                    .ThenBy(m => m.MealType)
                    .ThenBy(m => m.MealName)
                    .ToListAsync();

                // Pass the meals list to our view
                return View("~/Views/Admin/MealManagement.cshtml", meals);
            }
            catch (Exception ex)
            {
                // Log if there is any exception when retrieving meals
                _logger.LogError(ex, "Error loading meals");
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل الوجبات";
                // Return an empty list on error
                return View("~/Views/Admin/MealManagement.cshtml", new List<Meal>());
            }
        }

        // Displays a blank Meal object for adding a new meal
        public IActionResult Add() => View("~/Views/Admin/AddMeal.cshtml", new Meal());

        // Retrieves a specific meal by ID and passes it to the edit view
        public async Task<IActionResult> Edit(int id)
        {
            // Include related review and customer info in the query
            var meal = await _context.Meals
                .Include(m => m.Reviews)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(m => m.MealID == id);

            // Redirect if no meal was found
            if (meal == null)
            {
                TempData["ErrorMessage"] = "الوجبة غير موجودة";
                return RedirectToAction(nameof(Index));
            }

            // Render the Edit page with the selected meal
            return View("~/Views/Admin/EditMeal.cshtml", meal);
        }

        // Handles creating a new meal record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMeal(Meal meal, IFormFile Picture)
        {
            try
            {
                // Validate essential meal fields
                if (!ValidateMealBasics(meal))
                {
                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها والوصف مطلوبة";
                    return RedirectToAction(nameof(Add));
                }

                // Ensure nutritional values have minimum valid defaults
                SetDefaultNutritionalValues(meal);

                // Save a new image or use default if none was provided
                meal.PicturePath = Picture != null && Picture.Length > 0
                    ? await SaveMealImage(Picture)
                    : "/images/meal.png";

                // Add the new meal entity to our DB context and save
                _context.Add(meal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // If something goes wrong, log it and show an error
                _logger.LogError(ex, "Error creating meal");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء إضافة الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Add));
            }
        }

        // Handles updating an existing meal record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMeal(int MealID, string MealName, string Description,
            int Calories, int Protein, int Carbs, int Fat, string MealCategory, string MealType,
            IFormFile Picture, bool RemoveImage = false)
        {
            try
            {
                // Basic validation to ensure required fields aren't empty
                if (string.IsNullOrEmpty(MealName) || string.IsNullOrEmpty(MealCategory) || string.IsNullOrEmpty(MealType))
                {
                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها مطلوبة";
                    return RedirectToAction(nameof(Edit), new { id = MealID });
                }

                // Attempt to find the corresponding meal record
                var meal = await _context.Meals.FindAsync(MealID);
                if (meal == null)
                {
                    TempData["ErrorMessage"] = "الوجبة غير موجودة";
                    return RedirectToAction(nameof(Index));
                }

                // Keep track of the old image path in case we need to delete it
                string oldPicturePath = meal.PicturePath;

                // Update meal fields with new values
                UpdateMealProperties(meal, MealName, Description, Calories, Protein, Carbs, Fat, MealCategory, MealType);

                // Process new or removed images
                await HandleMealImage(meal, oldPicturePath, Picture, RemoveImage);

                // Save the updated meal to the database
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تحديث الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error if update fails
                _logger.LogError(ex, $"Error updating meal ID: {MealID}");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء تحديث الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Edit), new { id = MealID });
            }
        }

        // Handles deleting a meal record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMeal(int MealID)
        {
            try
            {
                // Attempt to find the meal with related entries
                var meal = await _context.Meals
                    .Include(m => m.MenuMeals)
                    .Include(m => m.Reviews)
                    .FirstOrDefaultAsync(m => m.MealID == MealID);

                // If no meal found, redirect
                if (meal == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على الوجبة";
                    return RedirectToAction(nameof(Index));
                }

                // Remove related menu-meals if any exist
                if (meal.MenuMeals?.Any() == true)
                {
                    _context.MenuMeals.RemoveRange(meal.MenuMeals);
                }

                // Remove associated reviews if any exist
                if (meal.Reviews?.Any() == true)
                {
                    _context.Reviews.RemoveRange(meal.Reviews);
                }

                // Save changes before deleting the image for safety
                await _context.SaveChangesAsync();

                // Delete the meal's image file if it's custom (not the default)
                if (!string.IsNullOrEmpty(meal.PicturePath) &&
                    !meal.PicturePath.Equals("/images/meal.png") &&
                    meal.PicturePath.StartsWith("/images/meals/"))
                {
                    DeleteImageFile(meal.PicturePath);
                }

                // Remove the meal itself from the database
                _context.Meals.Remove(meal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log any exception during deletion
                _logger.LogError(ex, $"Error deleting meal ID: {MealID}");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء حذف الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Deletes a review from a meal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int reviewId, int returnToMealId)
        {
            try
            {
                // Locate the review by its ID
                var review = await _context.Reviews.FindAsync(reviewId);
                if (review == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على التقييم";
                    return RedirectToAction(nameof(Edit), new { id = returnToMealId });
                }

                // Remove the targeted review
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف التقييم بنجاح";
                return RedirectToAction(nameof(Edit), new { id = returnToMealId });
            }
            catch (Exception ex)
            {
                // Log errors if the review wasn't deleted successfully
                _logger.LogError(ex, $"Error deleting review ID: {reviewId}");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف التقييم";
                return RedirectToAction(nameof(Edit), new { id = returnToMealId });
            }
        }

        #region Helper Methods

        // Checks if the meal has the basic required fields
        private bool ValidateMealBasics(Meal meal)
        {
            return !string.IsNullOrEmpty(meal.MealName) &&
                   !string.IsNullOrEmpty(meal.MealCategory) &&
                   !string.IsNullOrEmpty(meal.MealType) &&
                   !string.IsNullOrEmpty(meal.Description);
        }

        // Assigns default nutritional values if any are zero or less
        private void SetDefaultNutritionalValues(Meal meal)
        {
            if (meal.Calories <= 0) meal.Calories = 1;
            if (meal.Protein <= 0) meal.Protein = 1;
            if (meal.Carbs <= 0) meal.Carbs = 1;
            if (meal.Fat <= 0) meal.Fat = 1;
        }

        // Updates the meal's properties based on the provided information
        private void UpdateMealProperties(Meal meal, string name, string description,
            int calories, int protein, int carbs, int fat, string category, string type)
        {
            meal.MealName = name;
            meal.Description = description ?? string.Empty;
            meal.Calories = calories;
            meal.Protein = protein;
            meal.Carbs = carbs;
            meal.Fat = fat;
            meal.MealCategory = category;
            meal.MealType = type;
        }

        // Orchestrates image handling operations during meal update
        private async Task HandleMealImage(Meal meal, string oldPath, IFormFile picture, bool removeImage)
        {
            if (removeImage)
            {
                // If the image is set to be removed, switch to default image and delete the old file
                meal.PicturePath = "/images/meal.png";
                DeleteOldImageIfNeeded(oldPath);
            }
            else if (picture != null && picture.Length > 0)
            {
                // If a new picture is provided, save it and remove the previous one if necessary
                meal.PicturePath = await SaveMealImage(picture);
                DeleteOldImageIfNeeded(oldPath);
            }
        }

        // Removes an existing image file if it's not the default
        private void DeleteOldImageIfNeeded(string oldPath)
        {
            if (!string.IsNullOrEmpty(oldPath) &&
                !oldPath.Equals("/images/meal.png") &&
                oldPath.StartsWith("/images/meals/"))
            {
                DeleteImageFile(oldPath);
            }
        }

        // Deletes the specified image file from the server
        private void DeleteImageFile(string imagePath)
        {
            try
            {
                // Build the full physical path and check if the file exists
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _logger.LogInformation($"Deleted image: {filePath}");
                }
            }
            catch (Exception ex)
            {
                // Log a warning if image deletion fails
                _logger.LogWarning(ex, $"Could not delete image: {imagePath}");
            }
        }

        // Saves the uploaded meal image to a dedicated folder on the server
        private async Task<string> SaveMealImage(IFormFile picture)
        {
            // Ensure the folder structure for meal images exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "meals");
            Directory.CreateDirectory(uploadsFolder);

            // Create a unique file name for the image to avoid collisions
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(picture.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Copy the uploaded file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }

            // Return the new image path for storing in the database
            return $"/images/meals/{uniqueFileName}";
        }

        #endregion
    }
}
