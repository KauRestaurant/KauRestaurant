using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KauRestaurant.Controllers.User
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<KauRestaurantUser> _userManager;

        public FeedbackController(
            ApplicationDbContext context,
            UserManager<KauRestaurantUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get all feedback for this user
            var userFeedbacks = await _context.Feedbacks
                .Where(f => f.UserID == userId)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();

            // Prepare model for the view
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new FeedbackViewModel
            {
                UserFeedbacks = userFeedbacks,
                NewFeedback = new Feedback
                {
                    UserID = userId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserEmail = user.Email ?? string.Empty
                }
            };

            return View("~/Views/User/Feedback.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string subject, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(text))
                {
                    TempData["ErrorMessage"] = "يجب ملء جميع الحقول المطلوبة";
                    return RedirectToAction(nameof(Index));
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);

                var feedback = new Feedback
                {
                    UserID = userId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserEmail = user.Email ?? string.Empty,
                    Subject = subject,
                    Text = text,
                    CreatedDate = DateTime.Now
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["StatusMessage"] = "تم إرسال رسالتك بنجاح!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إرسال الرسالة، يرجى المحاولة مرة أخرى.";
                return RedirectToAction(nameof(Index));
            }
        }
    }

    public class FeedbackViewModel
    {
        public Feedback NewFeedback { get; set; }
        public IList<Feedback> UserFeedbacks { get; set; } = new List<Feedback>();
    }
}
