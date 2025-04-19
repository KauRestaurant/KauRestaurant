using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RestaurantInfoManagementController> _logger;

        public RestaurantInfoManagementController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ILogger<RestaurantInfoManagementController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
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
        public async Task<IActionResult> Update(IFormCollection form, IFormFile photo)
        {
            try
            {
                // Get the restaurant or create one if it doesn't exist
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
                if (restaurant == null)
                {
                    restaurant = new Restaurant();
                    _context.Add(restaurant);
                }

                // Copy basic information from form
                restaurant.Name = form["Name"];
                restaurant.Description = form["Description"];
                restaurant.Address = form["Address"];
                restaurant.PhoneNumber = form["PhoneNumber"];
                restaurant.Email = form["Email"];
                restaurant.LocationUrl = form["LocationUrl"];

                // Process each day's settings
                string[] days = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                string[] meals = { "Breakfast", "Lunch", "Dinner" };

                foreach (var day in days)
                {
                    // Set the day's open status
                    bool isDayOpen = form.Keys.Contains($"Is{day}Open");
                    typeof(Restaurant).GetProperty($"Is{day}Open").SetValue(restaurant, isDayOpen);

                    foreach (var meal in meals)
                    {
                        // Set meal served status
                        bool servesMeal = form.Keys.Contains($"{day}Serves{meal}");
                        var servesMealProp = typeof(Restaurant).GetProperty($"{day}Serves{meal}");
                        if (servesMealProp != null)
                        {
                            servesMealProp.SetValue(restaurant, servesMeal);
                        }

                        // Handle open time
                        var openTimePropName = $"{day}{meal}OpenTime";
                        var openTimeProp = typeof(Restaurant).GetProperty(openTimePropName);

                        if (form.Keys.Contains(openTimePropName) && !string.IsNullOrEmpty(form[openTimePropName]))
                        {
                            if (TimeSpan.TryParse(form[openTimePropName], out TimeSpan openTime))
                            {
                                openTimeProp?.SetValue(restaurant, openTime);
                            }
                        }

                        // Handle close time
                        var closeTimePropName = $"{day}{meal}CloseTime";
                        var closeTimeProp = typeof(Restaurant).GetProperty(closeTimePropName);

                        if (form.Keys.Contains(closeTimePropName) && !string.IsNullOrEmpty(form[closeTimePropName]))
                        {
                            if (TimeSpan.TryParse(form[closeTimePropName], out TimeSpan closeTime))
                            {
                                closeTimeProp?.SetValue(restaurant, closeTime);
                            }
                        }
                    }
                }

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating restaurant information");
                TempData["InfoMessage"] = "حدث خطأ أثناء حفظ المعلومات. يرجى المحاولة مرة أخرى.";
            }

            return RedirectToAction("Index");
        }
    }
}
