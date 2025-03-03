using System.Diagnostics;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using KauRestaurant.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KauRestaurant.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;

        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
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

            // Get today's menu with meals
            var todayMenu = await _context.Menus
                .Include(m => m.Meals)
                .FirstOrDefaultAsync(m => m.Day == arabicDay);

            return View(todayMenu);
        }

        public async Task<IActionResult> Menu()
        {
            var menus = await _context.Menus
                .Include(m => m.Meals)
                .ToListAsync();

            return View("menu", menus);
        }

        public async Task<IActionResult> Meal(int id)
        {
            var meal = await _context.Meals
                .Include(m => m.Menu)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(m => m.MealID == id);

            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        [HttpPost]
        public async Task<IActionResult> submitReview(int mealId, int rating, string comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "يجب تسجيل الدخول لتقديم مراجعة";
                return RedirectToAction("Meal", new { id = mealId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = new Review
            {
                MealID = mealId,
                CustomerID = userId,
                Rating = rating,
                ReviewText = comment,
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Meal", new { id = mealId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
