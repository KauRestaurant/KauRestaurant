using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.User
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/User/FAQ.cshtml");
        }
    }
}
