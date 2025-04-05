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
    [Authorize(Roles = "A1,A2")]
    public class MealManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MealManagementController> _logger;

        public MealManagementController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ILogger<MealManagementController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var meals = await _context.Meals
                    .Include(m => m.Reviews)
                    .OrderBy(m => m.MealCategory)
                    .ThenBy(m => m.MealType)
                    .ThenBy(m => m.MealName)
                    .ToListAsync();

                return View("~/Views/Admin/MealManagement.cshtml", meals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading meals");
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل الوجبات";
                return View("~/Views/Admin/MealManagement.cshtml", new List<Meal>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMeal([Bind(Prefix = "newMeal")] Meal meal, IFormFile Picture)
        {
            try
            {
                // Only check for required text fields
                if (string.IsNullOrEmpty(meal.MealName) || string.IsNullOrEmpty(meal.MealCategory) ||
                    string.IsNullOrEmpty(meal.MealType) || string.IsNullOrEmpty(meal.Description))
                {
                    // Log the validation failure with details for debugging
                    _logger.LogWarning($"Form validation failed: Name={meal.MealName}, Category={meal.MealCategory}, " +
                                      $"Type={meal.MealType}, Description={meal.Description}");

                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها مطلوبة";
                    return RedirectToAction(nameof(Index));
                }

                // Set default values for numeric fields if they're 0
                if (meal.Calories <= 0) meal.Calories = 1;
                if (meal.Protein <= 0) meal.Protein = 1;
                if (meal.Carbs <= 0) meal.Carbs = 1;
                if (meal.Fat <= 0) meal.Fat = 1;

                // Handle picture upload
                if (Picture != null && Picture.Length > 0)
                {
                    meal.PicturePath = await SaveMealImage(Picture);
                }
                else
                {
                    // Set default image path
                    meal.PicturePath = "/images/meal.png";
                }

                _context.Add(meal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating meal");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء إضافة الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMeal(int MealID, string MealName, string Description,
            int Calories, int Protein, int Carbs, int Fat, string MealCategory, string MealType,
            IFormFile Picture, bool RemoveImage = false)
        {
            try
            {
                _logger.LogInformation($"Update meal request for ID: {MealID}, Name: {MealName}, RemoveImage: {RemoveImage}");

                // Manual validation
                if (string.IsNullOrEmpty(MealName) || string.IsNullOrEmpty(MealCategory) || string.IsNullOrEmpty(MealType))
                {
                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها مطلوبة";
                    return RedirectToAction(nameof(Index));
                }

                // Find the meal
                var meal = await _context.Meals.FindAsync(MealID);
                if (meal == null)
                {
                    TempData["ErrorMessage"] = "الوجبة غير موجودة";
                    return RedirectToAction(nameof(Index));
                }

                // Store the old picture path in case we need to delete it
                string oldPicturePath = meal.PicturePath;

                // Update properties
                meal.MealName = MealName;
                meal.Description = Description ?? string.Empty;
                meal.Calories = Calories;
                meal.Protein = Protein;
                meal.Carbs = Carbs;
                meal.Fat = Fat;
                meal.MealCategory = MealCategory;
                meal.MealType = MealType;

                // Handle picture upload or removal
                if (RemoveImage)
                {
                    // User wants to remove the image, set to default
                    meal.PicturePath = "/images/meal.png";

                    // Delete old image if it's not the default and it exists
                    if (!string.IsNullOrEmpty(oldPicturePath) &&
                        !oldPicturePath.Equals("/images/meal.png") &&
                        oldPicturePath.StartsWith("/images/meals/"))
                    {
                        await DeleteImageFile(oldPicturePath);
                    }
                }
                else if (Picture != null && Picture.Length > 0)
                {
                    // Save new image
                    meal.PicturePath = await SaveMealImage(Picture);

                    // Delete old image if it's not the default and it exists
                    if (!string.IsNullOrEmpty(oldPicturePath) &&
                        !oldPicturePath.Equals("/images/meal.png") &&
                        oldPicturePath.StartsWith("/images/meals/"))
                    {
                        await DeleteImageFile(oldPicturePath);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تحديث الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating meal ID: {MealID}");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء تحديث الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to delete image files
        private async Task DeleteImageFile(string imagePath)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    _logger.LogInformation($"Deleted old image: {filePath}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Could not delete old image: {filePath}");
                    // Continue execution even if the file delete fails
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMeal(int MealID)
        {
            try
            {
                // Load the meal with its relationships
                var meal = await _context.Meals
                    .Include(m => m.MenuMeals)
                    .Include(m => m.Reviews)
                    .FirstOrDefaultAsync(m => m.MealID == MealID);

                if (meal == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على الوجبة";
                    return RedirectToAction(nameof(Index));
                }

                // Remove MenuMeal relationships if present
                if (meal.MenuMeals != null && meal.MenuMeals.Any())
                {
                    _context.MenuMeals.RemoveRange(meal.MenuMeals);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Removed {meal.MenuMeals.Count} menu associations for meal ID: {MealID}");
                }

                // Remove reviews if present
                if (meal.Reviews != null && meal.Reviews.Any())
                {
                    _context.Reviews.RemoveRange(meal.Reviews);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Removed {meal.Reviews.Count} reviews for meal ID: {MealID}");
                }

                // Handle image deletion
                if (!string.IsNullOrEmpty(meal.PicturePath) &&
                    !meal.PicturePath.Equals("/images/meal.png") &&
                    meal.PicturePath.StartsWith("/images/meals/"))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, meal.PicturePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath);
                            _logger.LogInformation($"Deleted image file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Could not delete image file: {filePath}");
                            // Continue execution even if the file delete fails
                        }
                    }
                }

                // Finally remove the meal
                _context.Meals.Remove(meal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting meal ID: {MealID}");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء حذف الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to save meal images
        private async Task<string> SaveMealImage(IFormFile picture)
        {
            try
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "meals");
                Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                // Generate unique filename
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }

                _logger.LogInformation($"Saved new image: {filePath}");

                // Return the relative path for database storage
                return $"/images/meals/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving meal image");
                throw; // Re-throw to be handled by the calling method
            }
        }
    }
}
