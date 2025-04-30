using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;  // Required for IFormFile operations
using KauRestaurant.Data;
using KauRestaurant.Models;
using KauRestaurant.Services;
using System;
using System.IO;                 // Required for handling file paths
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.Admin
{
    // Restricts access to admins who have roles "A1" or "A2"
    [Authorize(Roles = "A1,A2")]
    public class RestaurantManagementController : Controller
    {
        // Allows retrieving and updating ticket prices
        private readonly TicketPriceService _ticketPriceService;
        // Provides database access
        private readonly ApplicationDbContext _context;

        // Constructor injecting ticket price service and database context
        public RestaurantManagementController(
            TicketPriceService ticketPriceService,
            ApplicationDbContext context)
        {
            _ticketPriceService = ticketPriceService;
            _context = context;
        }

        // Displays the main page that allows managing restaurant info and ticket prices
        public async Task<IActionResult> Index()
        {
            // Fetch all current ticket prices
            var prices = await _ticketPriceService.GetAllTicketPrices();
            // Fetch existing restaurant info or create a new record if none exists
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync() ?? new Restaurant();

            // Build a view model combining ticket prices and restaurant details
            var model = new RestaurantManagementViewModel
            {
                TicketPrices = new TicketPriceViewModel
                {
                    BreakfastPrice = prices["الإفطار"],
                    LunchPrice = prices["الغداء"]
                },
                Restaurant = restaurant
            };

            // Return the corresponding Razor view
            return View("~/Views/Admin/RestaurantManagement.cshtml", model);
        }

        // Handles updates to both restaurant information and ticket prices
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAll(RestaurantManagementViewModel model)
        {
            try
            {
                // Update ticket prices from form inputs
                await _ticketPriceService.UpdateTicketPrice("الإفطار", model.TicketPrices.BreakfastPrice);
                await _ticketPriceService.UpdateTicketPrice("الغداء", model.TicketPrices.LunchPrice);

                // Find or create restaurant record for updating
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
                bool isNewRestaurant = false;

                // If there is no existing restaurant, create a new entry
                if (restaurant == null)
                {
                    restaurant = new Restaurant();
                    isNewRestaurant = true;
                }

                // Update relevant restaurant fields using the posted data
                restaurant.Location = model.Restaurant.Location;
                restaurant.GoogleMapsLink = model.Restaurant.GoogleMapsLink;
                restaurant.ContactNumber = model.Restaurant.ContactNumber;
                restaurant.Email = model.Restaurant.Email;
                restaurant.WorkingDays = model.Restaurant.WorkingDays;
                restaurant.BreakfastHours = model.Restaurant.BreakfastHours;
                restaurant.LunchHours = model.Restaurant.LunchHours;
                restaurant.IsOpen = model.Restaurant.IsOpen;

                // If this is a new restaurant, add it to the database
                if (isNewRestaurant)
                {
                    _context.Restaurants.Add(restaurant);
                }

                // Save all modifications to the database
                await _context.SaveChangesAsync();

                // Present a user-friendly success message
                TempData["SuccessMessage"] = "تم تحديث معلومات المطعم وأسعار التذاكر بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // In case of an error, return a friendly message
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث المعلومات: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }

    // View model to handle ticket price fields (breakfast and lunch)
    public class TicketPriceViewModel
    {
        [Required(ErrorMessage = "يرجى إدخال سعر تذكرة الإفطار")]
        [Range(0.01, 1000.00, ErrorMessage = "يجب أن يكون السعر أكبر من صفر وأقل من 1000")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "سعر تذكرة الإفطار")]
        public decimal BreakfastPrice { get; set; }

        [Required(ErrorMessage = "يرجى إدخال سعر تذكرة الغداء")]
        [Range(0.01, 1000.00, ErrorMessage = "يجب أن يكون السعر أكبر من صفر وأقل من 1000")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "سعر تذكرة الغداء")]
        public decimal LunchPrice { get; set; }
    }

    // Combines ticket price data and restaurant model into one structure
    public class RestaurantManagementViewModel
    {
        public TicketPriceViewModel TicketPrices { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
