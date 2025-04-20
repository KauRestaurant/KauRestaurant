using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

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
            // Fetch meal types and ticket types for the view
            ViewBag.MealTypes = _context.Meals
                .Select(m => m.MealType)
                .Distinct()
                .ToList();

            ViewBag.TicketTypes = new List<string> { "الإفطار", "الغداء"};

            return View("~/Views/Admin/Reports.cshtml");
        }

        [HttpGet]
        public IActionResult GetTicketStatistics(string ticketType = null)
        {
            try
            {
                // Get last 6 months
                var today = DateTime.Today;
                var months = new List<DateTime>();

                for (int i = 5; i >= 0; i--)
                {
                    months.Add(today.AddMonths(-i));
                }

                var labels = months.Select(m => m.ToString("MMM yyyy")).ToArray();

                var purchasedTickets = new int[6];
                var redeemedTickets = new int[6];

                for (int i = 0; i < 6; i++)
                {
                    var startOfMonth = new DateTime(months[i].Year, months[i].Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Base queries
                    var purchasedQuery = _context.Tickets
                        .Where(t => t.Order.OrderDate >= startOfMonth && t.Order.OrderDate <= endOfMonth);

                    var redeemedQuery = _context.Tickets
                        .Where(t => t.IsRedeemed && t.Order.OrderDate >= startOfMonth && t.Order.OrderDate <= endOfMonth);

                    // Apply ticket type filter if provided
                    if (!string.IsNullOrEmpty(ticketType))
                    {
                        purchasedQuery = purchasedQuery.Where(t => t.MealType == ticketType);
                        redeemedQuery = redeemedQuery.Where(t => t.MealType == ticketType);
                    }

                    // Count tickets
                    purchasedTickets[i] = purchasedQuery.Count();
                    redeemedTickets[i] = redeemedQuery.Count();
                }

                var data = new
                {
                    labels = labels,
                    purchasedTickets = purchasedTickets,
                    redeemedTickets = redeemedTickets
                };

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket statistics");
                return StatusCode(500, "Error retrieving ticket statistics");
            }
        }

        [HttpGet]
        public IActionResult GetMealRatings(string mealType = null)
        {
            try
            {
                // Base query for meals with reviews
                var mealsQuery = _context.Meals
                    .Where(m => m.Reviews.Count > 0);

                // Apply meal type filter if provided
                if (!string.IsNullOrEmpty(mealType))
                {
                    mealsQuery = mealsQuery.Where(m => m.MealType == mealType);
                }

                var mealsWithRatings = mealsQuery
                    .Select(m => new
                    {
                        name = m.MealName,
                        rating = m.Reviews.Average(r => r.Rating)
                    })
                    .ToList();

                // Get top 5 highest rated meals
                var topRated = mealsWithRatings
                    .OrderByDescending(m => m.rating)
                    .Take(5)
                    .ToList();

                // Get top 5 lowest rated meals
                var lowestRated = mealsWithRatings
                    .OrderBy(m => m.rating)
                    .Take(5)
                    .ToList();

                var data = new
                {
                    topRated = topRated,
                    lowestRated = lowestRated
                };

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving meal ratings");
                return StatusCode(500, "Error retrieving meal ratings");
            }
        }
    }
}
