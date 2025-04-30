using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KauRestaurant.Controllers.User
{
    [Authorize]
    public class TicketsController : Controller
    {
        // Holds a reference to the database context
        private readonly ApplicationDbContext _context;

        // Constructor injecting the database context
        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the list of non-redeemed tickets for the logged-in user
        public async Task<IActionResult> Index()
        {
            // Extract the user ID from authentication claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If user ID isn't present, the user isn't properly authenticated
            if (userId == null)
            {
                // Return an Unauthorized result to indicate invalid access
                return Unauthorized();
            }

            // Retrieve all tickets related to the user that have not been redeemed yet
            var tickets = await _context.Tickets
                .Include(t => t.Order)                          // Include the related Order
                .Where(t => t.Order.CustomerID == userId       // Filter by the current user's ID
                            && !t.IsRedeemed)                  // Only tickets that have not been redeemed
                .ToListAsync();

            // Separate tickets by meal type for easier display
            var breakfastTickets = tickets
                .Where(t => t.MealType == "الإفطار")           // Only tickets labeled "الإفطار" (breakfast)
                .ToList();

            var lunchTickets = tickets
                .Where(t => t.MealType == "الغداء")            // Only tickets labeled "الغداء" (lunch)
                .ToList();

            // Populate a view model to hold both ticket lists and counts
            var viewModel = new TicketsViewModel
            {
                BreakfastCount = breakfastTickets.Count,       // Number of breakfast tickets
                LunchCount = lunchTickets.Count,               // Number of lunch tickets
                BreakfastTickets = breakfastTickets,           // List of breakfast tickets
                LunchTickets = lunchTickets                    // List of lunch tickets
            };

            // Render the Tickets view (under /Views/User/Tickets.cshtml) with the model
            return View("~/Views/User/Tickets.cshtml", viewModel);
        }

        // Handles POST requests to redeem a specific ticket
        [HttpPost]
        public async Task<IActionResult> RedeemTicket(int ticketId)
        {
            // Get the current user's ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If user ID is missing, deny the request
            if (userId == null)
            {
                return Unauthorized();
            }

            // Locate the ticket in the database, including its associated Order information
            var ticket = await _context.Tickets
                .Include(t => t.Order)                          // Include the Order to check ownership
                .FirstOrDefaultAsync(t => t.TicketID == ticketId);

            // If no ticket is found, return a 404 Not Found
            if (ticket == null)
            {
                return NotFound();
            }

            // Compare ticket owner to the logged-in user; if they differ, forbid
            if (ticket.Order.CustomerID != userId)
            {
                return Forbid();
            }

            // Check if the ticket has already been redeemed
            if (ticket.IsRedeemed)
            {
                // Return a 400 Bad Request with an explanation (ticket is already redeemed)
                return BadRequest("This ticket has already been redeemed.");
            }

            // Mark the ticket as redeemed
            ticket.IsRedeemed = true;
            // Save the change to the database
            await _context.SaveChangesAsync();

            // Return success result (often handled by client-side scripting)
            return Ok(new { success = true });
        }
    }

    // Represents data needed to display ticket counts and lists for the user
    public class TicketsViewModel
    {
        public int BreakfastCount { get; set; }
        public int LunchCount { get; set; }
        public List<Ticket> BreakfastTickets { get; set; } = new List<Ticket>();
        public List<Ticket> LunchTickets { get; set; } = new List<Ticket>();
    }
}
