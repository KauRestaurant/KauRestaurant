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

        public IActionResult Add() => View("~/Views/Admin/AddMeal.cshtml", new Meal());

        public async Task<IActionResult> Edit(int id)
        {
            var meal = await _context.Meals
                .Include(m => m.Reviews)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(m => m.MealID == id);

            if (meal == null)
            {
                TempData["ErrorMessage"] = "الوجبة غير موجودة";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/EditMeal.cshtml", meal);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMeal(Meal meal, IFormFile Picture)
        {
            try
            {
                if (!ValidateMealBasics(meal))
                {
                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها والوصف مطلوبة";
                    return RedirectToAction(nameof(Add));
                }

                SetDefaultNutritionalValues(meal);

                // Handle picture upload
                meal.PicturePath = Picture != null && Picture.Length > 0
                    ? await SaveMealImage(Picture)
                    : "/images/meal.png";

                _context.Add(meal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating meal");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء إضافة الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Add));
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
                if (string.IsNullOrEmpty(MealName) || string.IsNullOrEmpty(MealCategory) || string.IsNullOrEmpty(MealType))
                {
                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها مطلوبة";
                    return RedirectToAction(nameof(Edit), new { id = MealID });
                }

                var meal = await _context.Meals.FindAsync(MealID);
                if (meal == null)
                {
                    TempData["ErrorMessage"] = "الوجبة غير موجودة";
                    return RedirectToAction(nameof(Index));
                }

                // Store the old picture path
                string oldPicturePath = meal.PicturePath;

                // Update basic properties
                UpdateMealProperties(meal, MealName, Description, Calories, Protein, Carbs, Fat, MealCategory, MealType);

                // Handle image changes
                await HandleMealImage(meal, oldPicturePath, Picture, RemoveImage);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تحديث الوجبة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating meal ID: {MealID}");
                TempData["ErrorMessage"] = $"حدث خطأ أثناء تحديث الوجبة: {ex.Message}";
                return RedirectToAction(nameof(Edit), new { id = MealID });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMeal(int MealID)
        {
            try
            {
                var meal = await _context.Meals
                    .Include(m => m.MenuMeals)
                    .Include(m => m.Reviews)
                    .FirstOrDefaultAsync(m => m.MealID == MealID);

                if (meal == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على الوجبة";
                    return RedirectToAction(nameof(Index));
                }

                // Clean up related records
                if (meal.MenuMeals?.Any() == true)
                {
                    _context.MenuMeals.RemoveRange(meal.MenuMeals);
                }

                if (meal.Reviews?.Any() == true)
                {
                    _context.Reviews.RemoveRange(meal.Reviews);
                }

                await _context.SaveChangesAsync();

                // Delete image if it's not the default
                if (!string.IsNullOrEmpty(meal.PicturePath) &&
                    !meal.PicturePath.Equals("/images/meal.png") &&
                    meal.PicturePath.StartsWith("/images/meals/"))
                {
                    DeleteImageFile(meal.PicturePath);
                }

                // Remove the meal and save changes
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int reviewId, int returnToMealId)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(reviewId);
                if (review == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على التقييم";
                    return RedirectToAction(nameof(Edit), new { id = returnToMealId });
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف التقييم بنجاح";
                return RedirectToAction(nameof(Edit), new { id = returnToMealId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting review ID: {reviewId}");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف التقييم";
                return RedirectToAction(nameof(Edit), new { id = returnToMealId });
            }
        }



        #region Helper Methods

        private bool ValidateMealBasics(Meal meal)
        {
            return !string.IsNullOrEmpty(meal.MealName) &&
                   !string.IsNullOrEmpty(meal.MealCategory) &&
                   !string.IsNullOrEmpty(meal.MealType) &&
                   !string.IsNullOrEmpty(meal.Description);
        }

        private void SetDefaultNutritionalValues(Meal meal)
        {
            if (meal.Calories <= 0) meal.Calories = 1;
            if (meal.Protein <= 0) meal.Protein = 1;
            if (meal.Carbs <= 0) meal.Carbs = 1;
            if (meal.Fat <= 0) meal.Fat = 1;
        }

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

        private async Task HandleMealImage(Meal meal, string oldPath, IFormFile picture, bool removeImage)
        {
            if (removeImage)
            {
                meal.PicturePath = "/images/meal.png";
                DeleteOldImageIfNeeded(oldPath);
            }
            else if (picture != null && picture.Length > 0)
            {
                meal.PicturePath = await SaveMealImage(picture);
                DeleteOldImageIfNeeded(oldPath);
            }
        }

        private void DeleteOldImageIfNeeded(string oldPath)
        {
            if (!string.IsNullOrEmpty(oldPath) &&
                !oldPath.Equals("/images/meal.png") &&
                oldPath.StartsWith("/images/meals/"))
            {
                DeleteImageFile(oldPath);
            }
        }

        private void DeleteImageFile(string imagePath)
        {
            try
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _logger.LogInformation($"Deleted image: {filePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not delete image: {imagePath}");
            }
        }

        private async Task<string> SaveMealImage(IFormFile picture)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "meals");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(picture.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }

            return $"/images/meals/{uniqueFileName}";
        }

        #endregion
    }
}
