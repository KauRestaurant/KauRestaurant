using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.User
{
    public class RestaurantInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestaurantInfoController> _logger;

        public RestaurantInfoController(ApplicationDbContext context, ILogger<RestaurantInfoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            if (restaurant == null)
            {
                _logger.LogWarning("No restaurant information found in database");
                restaurant = new Restaurant
                {
                    Name = "مطعم KAU",
                    Description = "مطعم الجامعة الرئيسي الذي يقدم مجموعة متنوعة من الوجبات اللذيذة للطلاب وأعضاء هيئة التدريس.",
                    Address = "جامعة الملك عبدالعزيز، جدة، المملكة العربية السعودية",
                    DaysOpen = "الأحد,الإثنين,الثلاثاء,الأربعاء,الخميس"
                };
            }

            return View("~/Views/User/RestaurantInfo.cshtml", restaurant);
        }
    }
}
