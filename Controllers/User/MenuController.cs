using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.User
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly ApplicationDbContext _context;

        public MenuController(ILogger<MenuController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus
                .Include(m => m.MenuMeals)
                .ThenInclude(mm => mm.Meal)
                .ThenInclude(m => m.Reviews)
                .ToListAsync();

            return View("~/Views/User/Menu.cshtml", menus);
        }
    }
}
