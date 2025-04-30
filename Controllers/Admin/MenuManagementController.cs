using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.Admin
{
    // Restricts access to admins who have roles "A1" or "A2"
    [Authorize(Roles = "A1,A2")]
    public class MenuManagementController : Controller
    {
        // Provides database access
        private readonly ApplicationDbContext _context;
        // Assists with logging operations within this controller
        private readonly ILogger<MenuManagementController> _logger;

        // Constructor injecting the database context and logging provider
        public MenuManagementController(ApplicationDbContext context, ILogger<MenuManagementController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Displays all menus along with their associated meals
        public async Task<IActionResult> Index()
        {
            // Load all Menu entities, including MenuMeals and their related Meal data
            var menus = await _context.Menus
                .Include(m => m.MenuMeals)
                .ThenInclude(mm => mm.Meal)
                .ToListAsync();

            // Load all Meals for selection in the dropdowns
            var allMeals = await _context.Meals.ToListAsync();

            // Pass the collected information to the corresponding Razor view
            ViewBag.AllMeals = allMeals;
            return View("~/Views/Admin/MenuManagement.cshtml", menus);
        }

        // Handles modifications to the menu by adding, removing, or updating MenuMeals
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMenu(
            Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<int, int>>>> mealSelections)
        {
            try
            {
                // Process each menu/day in the posted data
                foreach (var menuEntry in mealSelections)
                {
                    int menuId = menuEntry.Key;

                    // Retrieve the target Menu with its existing MenuMeals
                    var menu = await _context.Menus
                        .Include(m => m.MenuMeals)
                        .ThenInclude(mm => mm.Meal)
                        .FirstOrDefaultAsync(m => m.MenuID == menuId);

                    // If the menu wasn't found, log a warning and skip it
                    if (menu == null)
                    {
                        _logger.LogWarning($"Menu with ID {menuId} not found");
                        continue;
                    }

                    // Track MenuMeal entries that remain after processing updates
                    var updatedMenuMealIds = new HashSet<int>();
                    // Make a local copy to safely iterate and modify
                    var existingMenuMeals = menu.MenuMeals.ToList();

                    // Iterate through the category and meal type entries
                    foreach (var categoryEntry in menuEntry.Value)
                    {
                        string category = categoryEntry.Key;

                        foreach (var typeEntry in categoryEntry.Value)
                        {
                            string mealType = typeEntry.Key;

                            // Find existing items matching this category and meal type
                            var categoryTypeMeals = existingMenuMeals
                                .Where(mm => mm.Meal?.MealCategory == category &&
                                             mm.Meal?.MealType == mealType)
                                .ToList();

                            // Process each selected meal
                            foreach (var indexEntry in typeEntry.Value)
                            {
                                int index = indexEntry.Key;
                                int selectedMealId = indexEntry.Value;

                                // Skip slots where no meal was chosen
                                if (selectedMealId == 0)
                                {
                                    continue;
                                }

                                // Update an existing MenuMeal if possible; otherwise, create a new one
                                if (index < categoryTypeMeals.Count)
                                {
                                    // Reuse existing entry
                                    var menuMeal = categoryTypeMeals[index];
                                    menuMeal.MealID = selectedMealId;
                                    updatedMenuMealIds.Add(menuMeal.MenuMealID);
                                }
                                else
                                {
                                    // Create a new MenuMeal
                                    var newMenuMeal = new MenuMeal
                                    {
                                        MenuID = menuId,
                                        MealID = selectedMealId
                                    };
                                    _context.MenuMeals.Add(newMenuMeal);
                                }
                            }
                        }
                    }

                    // Remove any existing MenuMeals not updated in this pass
                    var menuMealsToRemove = existingMenuMeals
                        .Where(mm => !updatedMenuMealIds.Contains(mm.MenuMealID))
                        .ToList();
                    foreach (var menuMealToRemove in menuMealsToRemove)
                    {
                        _context.MenuMeals.Remove(menuMealToRemove);
                    }
                }

                // Commit all changes to the database
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم حفظ القائمة بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log any issues and display a user-friendly message
                _logger.LogError(ex, "Error saving menu");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ القائمة: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
