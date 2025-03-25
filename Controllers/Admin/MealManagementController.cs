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
        public async Task<IActionResult> CreateMeal(string MealName, string Description,
            int Calories, int Protein, int Carbs, int Fat, string MealCategory, string MealType,
            IFormFile Picture)
        {
            try
            {
                // Manual validation
                if (string.IsNullOrEmpty(MealName) || string.IsNullOrEmpty(MealCategory) || string.IsNullOrEmpty(MealType))
                {
                    TempData["ErrorMessage"] = "اسم الوجبة وفئتها ونوعها مطلوبة";
                    return RedirectToAction(nameof(Index));
                }

                // Create new meal
                var meal = new Meal
                {
                    MealName = MealName,
                    Description = Description ?? string.Empty,
                    Calories = Calories,
                    Protein = Protein,
                    Carbs = Carbs,
                    Fat = Fat,
                    MealCategory = MealCategory,
                    MealType = MealType
                };

                // Handle picture upload
                if (Picture != null && Picture.Length > 0)
                {
                    meal.PicturePath = await SaveMealImage(Picture);
                }
                else
                {
                    // Set default image
                    meal.PicturePath = "/images/meal-placeholder.png";
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
            IFormFile Picture)
        {
            try
            {
                _logger.LogInformation($"Update meal request for ID: {MealID}, Name: {MealName}");

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

                // Handle picture upload
                if (Picture != null && Picture.Length > 0)
                {
                    // Save new image
                    meal.PicturePath = await SaveMealImage(Picture);

                    // Delete old image if it's not the default and it exists
                    if (!string.IsNullOrEmpty(oldPicturePath) &&
                        !oldPicturePath.Equals("/images/meal-placeholder.png") &&
                        oldPicturePath.StartsWith("/images/meals/"))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, oldPicturePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldFilePath);
                                _logger.LogInformation($"Deleted old image: {oldFilePath}");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, $"Could not delete old image: {oldFilePath}");
                                // Continue execution even if the file delete fails
                            }
                        }
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
                    !meal.PicturePath.Equals("/images/meal-placeholder.png") &&
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
