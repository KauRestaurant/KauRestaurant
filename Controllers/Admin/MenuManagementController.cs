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
    [Authorize(Roles = "A1,A2")]
    public class MenuManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuManagementController> _logger;

        public MenuManagementController(ApplicationDbContext context, ILogger<MenuManagementController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Get all menus with their related meals
            var menus = await _context.Menus
                .Include(m => m.MenuMeals)
                .ThenInclude(mm => mm.Meal)
                .ToListAsync();

            // Get all available meals for dropdowns
            var allMeals = await _context.Meals.ToListAsync();

            // Pass data to the view
            ViewBag.AllMeals = allMeals;

            return View("~/Views/Admin/MenuManagement.cshtml", menus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMenu(Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<int, int>>>> mealSelections)
        {
            try
            {
                // Process each menu (day)
                foreach (var menuEntry in mealSelections)
                {
                    int menuId = menuEntry.Key;

                    // Load the menu with its meals and ensure Meal is loaded for each MenuMeal
                    var menu = await _context.Menus
                        .Include(m => m.MenuMeals)
                        .ThenInclude(mm => mm.Meal)
                        .FirstOrDefaultAsync(m => m.MenuID == menuId);

                    if (menu == null)
                    {
                        _logger.LogWarning($"Menu with ID {menuId} not found");
                        continue;
                    }

                    // Track all menu meals we want to keep
                    var updatedMenuMealIds = new HashSet<int>();
                    var existingMenuMeals = menu.MenuMeals.ToList(); // Create a copy to avoid modification during iteration

                    // Process the selections
                    foreach (var categoryEntry in menuEntry.Value)
                    {
                        string category = categoryEntry.Key;

                        foreach (var typeEntry in categoryEntry.Value)
                        {
                            string mealType = typeEntry.Key;

                            // Find meals of this category and type that already exist in the menu
                            var categoryTypeMeals = existingMenuMeals
                                .Where(mm => mm.Meal?.MealCategory == category && mm.Meal?.MealType == mealType)
                                .ToList();

                            foreach (var indexEntry in typeEntry.Value)
                            {
                                int index = indexEntry.Key;
                                int selectedMealId = indexEntry.Value;

                                // Skip if no meal was selected (value = 0)
                                if (selectedMealId == 0)
                                {
                                    continue;
                                }

                                // Try to find an existing MenuMeal to update
                                MenuMeal menuMeal = null;

                                // If we have enough entries for this category+type
                                if (index < categoryTypeMeals.Count)
                                {
                                    menuMeal = categoryTypeMeals[index];
                                    menuMeal.MealID = selectedMealId;
                                    updatedMenuMealIds.Add(menuMeal.MenuMealID);
                                }
                                else
                                {
                                    // Need to create a new one
                                    menuMeal = new MenuMeal
                                    {
                                        MenuID = menuId,
                                        MealID = selectedMealId
                                    };
                                    _context.MenuMeals.Add(menuMeal);
                                }
                            }
                        }
                    }

                    // Remove any MenuMeals that weren't updated
                    var menuMealsToRemove = existingMenuMeals
                        .Where(mm => !updatedMenuMealIds.Contains(mm.MenuMealID))
                        .ToList();

                    foreach (var menuMealToRemove in menuMealsToRemove)
                    {
                        _context.MenuMeals.Remove(menuMealToRemove);
                    }
                }

                // Save all changes
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حفظ القائمة بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving menu");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ القائمة: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
