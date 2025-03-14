using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KauRestaurant.Controllers.User
{
    public class MealController : Controller
    {
        private readonly ILogger<MealController> _logger;
        private readonly ApplicationDbContext _context;

        public MealController(ILogger<MealController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("User/Meal/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            // Updated to use many-to-many relationship
            var meal = await _context.Meals
                .Include(m => m.MenuMeals)
                .ThenInclude(mm => mm.Menu)
                .Include(m => m.Reviews)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(m => m.MealID == id);

            if (meal == null)
            {
                return NotFound();
            }

            return View("~/Views/User/Meal.cshtml", meal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
