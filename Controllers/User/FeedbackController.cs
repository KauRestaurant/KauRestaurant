using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.User
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
