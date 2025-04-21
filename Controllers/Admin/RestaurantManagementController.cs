using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Add this for IFormFile
using KauRestaurant.Data;
using KauRestaurant.Models;
using KauRestaurant.Services;
using System;
using System.IO; // Add this for Path operations
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1")]
    public class RestaurantManagementController : Controller
    {
        private readonly TicketPriceService _ticketPriceService;
        private readonly ApplicationDbContext _context;

        public RestaurantManagementController(TicketPriceService ticketPriceService, ApplicationDbContext context)
        {
            _ticketPriceService = ticketPriceService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prices = await _ticketPriceService.GetAllTicketPrices();
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync() ?? new Restaurant();

            var model = new RestaurantManagementViewModel
            {
                TicketPrices = new TicketPriceViewModel
                {
                    BreakfastPrice = prices["الإفطار"],
                    LunchPrice = prices["الغداء"]
                },
                Restaurant = restaurant
            };

            return View("~/Views/Admin/RestaurantManagement.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAll(RestaurantManagementViewModel model)
        {
            try
            {
                // Update ticket prices
                await _ticketPriceService.UpdateTicketPrice("الإفطار", model.TicketPrices.BreakfastPrice);
                await _ticketPriceService.UpdateTicketPrice("الغداء", model.TicketPrices.LunchPrice);

                // Update restaurant information
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
                bool isNewRestaurant = false;
                if (restaurant == null)
                {
                    restaurant = new Restaurant();
                    isNewRestaurant = true;
                }

                // Update other restaurant properties - photo handling removed
                restaurant.Location = model.Restaurant.Location;
                restaurant.GoogleMapsLink = model.Restaurant.GoogleMapsLink;
                restaurant.ContactNumber = model.Restaurant.ContactNumber;
                restaurant.Email = model.Restaurant.Email;
                restaurant.WorkingDays = model.Restaurant.WorkingDays;
                restaurant.BreakfastHours = model.Restaurant.BreakfastHours;
                restaurant.LunchHours = model.Restaurant.LunchHours;
                restaurant.IsOpen = model.Restaurant.IsOpen;

                // Keep existing photo - removed photo updating functionality
                if (isNewRestaurant)
                {
                    _context.Restaurants.Add(restaurant);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تحديث معلومات المطعم وأسعار التذاكر بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث المعلومات: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }

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

    public class RestaurantManagementViewModel
    {
        public TicketPriceViewModel TicketPrices { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
