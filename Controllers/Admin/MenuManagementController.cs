using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class MenuManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/MenuManagement.cshtml");
        }
    }
}
