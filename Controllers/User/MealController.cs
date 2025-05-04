using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks; // Required for async/await

namespace KauRestaurant.Controllers.User
{
    // Controls the display of specific meal details and manages meal reviews
    public class MealController : Controller
    {
        // Logger for handling diagnostics and error messages
        private readonly ILogger<MealController> _logger;
        // Database context for reading and writing meal-related data
        private readonly ApplicationDbContext _context;

        // Constructor injecting both logger and database context
        public MealController(ILogger<MealController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Shows a single meal's details, along with possible menus and reviews
        [Route("User/Meal/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            // Retrieve the meal by ID, including related info like menu entries and reviews
            var meal = await _context.Meals
                .Include(m => m.MenuMeals)
                    .ThenInclude(mm => mm.Menu)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(m => m.MealID == id);

            // If no record found, return a 404 Not Found
            if (meal == null)
            {
                return NotFound();
            }

            // Return the meal details to the corresponding Razor view
            return View("~/Views/User/Meal.cshtml", meal);
        }

        // Handles posting a new review for a given meal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> submitReview(int mealId, int rating, string comment)
        {
            // Enforce authentication; must be signed in to submit a review
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "يجب تسجيل الدخول لتقديم مراجعة";
                return RedirectToAction("Index", new { id = mealId });
            }

            // Retrieve the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                // Create a new Review entity with user-supplied values
                var review = new Review
                {
                    MealID = mealId,
                    CustomerID = userId,
                    Rating = rating,
                    ReviewText = comment,
                    ReviewDate = DateTime.Now
                };

                // Save the new review in the database
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Set success message
                TempData["SuccessMessage"] = "تم إرسال تقييمك بنجاح";
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error submitting review for meal {MealId}", mealId);

                // Set error message
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ التقييم، يرجى المحاولة مرة أخرى";
            }

            // Redirect back to the meal's main page
            return RedirectToAction("Index", new { id = mealId });
        }
    }
}
