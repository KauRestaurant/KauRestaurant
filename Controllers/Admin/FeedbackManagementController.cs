using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1")]
    public class FeedbackManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allFeedback = await _context.Feedbacks
                .Include(f => f.User)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();

            return View("~/Views/Admin/FeedbackManagement.cshtml", allFeedback);
        }

        // Add Delete function for feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);

                if (feedback == null)
                {
                    TempData["InfoMessage"] = "لم يتم العثور على الرسالة المحددة";
                    return RedirectToAction(nameof(Index));
                }

                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف الرسالة بنجاح";
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"حدث خطأ أثناء محاولة الحذف: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
