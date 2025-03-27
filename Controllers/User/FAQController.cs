using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.User
{
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FAQController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var faqs = await _context.FAQs
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return View("~/Views/User/FAQ.cshtml", faqs);
        }
    }
}
