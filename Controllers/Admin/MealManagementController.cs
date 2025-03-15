using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class MealManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/MealManagement.cshtml");
        }
    }
}
