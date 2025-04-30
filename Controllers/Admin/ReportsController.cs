using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace KauRestaurant.Controllers.Admin
{
    // Restricts access to admins with roles "A1" or "A2"
    [Authorize(Roles = "A1,A2")]
    public class ReportsController : Controller
    {
        // Provides access to the database
        private readonly ApplicationDbContext _context;
        // Assists with logging operations within this controller
        private readonly ILogger<ReportsController> _logger;

        // Constructor injecting the database context and logging service
        public ReportsController(ApplicationDbContext context, ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Displays the main Reports view and populates lists of meal and ticket types
        public IActionResult Index()
        {
            // Fetch unique meal types from the Meals table
            ViewBag.MealTypes = _context.Meals
                .Select(m => m.MealType)
                .Distinct()
                .ToList();

            // Provide a static list of ticket types
            ViewBag.TicketTypes = new List<string> { "الإفطار", "الغداء" };

            // Return the corresponding Razor view
            return View("~/Views/Admin/Reports.cshtml");
        }

        // Retrieves ticket purchase and redemption statistics for the last six months
        [HttpGet]
        public IActionResult GetTicketStatistics(string ticketType = null)
        {
            try
            {
                // Determine the current date and list of months to process
                var today = DateTime.Today;
                var months = new List<DateTime>();
                for (int i = 5; i >= 0; i--)
                {
                    months.Add(today.AddMonths(-i));
                }

                // Convert each month to a formatted label
                var labels = months.Select(m => m.ToString("MMM yyyy")).ToArray();

                // Prepare arrays to store purchased and redeemed counts
                var purchasedTickets = new int[6];
                var redeemedTickets = new int[6];

                // Iterate through the months and compute ticket statistics
                for (int i = 0; i < 6; i++)
                {
                    var startOfMonth = new DateTime(months[i].Year, months[i].Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Base query for purchased tickets
                    var purchasedQuery = _context.Tickets
                        .Where(t => t.Order.OrderDate >= startOfMonth && t.Order.OrderDate <= endOfMonth);

                    // Base query for redeemed tickets
                    var redeemedQuery = _context.Tickets
                        .Where(t => t.IsRedeemed && t.Order.OrderDate >= startOfMonth && t.Order.OrderDate <= endOfMonth);

                    // If a ticket type is specified, filter by that type
                    if (!string.IsNullOrEmpty(ticketType))
                    {
                        purchasedQuery = purchasedQuery.Where(t => t.MealType == ticketType);
                        redeemedQuery = redeemedQuery.Where(t => t.MealType == ticketType);
                    }

                    // Count purchased and redeemed tickets
                    purchasedTickets[i] = purchasedQuery.Count();
                    redeemedTickets[i] = redeemedQuery.Count();
                }

                // Create a data object for chart rendering
                var data = new
                {
                    labels = labels,
                    purchasedTickets = purchasedTickets,
                    redeemedTickets = redeemedTickets
                };

                // Return chart data as JSON
                return Json(data);
            }
            catch (Exception ex)
            {
                // Log any failure and return an error
                _logger.LogError(ex, "Error retrieving ticket statistics");
                return StatusCode(500, "Error retrieving ticket statistics");
            }
        }

        // Retrieves rating data for meals, returning top and bottom five by rating
        [HttpGet]
        public IActionResult GetMealRatings(string mealType = null)
        {
            try
            {
                // Base query for meals that have reviews
                var mealsQuery = _context.Meals
                    .Where(m => m.Reviews.Count > 0);

                // If a meal type filter is provided, apply it
                if (!string.IsNullOrEmpty(mealType))
                {
                    mealsQuery = mealsQuery.Where(m => m.MealType == mealType);
                }

                // Select each meal's average rating
                var mealsWithRatings = mealsQuery
                    .Select(m => new
                    {
                        name = m.MealName,
                        rating = m.Reviews.Average(r => r.Rating)
                    })
                    .ToList();

                // Determine the five highest rated meals
                var topRated = mealsWithRatings
                    .OrderByDescending(m => m.rating)
                    .Take(5)
                    .ToList();

                // Determine the five lowest rated meals
                var lowestRated = mealsWithRatings
                    .OrderBy(m => m.rating)
                    .Take(5)
                    .ToList();

                // Combine the rating information for sending back to the client
                var data = new
                {
                    topRated = topRated,
                    lowestRated = lowestRated
                };

                // Return the meal rating data as JSON
                return Json(data);
            }
            catch (Exception ex)
            {
                // Log any errors and return an error response
                _logger.LogError(ex, "Error retrieving meal ratings");
                return StatusCode(500, "Error retrieving meal ratings");
            }
        }
    }
}
