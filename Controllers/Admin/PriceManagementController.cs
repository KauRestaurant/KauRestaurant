using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class PriceManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/PriceManagement.cshtml");
        }
    }
}
