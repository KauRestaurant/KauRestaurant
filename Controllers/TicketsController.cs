using KauRestaurant.Controllers;
using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YourNamespace.Models;

namespace KauRestaurant.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Changed from Tickets() to Index() to match convention
        public async Task<IActionResult> Index()
        {
            // Get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Get all non-redeemed tickets for the current user
            var tickets = await _context.Tickets
                .Include(t => t.Order)
                .Where(t => t.Order.CustomerID == userId && !t.IsRedeemed)
                .ToListAsync();

            // Group tickets by meal type
            var breakfastTickets = tickets.Where(t => t.MealType == "الإفطار").ToList();
            var lunchTickets = tickets.Where(t => t.MealType == "الغداء").ToList();
            var dinnerTickets = tickets.Where(t => t.MealType == "العشاء").ToList();

            // Create view model with summary and tickets
            var viewModel = new TicketsViewModel
            {
                BreakfastCount = breakfastTickets.Count,
                LunchCount = lunchTickets.Count,
                DinnerCount = dinnerTickets.Count,
                BreakfastTickets = breakfastTickets,
                LunchTickets = lunchTickets,
                DinnerTickets = dinnerTickets
            };

            return View("~/Views/User/tickets.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RedeemTicket(int ticketId)
        {
            // Get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Find the ticket and verify ownership
            var ticket = await _context.Tickets
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.TicketID == ticketId);

            if (ticket == null)
            {
                return NotFound();
            }

            if (ticket.Order.CustomerID != userId)
            {
                return Forbid();
            }

            if (ticket.IsRedeemed)
            {
                return BadRequest("This ticket has already been redeemed.");
            }

            // Mark ticket as redeemed
            ticket.IsRedeemed = true;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }

    // Add this class for the ViewModel if it doesn't exist already
    public class TicketsViewModel
    {
        public int BreakfastCount { get; set; }
        public int LunchCount { get; set; }
        public int DinnerCount { get; set; }
        public List<Ticket> BreakfastTickets { get; set; } = new List<Ticket>();
        public List<Ticket> LunchTickets { get; set; } = new List<Ticket>();
        public List<Ticket> DinnerTickets { get; set; } = new List<Ticket>();
    }
}
