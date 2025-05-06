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
    // Restricts access to users with specific roles
    [Authorize(Roles = "A1,A2,A3")]
    // Specifies custom route patterns for this controller
    [Route("[controller]")]
    public class ScanTicketController : Controller
    {
        // Manages user information and identity tasks
        private readonly UserManager<KauRestaurantUser> _userManager;
        // Provides access to the application database
        private readonly ApplicationDbContext _context;
        // Contains methods for generating and verifying ticket QR data
        private readonly TicketQrService _ticketQrService;
        // Used to log actions and issues within this controller
        private readonly ILogger<ScanTicketController> _logger;

        // Constructor injecting the necessary services and context
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

        // Displays the ScanTicket view for administrators
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/ScanTicket.cshtml");
        }

        // Represents the data structure passed when validating a ticket
        public class TicketQrRequest
        {
            public string qrData { get; set; }
        }

        // Validates a ticket based on a QR code provided in the POST request body
        [HttpPost]
        [Route("ValidateTicket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateTicket([FromBody] TicketQrRequest request)
        {
            // Log the QR data received for debugging and traceability
            _logger.LogInformation($"Received ticket request: {request?.qrData ?? "null"}");

            // Check if the request or QR data is missing
            if (request == null || string.IsNullOrEmpty(request.qrData))
            {
                _logger.LogWarning("No QR data provided");
                return Json(new { success = false, message = "لم يتم تقديم بيانات QR" });
            }

            try
            {
                // Verify the ticket QR signature to detect tampering
                bool isValid = _ticketQrService.VerifyTicketQrData(request.qrData);

                // If the signature is invalid, return an error response
                if (!isValid)
                {
                    _logger.LogWarning("Invalid QR signature");
                    return Json(new { success = false, message = "QR غير صالح أو تم العبث به" });
                }

                // Deserialize the QR data
                var ticketData = JsonSerializer.Deserialize<TicketQrService.TicketQrData>(request.qrData);

                // Search the database for a matching ticket
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.QRCode == ticketData.TicketId);

                // If no matching ticket is found, return an error
                if (ticket == null)
                {
                    _logger.LogWarning($"Ticket not found: {ticketData.TicketId}");
                    return Json(new { success = false, message = "التذكرة غير موجودة" });
                }

                // If a ticket has already been redeemed, prevent reuse
                if (ticket.IsRedeemed)
                {
                    _logger.LogWarning($"Ticket already redeemed: {ticketData.TicketId}");
                    return Json(new { success = false, message = "تم استخدام هذه التذكرة مسبقاً" });
                }

                // Mark the ticket as redeemed in the database
                ticket.IsRedeemed = true;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully validated ticket: {ticketData.TicketId}");

                // Return success with ticket details
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
                // Log any unexpected errors for troubleshooting
                _logger.LogError(ex, "Error validating ticket");
                return Json(new { success = false, message = $"خطأ: {ex.Message}" });
            }
        }

        // Holds the statistics data shown in ticket usage charts
        public class TicketStatistics
        {
            public List<int> PurchasedTickets { get; set; }
            public List<int> RedeemedTickets { get; set; }
            public List<string> Labels { get; set; }
        }

        // Retrieves ticket statistics for the last six months
        [HttpGet]
        [Route("Statistics")]
        public async Task<IActionResult> GetTicketStatistics()
        {
            // Get today's date to build a range of months
            var today = DateTime.Today;
            // Create a list of the last six months in ascending order
            var last6Months = Enumerable.Range(0, 6)
                .Select(i => today.AddMonths(-i))
                .OrderBy(d => d)
                .ToList();

            // Prepare an object to hold the stats, including labels
            var statistics = new TicketStatistics
            {
                PurchasedTickets = new List<int>(),
                RedeemedTickets = new List<int>(),
                Labels = last6Months.Select(d => d.ToString("MMM yyyy")).ToList()
            };

            // Iterate through each month and gather data
            foreach (var date in last6Months)
            {
                // Define the start date of the month and the next month
                var startDate = new DateTime(date.Year, date.Month, 1);
                var endDate = startDate.AddMonths(1);

                // Count purchased tickets in this date range
                var purchasedCount = await _context.Tickets
                    .Where(t => t.Order.OrderDate >= startDate && t.Order.OrderDate < endDate)
                    .CountAsync();

                // Count redeemed tickets in this date range
                var redeemedCount = await _context.Tickets
                    .Where(t => t.Order.OrderDate >= startDate && t.Order.OrderDate < endDate && t.IsRedeemed)
                    .CountAsync();

                // Add these counts to the statistics object
                statistics.PurchasedTickets.Add(purchasedCount);
                statistics.RedeemedTickets.Add(redeemedCount);
            }

            // Return the statistics as JSON
            return Json(statistics);
        }
    }
}
