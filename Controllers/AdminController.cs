using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourNamespace.Models;

namespace KauRestaurant.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TicketQrService _ticketQrService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<KauRestaurantUser> userManager,
            ApplicationDbContext context,
            TicketQrService ticketQrService,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _context = context;
            _ticketQrService = ticketQrService;
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult ScanTicket()
        {
            return View();
        }

        public class TicketQrRequest
        {
            public string qrData { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateTicket([FromBody] TicketQrRequest request)
        {
            _logger.LogInformation($"Received ticket request: {request?.qrData ?? "null"}");
            
            if (request == null || string.IsNullOrEmpty(request.qrData))
            {
                _logger.LogWarning("No QR data provided");
                return Json(new { success = false, message = "لم يتم تقديم بيانات QR" });
            }

            try
            {
                // Verify the QR data signature
                bool isValid = _ticketQrService.VerifyTicketQrData(request.qrData);

                if (!isValid)
                {
                    _logger.LogWarning("Invalid QR signature");
                    return Json(new { success = false, message = "QR غير صالح أو تم العبث به" });
                }

                // Parse the QR data
                var ticketData = JsonSerializer.Deserialize<TicketQrService.TicketQrData>(request.qrData);

                // Find the ticket in the database
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.QRCode == ticketData.TicketId);

                if (ticket == null)
                {
                    _logger.LogWarning($"Ticket not found: {ticketData.TicketId}");
                    return Json(new { success = false, message = "التذكرة غير موجودة" });
                }

                if (ticket.IsRedeemed)
                {
                    _logger.LogWarning($"Ticket already redeemed: {ticketData.TicketId}");
                    return Json(new { success = false, message = "تم استخدام هذه التذكرة مسبقاً" });
                }

                // Mark the ticket as redeemed
                ticket.IsRedeemed = true;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully validated ticket: {ticketData.TicketId}");

                return Json(new
                {
                    success = true,
                    message = "تم التحقق من التذكرة بنجاح",
                    ticketDetails = new
                    {
                        ticketId = ticket.TicketID,
                        mealType = ticket.MealType,
                        userName = ticketData.UserFullName
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating ticket");
                return Json(new { success = false, message = $"خطأ: {ex.Message}" });
            }
        }
    }
}
