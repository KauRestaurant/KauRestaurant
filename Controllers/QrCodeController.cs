using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Threading.Tasks;
using YourNamespace.Models;

public class QrCodeController : Controller
{
    private readonly TicketQrService _ticketQrService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<KauRestaurantUser> _userManager;

    public QrCodeController(
        TicketQrService ticketQrService,
        ApplicationDbContext context,
        UserManager<KauRestaurantUser> userManager)
    {
        _ticketQrService = ticketQrService;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> GenerateQrCode(int ticketId, string mealType, string userId)
    {
        if (ticketId <= 0)
        {
            return BadRequest("Invalid ticket ID.");
        }

        // Get the actual ticket from the database
        var ticket = await _context.Tickets
            .Include(t => t.Order)
            .FirstOrDefaultAsync(t => t.TicketID == ticketId);

        if (ticket == null)
        {
            return NotFound("Ticket not found.");
        }

        // Check if the ticket is redeemed
        if (ticket.IsRedeemed)
        {
            return BadRequest("Ticket has already been redeemed.");
        }

        // Get additional user information
        var user = await _userManager.FindByIdAsync(ticket.Order.CustomerID);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        // Build the QR data
        var ticketData = new TicketQrService.TicketQrData
        {
            TicketId = ticketId.ToString(),
            MealType = mealType,
            UserId = ticket.Order.CustomerID,
            UserName = user.UserName,
            UserFullName = $"{user.FirstName} {user.LastName}",
            OrderId = ticket.OrderID.ToString(),
            OrderDate = ticket.Order.OrderDate,
        };

        string qrData = _ticketQrService.GenerateTicketQrData(ticketData);

        // Generate QR code
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);

        return File(qrCodeImage, "image/png");
    }
}
