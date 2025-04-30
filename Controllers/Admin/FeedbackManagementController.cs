using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.Admin
{
    // Restrict access to users with roles A1 or A2
    [Authorize(Roles = "A1,A2")]
    public class FeedbackManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Dependency inject the application's database context
        public FeedbackManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves all feedback entries from the database in descending order by creation date
        public async Task<IActionResult> Index()
        {
            // Include the associated user data for each feedback
            var allFeedback = await _context.Feedbacks
                .Include(f => f.User)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();

            // Render the feedback list in our admin view
            return View("~/Views/Admin/FeedbackManagement.cshtml", allFeedback);
        }

        // Handles the deletion of a single feedback record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Attempt to find the feedback by its ID
                var feedback = await _context.Feedbacks.FindAsync(id);

                // If not found, inform the user and exit
                if (feedback == null)
                {
                    TempData["InfoMessage"] = "لم يتم العثور على الرسالة المحددة";
                    return RedirectToAction(nameof(Index));
                }

                // Remove the feedback from the database and save changes
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();

                // Provide success indication
                TempData["SuccessMessage"] = "تم حذف الرسالة بنجاح";
            }
            catch (Exception ex)
            {
                // Catch any errors that occur during deletion
                TempData["InfoMessage"] = $"حدث خطأ أثناء محاولة الحذف: {ex.Message}";
            }

            // Return the user to the list view
            return RedirectToAction(nameof(Index));
        }
    }
}
