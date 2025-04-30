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

            // Process menu items and organize by meal categories
            var categories = new[] { "الإفطار", "الغداء" };
            var viewModel = new IndexViewModel
            {
                Menu = todayMenu,
                Restaurant = restaurant,
                CurrentDay = DateTime.Now.DayOfWeek,
                Categories = categories,
                MealsByCategory = new Dictionary<string, List<MealWithMetadata>>()
            };

            if (todayMenu != null && todayMenu.MenuMeals != null)
            {
                var mealsByCategory = todayMenu.MenuMeals
                    .Select(mm => mm.Meal)
                    .GroupBy(m => m.MealCategory)
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Process each category
                foreach (var category in categories)
                {
                    var meals = mealsByCategory.ContainsKey(category)
                        ? mealsByCategory[category]
                        : new List<KauRestaurant.Models.Meal>();

                    var mealsByType = meals
                        .GroupBy(m => m.MealType)
                        .OrderBy(g => g.Key != "الطبق الرئيسي")
                        .ThenBy(g => g.Key != "طبق جانبي")
                        .ThenBy(g => g.Key != "حلوى")
                        .ThenBy(g => g.Key != "مشروب")
                        .ToDictionary(g => g.Key, g => g.ToList());

                    var processedMeals = new List<MealWithMetadata>();
                    foreach (var mealGroup in mealsByType)
                    {
                        foreach (var meal in mealGroup.Value)
                        {
                            var reviewRating = new MealRatingInfo
                            {
                                HasReviews = meal.Reviews != null && meal.Reviews.Any()
                            };

                            if (reviewRating.HasReviews)
                            {
                                reviewRating.ReviewsCount = meal.Reviews.Count;
                                reviewRating.AverageRating = (float)meal.Reviews.Average(r => r.Rating);
                                reviewRating.FullStars = (int)Math.Floor(reviewRating.AverageRating);
                                reviewRating.HasHalfStar = reviewRating.AverageRating - reviewRating.FullStars >= 0.5;
                                reviewRating.EmptyStars = 5 - reviewRating.FullStars - (reviewRating.HasHalfStar ? 1 : 0);
                            }

                            processedMeals.Add(new MealWithMetadata
                            {
                                Meal = meal,
                                Type = mealGroup.Key,
                                TypeStyle = GetMealTypeStyle(mealGroup.Key),
                                Rating = reviewRating
                            });
                        }
                    }

                    viewModel.MealsByCategory[category] = processedMeals;
                    viewModel.CategoryIcons[category] = GetCategoryIcon(category);
                }
            }

            return View("~/Views/User/Index.cshtml", viewModel);
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
