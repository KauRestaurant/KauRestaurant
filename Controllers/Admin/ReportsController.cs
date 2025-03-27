using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1,A2")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ApplicationDbContext context, ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("~/Views/Admin/Reports.cshtml");
        }

        public class TicketStatistics
        {
            public List<int> PurchasedTickets { get; set; }
            public List<int> RedeemedTickets { get; set; }
            public List<string> Labels { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetTicketStatistics()
        {
            try
            {
                var today = DateTime.Today;
                var last6Months = Enumerable.Range(0, 6)
                    .Select(i => today.AddMonths(-i))
                    .OrderBy(d => d)
                    .ToList();

                var statistics = new TicketStatistics
                {
                    PurchasedTickets = new List<int>(),
                    RedeemedTickets = new List<int>(),
                    Labels = last6Months.Select(d => d.ToString("MMM yyyy")).ToList()
                };

                foreach (var date in last6Months)
                {
                    var startDate = new DateTime(date.Year, date.Month, 1);
                    var endDate = startDate.AddMonths(1);

                    // Get purchased tickets count
                    var purchasedCount = await _context.Tickets
                        .Where(t => t.Order.OrderDate >= startDate && t.Order.OrderDate < endDate)
                        .CountAsync();

                    // Get redeemed tickets count
                    var redeemedCount = await _context.Tickets
                        .Where(t => t.Order.OrderDate >= startDate && t.Order.OrderDate < endDate && t.IsRedeemed)
                        .CountAsync();

                    statistics.PurchasedTickets.Add(purchasedCount);
                    statistics.RedeemedTickets.Add(redeemedCount);
                }

                return Json(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket statistics");
                return StatusCode(500, "Error retrieving ticket statistics");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMealRatings()
        {
            try
            {
                // Get top rated meals
                var topRatedMeals = await _context.Meals
                    .Where(m => m.Reviews.Count > 0)
                    .OrderByDescending(m => m.Reviews.Average(r => r.Rating))
                    .Take(5)
                    .Select(m => new
                    {
                        name = m.MealName,
                        rating = Math.Round(m.Reviews.Average(r => r.Rating), 1)
                    })
                    .ToListAsync();

                // Get lowest rated meals
                var lowestRatedMeals = await _context.Meals
                    .Where(m => m.Reviews.Count > 0)
                    .OrderBy(m => m.Reviews.Average(r => r.Rating))
                    .Take(5)
                    .Select(m => new
                    {
                        name = m.MealName,
                        rating = Math.Round(m.Reviews.Average(r => r.Rating), 1)
                    })
                    .ToListAsync();

                return Json(new
                {
                    topRated = topRatedMeals,
                    lowestRated = lowestRatedMeals
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving meal ratings");
                return StatusCode(500, "Error retrieving meal ratings");
            }
        }
    }
}
