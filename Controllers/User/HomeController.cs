using System.Diagnostics;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using KauRestaurant.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

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
            var arabicDay = GetArabicDayName(DateTime.Now.DayOfWeek);

            // Get today's menu with related data
            var todayMenu = await _context.Menus
                .Include(m => m.MenuMeals)
                    .ThenInclude(mm => mm.Meal)
                        .ThenInclude(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Day == arabicDay);

            // Get restaurant info
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            // Create view model with basic data
            var categories = new[] { "الإفطار", "الغداء" };
            var viewModel = new IndexViewModel
            {
                Menu = todayMenu,
                Restaurant = restaurant,
                CurrentDay = DateTime.Now.DayOfWeek,
                Categories = categories,
                MealsByCategory = new Dictionary<string, List<MealWithMetadata>>()
            };

            // Process menu meals if available
            if (todayMenu?.MenuMeals != null)
            {
                var mealsByCategory = todayMenu.MenuMeals
                    .Select(mm => mm.Meal)
                    .GroupBy(m => m.MealCategory)
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Process each category
                foreach (var category in categories)
                {
                    viewModel.MealsByCategory[category] = ProcessCategoryMeals(
                        mealsByCategory.GetValueOrDefault(category, new List<Meal>()));
                    viewModel.CategoryIcons[category] = GetCategoryIcon(category);
                }
            }

            return View("~/Views/User/Index.cshtml", viewModel);
        }

        private string GetArabicDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الإثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                DayOfWeek.Friday => "الجمعة",
                DayOfWeek.Saturday => "السبت",
                _ => "الأحد"
            };
        }

        private List<MealWithMetadata> ProcessCategoryMeals(List<Meal> meals)
        {
            var result = new List<MealWithMetadata>();

            var mealsByType = meals
                .GroupBy(m => m.MealType)
                .OrderBy(g => g.Key != "الطبق الرئيسي")
                .ThenBy(g => g.Key != "طبق جانبي")
                .ThenBy(g => g.Key != "حلوى")
                .ThenBy(g => g.Key != "مشروب");

            foreach (var mealGroup in mealsByType)
            {
                foreach (var meal in mealGroup)
                {
                    var rating = CalculateMealRating(meal.Reviews);

                    result.Add(new MealWithMetadata
                    {
                        Meal = meal,
                        Type = mealGroup.Key,
                        TypeStyle = GetMealTypeStyle(mealGroup.Key),
                        Rating = rating
                    });
                }
            }

            return result;
        }

        private MealRatingInfo CalculateMealRating(ICollection<Review> reviews)
        {
            var hasReviews = reviews != null && reviews.Any();
            if (!hasReviews)
            {
                return new MealRatingInfo { HasReviews = false };
            }

            var averageRating = (float)reviews.Average(r => r.Rating);
            var fullStars = (int)Math.Floor(averageRating);
            var hasHalfStar = averageRating - fullStars >= 0.5;

            return new MealRatingInfo
            {
                HasReviews = true,
                ReviewsCount = reviews.Count,
                AverageRating = averageRating,
                FullStars = fullStars,
                HasHalfStar = hasHalfStar,
                EmptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0)
            };
        }


        private string GetMealTypeStyle(string mealType)
        {
            return mealType switch
            {
                "الطبق الرئيسي" => "bg-main-dish",
                "طبق جانبي" => "bg-side-dish",
                "حلوى" => "bg-dessert",
                "مشروب" => "bg-drink",
                _ => "bg-success"
            };
        }

        private (string icon, string bgColor) GetCategoryIcon(string category)
        {
            return category switch
            {
                "الإفطار" => ("bi-sun", "bg-breakfast"),
                "الغداء" => ("bi-brightness-high", "bg-lunch"),
                _ => ("bi-question", "#6c757d")
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class IndexViewModel
    {
        public Menu Menu { get; set; }
        public Restaurant Restaurant { get; set; }
        public DayOfWeek CurrentDay { get; set; }
        public string[] Categories { get; set; }
        public Dictionary<string, List<MealWithMetadata>> MealsByCategory { get; set; }
        public Dictionary<string, (string icon, string bgColor)> CategoryIcons { get; set; } = new();
    }

    public class MealWithMetadata
    {
        public Meal Meal { get; set; }
        public string Type { get; set; }
        public string TypeStyle { get; set; }
        public MealRatingInfo Rating { get; set; }
    }

    public class MealRatingInfo
    {
        public bool HasReviews { get; set; }
        public int ReviewsCount { get; set; }
        public float AverageRating { get; set; }
        public int FullStars { get; set; }
        public bool HasHalfStar { get; set; }
        public int EmptyStars { get; set; }
    }
}
