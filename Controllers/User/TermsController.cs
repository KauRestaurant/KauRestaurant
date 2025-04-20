using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.User
{
    public class TermsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/User/Terms.cshtml");
        }
    }
}
