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
    [Authorize] // Requires authenticated access
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Manages user-related operations such as finding a specific user by ID
        private readonly UserManager<KauRestaurantUser> _userManager;
        // Used for logging messages and errors
        private readonly ILogger<PurchaseController> _logger;
        // Service that retrieves the current ticket prices (e.g., breakfast/lunch cost)
        private readonly TicketPriceService _ticketPriceService;

        // Constructor injecting the services and context
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

        // Renders the purchase page, populating ticket prices
        public async Task<IActionResult> Index()
        {
            // Fetch all ticket prices (e.g., breakfast, lunch)
            var ticketPrices = await _ticketPriceService.GetAllTicketPrices();

            // Store them in the ViewBag for use in the Razor view
            ViewBag.TicketPrices = ticketPrices;

            // Return the purchase page as the response
            return View("~/Views/User/Purchase.cshtml");
        }

        // Creates an order with requested breakfast/lunch ticket quantities
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
        {
            try
            {
                // Log the received ticket quantities for troubleshooting
                _logger.LogInformation($"Received order data - Breakfast: {model.breakfastQty}, Lunch: {model.lunchQty}");

                // Retrieve the currently authenticated user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // If user is not found, return an unauthorized status
                if (userId == null)
                {
                    _logger.LogWarning("User ID is null during CreateOrder");
                    return Unauthorized();
                }

                // Retrieve the latest breakfast and lunch ticket prices from the service
                var ticketPrices = await _ticketPriceService.GetAllTicketPrices();
                var breakfastPrice = ticketPrices["الإفطار"];
                var lunchPrice = ticketPrices["الغداء"];

                // Calculate the total cost based on requested quantities
                var totalAmount = model.breakfastQty * breakfastPrice + model.lunchQty * lunchPrice;

                // Create a new order record and populate its properties
                var order = new Order
                {
                    CustomerID = userId,                  // Associate order with the current user
                    OrderDate = DateTime.Now,             // Set the order creation date to now
                    Status = "Completed",                 // Mark as completed (assuming immediate payment)
                    TotalPaid = (float)totalAmount,       // Total paid by the user
                    BreakfastTicketsCount = model.breakfastQty,
                    LunchTicketsCount = model.lunchQty
                };

                // Save the order to the database so we have an OrderID
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Log creation details, including newly assigned OrderID
                _logger.LogInformation($"Created order {order.OrderID} with B:{order.BreakfastTicketsCount}, L:{order.LunchTicketsCount}");

                // List to hold all newly created tickets
                var tickets = new List<Ticket>();

                // Generate breakfast tickets
                for (int i = 0; i < model.breakfastQty; i++)
                {
                    tickets.Add(new Ticket
                    {
                        OrderID = order.OrderID,
                        MealType = "الإفطار",      // Label ticket as breakfast
                        QRCode = Guid.NewGuid().ToString(),  // Generate unique code
                        IsRedeemed = false,        // Mark as not yet redeemed
                        Price = breakfastPrice     // Record the current breakfast price
                    });
                }

                // Generate lunch tickets
                for (int i = 0; i < model.lunchQty; i++)
                {
                    tickets.Add(new Ticket
                    {
                        OrderID = order.OrderID,
                        MealType = "الغداء",       // Label ticket as lunch
                        QRCode = Guid.NewGuid().ToString(),  // Generate another unique code
                        IsRedeemed = false,        // Mark as not yet redeemed
                        Price = lunchPrice         // Record the current lunch price
                    });
                }

                // Add all generated tickets to the database
                _context.Tickets.AddRange(tickets);
                await _context.SaveChangesAsync();

                // Log how many tickets were created for auditing
                _logger.LogInformation($"Created {tickets.Count} tickets for order {order.OrderID}");

                // Return success response to the client, including order details
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
                // Log the exception details for debugging
                _logger.LogError($"Error in CreateOrder: {ex.Message}");

                // Return a generic 500 status, indicating a server-side error
                return StatusCode(500, new { success = false, message = "An error occurred while processing your order" });
            }
        }
    }

    // Represents the user input for creating an order
    public class OrderViewModel
    {
        public int breakfastQty { get; set; }  // Number of breakfast tickets
        public int lunchQty { get; set; }      // Number of lunch tickets
    }
}
