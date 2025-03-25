using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1,A2,A3")]
    [Route("[controller]")]
    public class ScanTicketController : Controller
    {
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TicketQrService _ticketQrService;
        private readonly ILogger<ScanTicketController> _logger;

        public ScanTicketController(
            UserManager<KauRestaurantUser> userManager,
            ApplicationDbContext context,
            TicketQrService ticketQrService,
            ILogger<ScanTicketController> logger)
        {
            _userManager = userManager;
            _context = context;
            _ticketQrService = ticketQrService;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/ScanTicket.cshtml");
        }

        public class TicketQrRequest
        {
            public string qrData { get; set; }
        }

        [HttpPost]
        [Route("ValidateTicket")]
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

        public class TicketStatistics
        {
            public List<int> PurchasedTickets { get; set; }
            public List<int> RedeemedTickets { get; set; }
            public List<string> Labels { get; set; }
        }

        [HttpGet]
        [Route("Statistics")] // Add this explicit route
        public async Task<IActionResult> GetTicketStatistics()
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
    }
}
