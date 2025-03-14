using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class RestaurantInfoManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
