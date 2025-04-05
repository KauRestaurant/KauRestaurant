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
            return View("~/Views/Admin/Reports.cshtml");
        }

        [HttpGet]
        public IActionResult GetTicketStatistics()
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

                    // Count purchased tickets for this month
                    purchasedTickets[i] = _context.Tickets
                        .Where(t => t.Order.OrderDate >= startOfMonth && t.Order.OrderDate <= endOfMonth)
                        .Count();

                    // Count redeemed tickets for this month
                    redeemedTickets[i] = _context.Tickets
                        .Where(t => t.IsRedeemed && t.Order.OrderDate >= startOfMonth && t.Order.OrderDate <= endOfMonth)
                        .Count();
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
        public IActionResult GetMealRatings()
        {
            try
            {
                // Get top 5 highest rated meals
                var topRated = _context.Meals
                    .Where(m => m.Reviews.Count > 0)
                    .Select(m => new
                    {
                        name = m.MealName,
                        rating = m.Reviews.Average(r => r.Rating)
                    })
                    .OrderByDescending(m => m.rating)
                    .Take(5)
                    .ToList();

                // Get top 5 lowest rated meals
                var lowestRated = _context.Meals
                    .Where(m => m.Reviews.Count > 0)
                    .Select(m => new
                    {
                        name = m.MealName,
                        rating = m.Reviews.Average(r => r.Rating)
                    })
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
