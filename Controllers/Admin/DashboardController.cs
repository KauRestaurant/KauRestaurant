using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    // Restrict access to users with roles A1, A2, or A3
    [Authorize(Roles = "A1,A2,A3")]
    public class DashboardController : Controller
    {
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        // Constructor dependency injection for user manager, DB context, and logger
        public DashboardController(
            UserManager<KauRestaurantUser> userManager,
            ApplicationDbContext context,
            ILogger<DashboardController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // Opens the Dashboard view for administrators
        public IActionResult Index()
        {
            // Renders admin dashboard page located under the Admin folder
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}
