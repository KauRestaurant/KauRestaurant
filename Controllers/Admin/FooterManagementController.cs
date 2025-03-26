using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class FooterManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/FooterManagement.cshtml");
        }
    }
}
