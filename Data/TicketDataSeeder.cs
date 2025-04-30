using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace KauRestaurant.Data
{
    public static class TicketDataSeeder
    {
        public static async Task SeedTickets(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if we already have tickets in the database
            if (await context.Tickets.AnyAsync())
                return;

            // Get all 5 users
            var admin1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin1@example.com");
            var admin2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin2@example.com");
            var admin3 = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin3@example.com");
            var user1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "azizayman1421@gmail.com");
            var user2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "zzozamai@gmail.com");

            var users = new[] { admin1, admin2, admin3, user1, user2 };
            var breakfastPrice = 7.00m;
            var lunchPrice = 10.00m;

            var orders = new List<Order>();
            var tickets = new List<Ticket>();
            var random = new Random();

            // Create month-specific ticket distributions to vary per month
            var monthDistributions = new Dictionary<int, (int breakfastMin, int breakfastMax, int lunchMin, int lunchMax)>
            {
                { 1, (1, 3, 2, 4) },     // January: fewer breakfast, more lunch
                { 2, (3, 5, 1, 3) },     // February: more breakfast, fewer lunch
                { 3, (2, 4, 2, 4) },     // March: balanced
                { 4, (4, 6, 1, 2) }      // April: many breakfast, few lunch
            };

            foreach (var user in users)
            {
                foreach (var month in monthDistributions.Keys)
                {
                    var (breakfastMin, breakfastMax, lunchMin, lunchMax) = monthDistributions[month];

                    // Create one breakfast order per month per user with varied ticket count
                    int breakfastTicketCount = random.Next(breakfastMin, breakfastMax + 1);

                    if (breakfastTicketCount > 0)
                    {
                        var breakfastOrder = new Order
                        {
                            CustomerID = user.Id,
                            OrderDate = new DateTime(2025, month, random.Next(1, DateTime.DaysInMonth(2025, month) + 1)),
                            Status = "Completed",
                            BreakfastTicketsCount = breakfastTicketCount,
                            LunchTicketsCount = 0,
                            TotalPaid = (float)(breakfastPrice * breakfastTicketCount)
                        };
                        orders.Add(breakfastOrder);

                        // Create breakfast tickets for this order
                        for (int i = 0; i < breakfastTicketCount; i++)
                        {
                            // Vary redemption rates by month
                            bool isRedeemed = random.NextDouble() < GetRedemptionRate(month);

                            tickets.Add(new Ticket
                            {
                                Order = breakfastOrder,
                                QRCode = GenerateQRCode(),
                                MealType = "الإفطار",
                                IsRedeemed = isRedeemed,
                                Price = breakfastPrice
                            });
                        }
                    }

                    // Create one lunch order per month per user with varied ticket count
                    int lunchTicketCount = random.Next(lunchMin, lunchMax + 1);

                    if (lunchTicketCount > 0)
                    {
                        var lunchOrder = new Order
                        {
                            CustomerID = user.Id,
                            OrderDate = new DateTime(2025, month, random.Next(1, DateTime.DaysInMonth(2025, month) + 1)),
                            Status = "Completed",
                            BreakfastTicketsCount = 0,
                            LunchTicketsCount = lunchTicketCount,
                            TotalPaid = (float)(lunchPrice * lunchTicketCount)
                        };
                        orders.Add(lunchOrder);

                        // Create lunch tickets for this order
                        for (int i = 0; i < lunchTicketCount; i++)
                        {
                            // Vary redemption rates by month
                            bool isRedeemed = random.NextDouble() < GetRedemptionRate(month);

                            tickets.Add(new Ticket
                            {
                                Order = lunchOrder,
                                QRCode = GenerateQRCode(),
                                MealType = "الغداء",
                                IsRedeemed = isRedeemed,
                                Price = lunchPrice
                            });
                        }
                    }
                }
            }

            // First add and save orders
            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();

            // Then add and save tickets
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();

            Console.WriteLine($"Added {orders.Count} orders and {tickets.Count} tickets.");
        }

        // Get redemption rate based on month (simulating seasonal variations)
        private static double GetRedemptionRate(int month)
        {
            return month switch
            {
                1 => 0.4,  // January: 40% redemption rate
                2 => 0.6,  // February: 60% redemption rate
                3 => 0.7,  // March: 70% redemption rate
                4 => 0.5,  // April: 50% redemption rate
                _ => 0.5   // Default: 50% redemption rate
            };
        }

        // Generate a random QR code string (for demonstration purposes)
        private static string GenerateQRCode()
        {
            var bytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }
    }
}
