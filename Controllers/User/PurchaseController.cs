using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Models;
using KauRestaurant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KauRestaurant.Controllers.User
{
    [Authorize] // Ensures user is logged in
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly ILogger<PurchaseController> _logger;
        private readonly TicketPriceService _ticketPriceService;

        public PurchaseController(
            ApplicationDbContext context,
            UserManager<KauRestaurantUser> userManager,
            ILogger<PurchaseController> logger,
            TicketPriceService ticketPriceService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _ticketPriceService = ticketPriceService;
        }

        public async Task<IActionResult> Index()
        {
            // Get the current prices and pass them to the view
            var ticketPrices = await _ticketPriceService.GetAllTicketPrices();
            ViewBag.TicketPrices = ticketPrices;

            return View("~/Views/User/Purchase.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
        {
            try
            {
                // Log received data
                _logger.LogInformation($"Received order data - Breakfast: {model.breakfastQty}, Lunch: {model.lunchQty}");

                // Get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    _logger.LogWarning("User ID is null during CreateOrder");
                    return Unauthorized();
                }

                // Calculate total price
                var ticketPrices = await _ticketPriceService.GetAllTicketPrices();
                var breakfastPrice = ticketPrices["الإفطار"];
                var lunchPrice = ticketPrices["الغداء"];

                var totalAmount = model.breakfastQty * breakfastPrice + model.lunchQty * lunchPrice;

                // Create new order
                var order = new Order
                {
                    CustomerID = userId,
                    OrderDate = DateTime.Now,
                    Status = "Completed", // Immediately mark as completed
                    TotalPaid = (float)totalAmount,
                    BreakfastTicketsCount = model.breakfastQty,
                    LunchTicketsCount = model.lunchQty,
                };

                // Save order to get OrderID
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created order {order.OrderID} with B:{order.BreakfastTicketsCount}, L:{order.LunchTicketsCount}");

                // Generate tickets for this order
                var tickets = new List<Ticket>();

                // Create breakfast tickets
                for (int i = 0; i < model.breakfastQty; i++)
                {
                    tickets.Add(new Ticket
                    {
                        OrderID = order.OrderID,
                        MealType = "الإفطار", // Breakfast
                        QRCode = Guid.NewGuid().ToString(),
                        IsRedeemed = false,
                        Price = breakfastPrice // Set the current price
                    });
                }

                // Create lunch tickets
                for (int i = 0; i < model.lunchQty; i++)
                {
                    tickets.Add(new Ticket
                    {
                        OrderID = order.OrderID,
                        MealType = "الغداء", // Lunch
                        QRCode = Guid.NewGuid().ToString(),
                        IsRedeemed = false,
                        Price = lunchPrice // Set the current price
                    });
                }

                // Save all tickets
                _context.Tickets.AddRange(tickets);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created {tickets.Count} tickets for order {order.OrderID}");

                // Return success with order details
                return Ok(new
                {
                    success = true,
                    orderId = order.OrderID,
                    totalAmount,
                    ticketCount = model.breakfastQty + model.lunchQty
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateOrder: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your order" });
            }
        }
    }

    public class OrderViewModel
    {
        public int breakfastQty { get; set; }
        public int lunchQty { get; set; }
    }
}
