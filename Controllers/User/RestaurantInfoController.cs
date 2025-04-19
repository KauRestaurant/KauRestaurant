using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            // Get the first restaurant (assuming there's only one in the system)
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            if (restaurant == null)
            {
                // Handle case where no restaurant exists
                return NotFound();
            }

            return View("~/Views/User/RestaurantInfo.cshtml", restaurant);
        }
    }
}
