using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.User
{
    public class TermsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TermsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get all terms from the database, ordered by LastUpdated
            var terms = await _context.Terms
                .OrderByDescending(t => t.LastUpdated)
                .ToListAsync();

            return View("~/Views/User/Terms.cshtml", terms);
        }
    }
}
