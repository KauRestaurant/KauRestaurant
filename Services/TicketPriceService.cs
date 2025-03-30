using KauRestaurant.Data;
using KauRestaurant.Models;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Services
{
    public class TicketPriceService
    {
        private readonly ApplicationDbContext _context;

        public TicketPriceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTicketPrice(string mealType)
        {
            var ticketPrice = await _context.TicketPrices
                .FirstOrDefaultAsync(tp => tp.MealType == mealType);

            if (ticketPrice == null)
            {
                // Default prices if not found
                return mealType == "الإفطار" ? 7m : 10m;
            }

            return ticketPrice.Price;
        }

        public async Task<Dictionary<string, decimal>> GetAllTicketPrices()
        {
            var prices = await _context.TicketPrices.ToListAsync();
            var result = new Dictionary<string, decimal>();

            // Add breakfast price
            var breakfastPrice = prices.FirstOrDefault(p => p.MealType == "الإفطار");
            result["الإفطار"] = breakfastPrice?.Price ?? 7m;

            // Add lunch price
            var lunchPrice = prices.FirstOrDefault(p => p.MealType == "الغداء");
            result["الغداء"] = lunchPrice?.Price ?? 10m;

            // Add dinner price
            var dinnerPrice = prices.FirstOrDefault(p => p.MealType == "العشاء");
            result["العشاء"] = dinnerPrice?.Price ?? 10m;

            return result;
        }
    }
}
