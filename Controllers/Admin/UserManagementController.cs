using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/UserManagement.cshtml");
        }
    }
}
