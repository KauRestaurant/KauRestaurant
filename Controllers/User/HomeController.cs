using System.Diagnostics;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using KauRestaurant.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KauRestaurant.Controllers.User
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get current day in Arabic
            var englishDay = DateTime.Now.DayOfWeek.ToString();
            var arabicDay = englishDay switch
            {
                "Sunday" => "الأحد",
                "Monday" => "الإثنين",
                "Tuesday" => "الثلاثاء",
                "Wednesday" => "الأربعاء",
                "Thursday" => "الخميس",
                "Friday" => "الجمعة",
                "Saturday" => "السبت",
                _ => "الأحد"
            };

            // Get today's menu
            var todayMenu = await _context.Menus
                .Include(m => m.MenuMeals)
                .ThenInclude(mm => mm.Meal)
                .ThenInclude(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Day == arabicDay);

            // Get restaurant info
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
            ViewBag.Restaurant = restaurant;

            return View("~/Views/User/Index.cshtml", todayMenu);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
