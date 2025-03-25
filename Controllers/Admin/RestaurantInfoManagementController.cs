using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1,A2")]
    public class RestaurantInfoManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RestaurantInfoManagementController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            if (restaurant == null)
            {
                restaurant = new Restaurant();
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
            }

            return View("~/Views/Admin/RestaurantInfoManagement.cshtml", restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Restaurant model, IFormFile photo)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            if (restaurant == null)
            {
                restaurant = new Restaurant();
                _context.Add(restaurant);
            }

            // Copy all properties
            restaurant.Name = model.Name;
            restaurant.Description = model.Description;
            restaurant.Address = model.Address;
            restaurant.PhoneNumber = model.PhoneNumber;
            restaurant.Email = model.Email;
            restaurant.LocationUrl = model.LocationUrl;
            restaurant.DaysOpen = model.DaysOpen;

            // Copy meal times
            restaurant.ServesBreakfast = model.ServesBreakfast;
            restaurant.BreakfastOpenTime = model.ServesBreakfast ? model.BreakfastOpenTime : null;
            restaurant.BreakfastCloseTime = model.ServesBreakfast ? model.BreakfastCloseTime : null;

            restaurant.ServesLunch = model.ServesLunch;
            restaurant.LunchOpenTime = model.ServesLunch ? model.LunchOpenTime : null;
            restaurant.LunchCloseTime = model.ServesLunch ? model.LunchCloseTime : null;

            restaurant.ServesDinner = model.ServesDinner;
            restaurant.DinnerOpenTime = model.ServesDinner ? model.DinnerOpenTime : null;
            restaurant.DinnerCloseTime = model.ServesDinner ? model.DinnerCloseTime : null;

            // Handle photo
            if (photo != null && photo.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "restaurant");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                restaurant.PhotoPath = $"/images/restaurant/{uniqueFileName}";
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم حفظ معلومات المطعم بنجاح";

            return RedirectToAction("Index");
        }
    }
}
