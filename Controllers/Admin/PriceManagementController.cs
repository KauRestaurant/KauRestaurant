using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KauRestaurant.Data;
using KauRestaurant.Models;
using KauRestaurant.Services;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1")]
    public class PriceManagementController : Controller
    {
        private readonly TicketPriceService _ticketPriceService;

        public PriceManagementController(TicketPriceService ticketPriceService)
        {
            _ticketPriceService = ticketPriceService;
        }

        public async Task<IActionResult> Index()
        {
            // Get the current price settings using the service
            var prices = await _ticketPriceService.GetAllTicketPrices();

            var model = new TicketPriceViewModel
            {
                BreakfastPrice = prices["الإفطار"],
                LunchPrice = prices["الغداء"]
            };

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
                    await _ticketPriceService.UpdateTicketPrice("الإفطار", model.BreakfastPrice);

                    // Update lunch price
                    await _ticketPriceService.UpdateTicketPrice("الغداء", model.LunchPrice);

                    TempData["SuccessMessage"] = "تم تحديث أسعار التذاكر بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث الأسعار: " + ex.Message;
                }
            }

            // If we get here, there was a validation error
            // Return the view with the model to show validation errors
            return View("~/Views/Admin/PriceManagement.cshtml", model);
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
}
