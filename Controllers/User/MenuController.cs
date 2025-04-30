using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks; // Required for async/await

namespace KauRestaurant.Controllers.User
{
    // Handles display of the weekly menu for end users
    public class MenuController : Controller
    {
        // Logger for informational messages or error tracking
        private readonly ILogger<MenuController> _logger;
        // Database context for data access
        private readonly ApplicationDbContext _context;

        // Constructor injecting logger and database context
        public MenuController(ILogger<MenuController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Fetches and displays the entire weekly menu
        public async Task<IActionResult> Index()
        {
            // Retrieve all Menu entities, including related Meal and Review data
            var menus = await _context.Menus
                .Include(m => m.MenuMeals)
                .ThenInclude(mm => mm.Meal)
                .ThenInclude(meal => meal.Reviews)
                .ToListAsync();

            // Retrieve general restaurant information, if available
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
            ViewBag.Restaurant = restaurant;

            // Return the menu list to a specific Razor view
            return View("~/Views/User/Menu.cshtml", menus);
        }
    }
}
