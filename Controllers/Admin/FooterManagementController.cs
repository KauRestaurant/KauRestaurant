using KauRestaurant.Data;
using KauRestaurant.Models;
using KauRestaurant.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1,A2")]
    public class FooterManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FooterManagementController> _logger;

        public FooterManagementController(ApplicationDbContext context, ILogger<FooterManagementController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new FooterManagementViewModel
                {
                    FAQs = await _context.FAQs.OrderBy(f => f.DisplayOrder).ToListAsync(),
                    Terms = await _context.Terms.OrderBy(t => t.DisplayOrder).ToListAsync()
                };

                return View("~/Views/Admin/FooterManagement.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading footer management page");
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل الصفحة. يرجى المحاولة مرة أخرى.";
                return View("~/Views/Admin/FooterManagement.cshtml", new FooterManagementViewModel());
            }
        }

        #region FAQ Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFAQ(FAQ model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Adjust display orders of existing FAQs if needed
                    if (model.DisplayOrder < int.MaxValue)
                    {
                        var existingFaqs = await _context.FAQs
                            .Where(f => f.DisplayOrder >= model.DisplayOrder)
                            .OrderBy(f => f.DisplayOrder)
                            .ToListAsync();

                        foreach (var faq in existingFaqs)
                        {
                            faq.DisplayOrder += 1;
                        }
                    }

                    await _context.FAQs.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تمت إضافة السؤال بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "يرجى التحقق من البيانات المدخلة.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding FAQ");
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة السؤال.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFAQ(FAQ model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingFAQ = await _context.FAQs.FindAsync(model.FAQID);
                    if (existingFAQ == null)
                    {
                        TempData["ErrorMessage"] = "لم يتم العثور على السؤال.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Store the old display order
                    int oldDisplayOrder = existingFAQ.DisplayOrder;
                    int newDisplayOrder = model.DisplayOrder;

                    // Update the FAQ 
                    existingFAQ.Question = model.Question;
                    existingFAQ.Answer = model.Answer;

                    // If the display order changed, adjust all affected FAQs
                    if (oldDisplayOrder != newDisplayOrder)
                    {
                        await AdjustFAQDisplayOrder(existingFAQ, oldDisplayOrder, newDisplayOrder);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "تم تحديث السؤال بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "يرجى التحقق من البيانات المدخلة.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FAQ");
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث السؤال.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to adjust FAQ display orders
        private async Task AdjustFAQDisplayOrder(FAQ currentFaq, int oldOrder, int newOrder)
        {
            // Get all FAQs except the current one
            var faqs = await _context.FAQs
                .Where(f => f.FAQID != currentFaq.FAQID)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            if (newOrder < oldOrder) // Moving item up (lower number)
            {
                // Increment display order for items between new and old positions
                foreach (var faq in faqs)
                {
                    if (faq.DisplayOrder >= newOrder && faq.DisplayOrder < oldOrder)
                    {
                        faq.DisplayOrder += 1;
                    }
                }
                // Set the new order for the current FAQ
                currentFaq.DisplayOrder = newOrder;
            }
            else if (newOrder > oldOrder) // Moving item down (higher number)
            {
                // Decrement display order for items between old and new positions
                foreach (var faq in faqs)
                {
                    if (faq.DisplayOrder > oldOrder && faq.DisplayOrder <= newOrder)
                    {
                        faq.DisplayOrder -= 1;
                    }
                }
                // Set the new order for the current FAQ
                currentFaq.DisplayOrder = newOrder;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                var faq = await _context.FAQs.FindAsync(id);
                if (faq == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على السؤال.";
                    return RedirectToAction(nameof(Index));
                }

                int deletedOrder = faq.DisplayOrder;
                _context.FAQs.Remove(faq);

                // Adjust the display order of items after the deleted item
                var faqs = await _context.FAQs
                    .Where(f => f.DisplayOrder > deletedOrder)
                    .OrderBy(f => f.DisplayOrder)
                    .ToListAsync();

                foreach (var item in faqs)
                {
                    item.DisplayOrder -= 1;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف السؤال بنجاح.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FAQ");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف السؤال.";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Terms Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTerms(Terms model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Set the LastUpdated to current date
                    model.LastUpdated = DateTime.Now;

                    // Adjust display orders of existing Terms if needed
                    if (model.DisplayOrder < int.MaxValue)
                    {
                        var existingTerms = await _context.Terms
                            .Where(t => t.DisplayOrder >= model.DisplayOrder)
                            .OrderBy(t => t.DisplayOrder)
                            .ToListAsync();

                        foreach (var term in existingTerms)
                        {
                            term.DisplayOrder += 1;
                        }
                    }

                    await _context.Terms.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تمت إضافة الشروط والأحكام بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "يرجى التحقق من البيانات المدخلة.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Terms");
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة الشروط والأحكام.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTerms(Terms model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var term = await _context.Terms.FindAsync(model.TermID);
                    if (term == null)
                    {
                        TempData["ErrorMessage"] = "الشروط والأحكام غير موجودة";
                        return RedirectToAction(nameof(Index));
                    }

                    // Store the old display order
                    int oldDisplayOrder = term.DisplayOrder;
                    int newDisplayOrder = model.DisplayOrder;

                    // Update term properties
                    term.Title = model.Title;
                    term.Content = model.Content;
                    term.LastUpdated = DateTime.UtcNow;

                    // If the display order changed, adjust all affected Terms
                    if (oldDisplayOrder != newDisplayOrder)
                    {
                        await AdjustTermsDisplayOrder(term, oldDisplayOrder, newDisplayOrder);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "تم تحديث الشروط والأحكام بنجاح";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating Terms");
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث الشروط والأحكام";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "البيانات المدخلة غير صحيحة";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to adjust Terms display orders
        private async Task AdjustTermsDisplayOrder(Terms currentTerm, int oldOrder, int newOrder)
        {
            // Get all Terms except the current one
            var terms = await _context.Terms
                .Where(t => t.TermID != currentTerm.TermID)
                .OrderBy(t => t.DisplayOrder)
                .ToListAsync();

            if (newOrder < oldOrder) // Moving item up (lower number)
            {
                // Increment display order for items between new and old positions
                foreach (var term in terms)
                {
                    if (term.DisplayOrder >= newOrder && term.DisplayOrder < oldOrder)
                    {
                        term.DisplayOrder += 1;
                    }
                }
                // Set the new order for the current Term
                currentTerm.DisplayOrder = newOrder;
            }
            else if (newOrder > oldOrder) // Moving item down (higher number)
            {
                // Decrement display order for items between old and new positions
                foreach (var term in terms)
                {
                    if (term.DisplayOrder > oldOrder && term.DisplayOrder <= newOrder)
                    {
                        term.DisplayOrder -= 1;
                    }
                }
                // Set the new order for the current Term
                currentTerm.DisplayOrder = newOrder;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTerms(int id)
        {
            try
            {
                var terms = await _context.Terms.FindAsync(id);
                if (terms == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على الشروط والأحكام.";
                    return RedirectToAction(nameof(Index));
                }

                int deletedOrder = terms.DisplayOrder;
                _context.Terms.Remove(terms);

                // Adjust the display order of items after the deleted item
                var remainingTerms = await _context.Terms
                    .Where(t => t.DisplayOrder > deletedOrder)
                    .OrderBy(t => t.DisplayOrder)
                    .ToListAsync();

                foreach (var item in remainingTerms)
                {
                    item.DisplayOrder -= 1;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف الشروط والأحكام بنجاح.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Terms");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الشروط والأحكام.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetTermsContent(int id)
        {
            try
            {
                var terms = await _context.Terms.FindAsync(id);
                if (terms == null)
                {
                    return NotFound();
                }

                return Json(new { content = terms.Content });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving terms content");
                return StatusCode(500, "Error retrieving terms content");
            }
        }

        #endregion
    }
}

namespace KauRestaurant.ViewModels
{
    public class FooterManagementViewModel
    {
        public IEnumerable<FAQ> FAQs { get; set; } = new List<FAQ>();
        public IEnumerable<Terms> Terms { get; set; } = new List<Terms>();

        // Properties for FAQ form
        public int FAQID { get; set; }
        [Required(ErrorMessage = "السؤال مطلوب")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز طول السؤال 500 حرف")]
        public string Question { get; set; }

        [Required(ErrorMessage = "الإجابة مطلوبة")]
        [StringLength(2000, ErrorMessage = "يجب ألا يتجاوز طول الإجابة 2000 حرف")]
        public string Answer { get; set; }

        // Properties for Terms form
        public int TermID { get; set; }
        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يزيد العنوان عن 100 حرف")]
        public string Title { get; set; }

        [Required(ErrorMessage = "المحتوى مطلوب")]
        [StringLength(2000, ErrorMessage = "يجب ألا يزيد المحتوى عن 2000 حرف")]
        public string Content { get; set; }

        [Required(ErrorMessage = "تاريخ التحديث مطلوب")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public int DisplayOrder { get; set; } = 0;
    }

}
