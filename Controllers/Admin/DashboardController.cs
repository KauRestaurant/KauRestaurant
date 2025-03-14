using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            UserManager<KauRestaurantUser> userManager,
            ApplicationDbContext context,
            ILogger<DashboardController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}
