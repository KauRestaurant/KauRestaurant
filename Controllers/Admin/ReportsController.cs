using Microsoft.AspNetCore.Mvc;

namespace KauRestaurant.Controllers.Admin
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/Reports.cshtml");
        }
    }
}
