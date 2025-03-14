using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class FeedbackManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
