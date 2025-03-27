using KauRestaurant.Data;
using KauRestaurant.Models;
using KauRestaurant.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
                    await _context.FAQs.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تمت إضافة السؤال الشائع بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "يرجى التحقق من البيانات المدخلة.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding FAQ");
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة السؤال الشائع.";
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
                        TempData["ErrorMessage"] = "لم يتم العثور على السؤال الشائع.";
                        return RedirectToAction(nameof(Index));
                    }

                    existingFAQ.Question = model.Question;
                    existingFAQ.Answer = model.Answer;
                    existingFAQ.DisplayOrder = model.DisplayOrder;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "تم تحديث السؤال الشائع بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "يرجى التحقق من البيانات المدخلة.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FAQ");
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث السؤال الشائع.";
            }

            return RedirectToAction(nameof(Index));
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
                    TempData["ErrorMessage"] = "لم يتم العثور على السؤال الشائع.";
                    return RedirectToAction(nameof(Index));
                }

                _context.FAQs.Remove(faq);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف السؤال الشائع بنجاح.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FAQ");
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف السؤال الشائع.";
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

                    term.Title = model.Title;
                    term.Content = model.Content;
                    term.DisplayOrder = model.DisplayOrder; // This line is missing in the original code
                    term.LastUpdated = DateTime.UtcNow;

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

                _context.Terms.Remove(terms);
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
    }
}

