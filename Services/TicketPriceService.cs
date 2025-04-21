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

            return result;
        }

        // Add method to update ticket prices
        public async Task UpdateTicketPrice(string mealType, decimal price)
        {
            var ticketPrice = await _context.TicketPrices
                .FirstOrDefaultAsync(tp => tp.MealType == mealType);

            if (ticketPrice == null)
            {
                // Create new entry if it doesn't exist
                ticketPrice = new TicketPrice
                {
                    MealType = mealType,
                    Price = price,
                    LastUpdated = DateTime.Now
                };
                _context.TicketPrices.Add(ticketPrice);
            }
            else
            {
                // Update existing entry
                ticketPrice.Price = price;
                ticketPrice.LastUpdated = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
