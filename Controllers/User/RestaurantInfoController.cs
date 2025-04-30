using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.User
{
    // Displays basic information for a restaurant
    public class RestaurantInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the restaurant info page
        public async Task<IActionResult> Index()
        {
            // Fetch the first record in the Restaurants table
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            // If no restaurant record is found, create a new instance and pass it to the view
            if (restaurant == null)
            {
                return View("~/Views/User/RestaurantInfo.cshtml", new Restaurant());
            }

            // Otherwise, pass the retrieved restaurant entity to the view
            return View("~/Views/User/RestaurantInfo.cshtml", restaurant);
        }
    }
}
