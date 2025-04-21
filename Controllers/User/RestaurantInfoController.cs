using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Controllers.User
{
    public class RestaurantInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            if (restaurant == null)
            {
                return View("~/Views/User/RestaurantInfo.cshtml", new Restaurant());
            }

            return View("~/Views/User/RestaurantInfo.cshtml", restaurant);
        }
    }
}
