using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KauRestaurant.Data;
using KauRestaurant.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1")]
    public class PriceManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PriceManagementController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            // Get the current price settings
            var breakfastPrice = await _context.TicketPrices.FirstOrDefaultAsync(tp => tp.MealType == "الإفطار");
            var lunchPrice = await _context.TicketPrices.FirstOrDefaultAsync(tp => tp.MealType == "الغداء");
            var dinnerPrice = await _context.TicketPrices.FirstOrDefaultAsync(tp => tp.MealType == "العشاء");

            var model = new TicketPriceViewModel
            {
                BreakfastPrice = breakfastPrice?.Price ?? 7m,
                BreakfastLastUpdated = breakfastPrice?.LastUpdated ?? DateTime.Now,

                LunchPrice = lunchPrice?.Price ?? 10m,
                LunchLastUpdated = lunchPrice?.LastUpdated ?? DateTime.Now,

                DinnerPrice = dinnerPrice?.Price ?? 10m,
                DinnerLastUpdated = dinnerPrice?.LastUpdated ?? DateTime.Now
            };

            // Get restaurant information to check if meal times are disabled
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
            ViewBag.Restaurant = restaurant;

            return View("~/Views/Admin/PriceManagement.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrices(TicketPriceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Update breakfast price
                    await UpdateTicketPrice("الإفطار", model.BreakfastPrice);

                    // Update lunch price
                    await UpdateTicketPrice("الغداء", model.LunchPrice);

                    // Update dinner price
                    await UpdateTicketPrice("العشاء", model.DinnerPrice);

                    TempData["SuccessMessage"] = "تم تحديث أسعار التذاكر بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث الأسعار: " + ex.Message;
                }
            }

            // Get restaurant information again for the view
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
            ViewBag.Restaurant = restaurant;

            // If we get here, there was a validation error
            // Return the view with the model to show validation errors
            return View("~/Views/Admin/PriceManagement.cshtml", model);
        }

        // Helper method to get the price for a specific ticket type
        private async Task<decimal> GetTicketPrice(string mealType)
        {
            var ticketPrice = await _context.TicketPrices
                .FirstOrDefaultAsync(tp => tp.MealType == mealType);

            if (ticketPrice == null)
            {
                // Default prices if not found
                decimal defaultPrice = mealType == "الإفطار" ? 7m : 10m;

                // Create new price entry
                ticketPrice = new TicketPrice
                {
                    MealType = mealType,
                    Price = defaultPrice,
                    LastUpdated = DateTime.Now
                };

                _context.TicketPrices.Add(ticketPrice);
                await _context.SaveChangesAsync();

                return defaultPrice;
            }

            return ticketPrice.Price;
        }

        // Helper method to update the price for a specific ticket type
        private async Task UpdateTicketPrice(string mealType, decimal price)
        {
            var ticketPrice = await _context.TicketPrices
                .FirstOrDefaultAsync(tp => tp.MealType == mealType);

            if (ticketPrice == null)
            {
                // Create new entry if it doesn't exist
                ticketPrice = new TicketPrice
                {
                    MealType = mealType,
                    Price = price,
                    LastUpdated = DateTime.Now
                };
                _context.TicketPrices.Add(ticketPrice);
            }
            else
            {
                // Update existing entry
                ticketPrice.Price = price;
                ticketPrice.LastUpdated = DateTime.Now;
                _context.TicketPrices.Update(ticketPrice);
            }

            await _context.SaveChangesAsync();
        }
    }

    public class TicketPriceViewModel
    {
        [Required(ErrorMessage = "يرجى إدخال سعر تذكرة الإفطار")]
        [Range(0.01, 1000.00, ErrorMessage = "يجب أن يكون السعر أكبر من صفر وأقل من 1000")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "سعر تذكرة الإفطار")]
        public decimal BreakfastPrice { get; set; }

        public DateTime BreakfastLastUpdated { get; set; }

        [Required(ErrorMessage = "يرجى إدخال سعر تذكرة الغداء")]
        [Range(0.01, 1000.00, ErrorMessage = "يجب أن يكون السعر أكبر من صفر وأقل من 1000")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "سعر تذكرة الغداء")]
        public decimal LunchPrice { get; set; }

        public DateTime LunchLastUpdated { get; set; }

        [Required(ErrorMessage = "يرجى إدخال سعر تذكرة العشاء")]
        [Range(0.01, 1000.00, ErrorMessage = "يجب أن يكون السعر أكبر من صفر وأقل من 1000")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "سعر تذكرة العشاء")]
        public decimal DinnerPrice { get; set; }

        public DateTime DinnerLastUpdated { get; set; }
    }
}
