using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Threading.Tasks;

public class QrCodeController : Controller
{
    // Provides logic to generate QR data strings for tickets
    private readonly TicketQrService _ticketQrService;
    private readonly ApplicationDbContext _context;
    // Manages user-related operations such as finding a specific user by ID
    private readonly UserManager<KauRestaurantUser> _userManager;

    // Constructor injecting the necessary services and context
    public QrCodeController(
        TicketQrService ticketQrService,
        ApplicationDbContext context,
        UserManager<KauRestaurantUser> userManager)
    {
        _ticketQrService = ticketQrService;
        _context = context;
        _userManager = userManager;
    }

    // Generates a QR code image for a given ticket
    public async Task<IActionResult> GenerateQrCode(int ticketId, string mealType, string userId)
    {
        // Validate the ticket ID
        if (ticketId <= 0)
        {
            return BadRequest("Invalid ticket ID.");
        }

        // Retrieve the specified ticket from the database, including its related order
        var ticket = await _context.Tickets
            .Include(t => t.Order)
            .FirstOrDefaultAsync(t => t.TicketID == ticketId);

        // If the ticket doesn't exist, return a 404 Not Found
        if (ticket == null)
        {
            return NotFound("Ticket not found.");
        }

        // Verify that the ticket has not yet been redeemed
        if (ticket.IsRedeemed)
        {
            return BadRequest("Ticket has already been redeemed.");
        }

        // Retrieve the user who owns this order, using the user manager
        var user = await _userManager.FindByIdAsync(ticket.Order.CustomerID);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        // Construct the data needed for the QR code, including unique IDs and user info
        var ticketData = new TicketQrService.TicketQrData
        {
            TicketId = ticket.QRCode,              // The ticket's unique QR code text
            MealType = mealType,                   // Name or type of the meal
            UserId = ticket.Order.CustomerID,      // ID of the user who owns the ticket
            UserName = user.UserName,             // Username for display or identification
            UserFullName = $"{user.FirstName} {user.LastName}", // Full name for clarity
            OrderId = ticket.OrderID.ToString(),   // ID of the related order
            OrderDate = ticket.Order.OrderDate     // Date the order was placed
        };

        // Generate a QR data string using the ticket data
        string qrData = _ticketQrService.GenerateTicketQrData(ticketData);

        // Initialize a QR code generator
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        // Create QR code data
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
        // Convert the QR code data into a PNG image
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);

        // Return the image as a PNG file in the response
        return File(qrCodeImage, "image/png");
    }
}
