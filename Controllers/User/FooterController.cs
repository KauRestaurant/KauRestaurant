using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.User
{
    // Handles FAQ, Terms, and Feedback-related pages
    public class FooterController : Controller
    {
        // Provides data access to Feedback, etc.
        private readonly ApplicationDbContext _context;
        // Manages user lookups (e.g., current user info)
        private readonly UserManager<KauRestaurantUser> _userManager;

        // Constructor injecting the application context and user manager
        public FooterController(
            ApplicationDbContext context,
            UserManager<KauRestaurantUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Displays Frequently Asked Questions page
        public IActionResult FAQ()
        {
            return View("~/Views/User/FAQ.cshtml");
        }

        // Displays Terms and Conditions page
        public IActionResult Terms()
        {
            return View("~/Views/User/Terms.cshtml");
        }

        // Shows the Feedback page with user-specific feedback if authenticated
        public async Task<IActionResult> Index()
        {
            // Prepare a ViewModel to hold existing feedback and a new feedback form
            var viewModel = new FeedbackViewModel
            {
                UserFeedbacks = new List<Feedback>(),
                NewFeedback = new Feedback()
            };

            // Only load feedback if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Retrieve the user’s unique ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Fetch and order user’s feedback by date
                var userFeedbacks = await _context.Feedbacks
                    .Where(f => f.UserID == userId)
                    .OrderByDescending(f => f.CreatedDate)
                    .ToListAsync();

                // Obtain user record for additional info
                var user = await _userManager.GetUserAsync(User);

                // Populate our ViewModel with existing feedback
                viewModel.UserFeedbacks = userFeedbacks;
                viewModel.NewFeedback = new Feedback
                {
                    UserID = userId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserEmail = user.Email ?? string.Empty
                };
            }

            // Return the Feedback view using a strongly typed ViewModel
            return View("~/Views/User/Feedback.cshtml", viewModel);
        }

        // Submits new feedback, handling both authenticated and guest users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(
            string subject,
            string text,
            string userName,
            string userEmail)
        {
            try
            {
                // Basic server-side validation
                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(text))
                {
                    TempData["ErrorMessage"] = "يجب ملء جميع الحقول المطلوبة";
                    return RedirectToAction(nameof(Index));
                }

                // Construct feedback object
                var feedback = new Feedback
                {
                    Subject = subject,
                    Text = text,
                    CreatedDate = DateTime.Now
                };

                // If user is signed in, link feedback to their account
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.GetUserAsync(User);

                    feedback.UserID = userId;
                    feedback.UserName = $"{user.FirstName} {user.LastName}";
                    feedback.UserEmail = user.Email ?? string.Empty;
                }
                else
                {
                    // Guest feedback requires a provided name and email
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
                    {
                        TempData["ErrorMessage"] = "يجب إدخال الاسم والبريد الإلكتروني";
                        return RedirectToAction(nameof(Index));
                    }

                    feedback.UserName = userName;
                    feedback.UserEmail = userEmail;
                }

                // Persist the new feedback record
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                // Inform user of success
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

    // Encapsulates the new feedback form and any existing feedback
    public class FeedbackViewModel
    {
        public Feedback NewFeedback { get; set; }
        public IList<Feedback> UserFeedbacks { get; set; } = new List<Feedback>();
    }
}
