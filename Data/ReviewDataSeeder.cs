using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Models;
using Microsoft.EntityFrameworkCore;

namespace KauRestaurant.Data
{
    public static class ReviewDataSeeder
    {
        public static async Task SeedReviews(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (await context.Reviews.AnyAsync())
                return;

            var admin1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin1@example.com");
            var admin2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin2@example.com");
            var admin3 = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin3@example.com");
            var user1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "azizayman1421@gmail.com");
            var user2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "zzozamai@gmail.com");

            var reviews = new List<Review>
            {
                new Review { CustomerID = admin1!.Id, MealID = 1, ReviewDate = new DateTime(2025, 1, 1), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 1, ReviewDate = new DateTime(2025, 1, 2), ReviewText = "ممتاز جداً", Rating = 4 },
                new Review { CustomerID = admin3!.Id, MealID = 1, ReviewDate = new DateTime(2025, 1, 3), ReviewText = "جيد", Rating = 4 },

                new Review { CustomerID = user1!.Id, MealID = 2, ReviewDate = new DateTime(2025, 1, 4), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = user2!.Id, MealID = 2, ReviewDate = new DateTime(2025, 1, 5), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin1!.Id, MealID = 2, ReviewDate = new DateTime(2025, 1, 6), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 2, ReviewDate = new DateTime(2025, 1, 7), ReviewText = "جيد", Rating = 4 },

                new Review { CustomerID = admin3!.Id, MealID = 3, ReviewDate = new DateTime(2025, 1, 8), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 3, ReviewDate = new DateTime(2025, 1, 9), ReviewText = "ممتاز جداً", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 3, ReviewDate = new DateTime(2025, 1, 10), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin1!.Id, MealID = 3, ReviewDate = new DateTime(2025, 1, 11), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin2!.Id, MealID = 3, ReviewDate = new DateTime(2025, 1, 12), ReviewText = "غير مرضٍ", Rating = 3 },

                new Review { CustomerID = admin3!.Id, MealID = 4, ReviewDate = new DateTime(2025, 1, 13), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 4, ReviewDate = new DateTime(2025, 1, 14), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 4, ReviewDate = new DateTime(2025, 1, 15), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin1!.Id, MealID = 5, ReviewDate = new DateTime(2025, 1, 16), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 5, ReviewDate = new DateTime(2025, 1, 17), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 5, ReviewDate = new DateTime(2025, 1, 18), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 5, ReviewDate = new DateTime(2025, 1, 19), ReviewText = "جيد", Rating = 4 },

                new Review { CustomerID = user2!.Id, MealID = 6, ReviewDate = new DateTime(2025, 1, 20), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin1!.Id, MealID = 6, ReviewDate = new DateTime(2025, 1, 21), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 6, ReviewDate = new DateTime(2025, 1, 22), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 6, ReviewDate = new DateTime(2025, 1, 23), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 6, ReviewDate = new DateTime(2025, 1, 24), ReviewText = "جيد", Rating = 4 },

                new Review { CustomerID = user2!.Id, MealID = 7, ReviewDate = new DateTime(2025, 1, 25), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin1!.Id, MealID = 7, ReviewDate = new DateTime(2025, 1, 26), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 7, ReviewDate = new DateTime(2025, 1, 27), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin3!.Id, MealID = 8, ReviewDate = new DateTime(2025, 1, 28), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 8, ReviewDate = new DateTime(2025, 1, 29), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 8, ReviewDate = new DateTime(2025, 1, 30), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin1!.Id, MealID = 8, ReviewDate = new DateTime(2025, 1, 31), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = admin2!.Id, MealID = 9, ReviewDate = new DateTime(2025, 2, 1), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = admin3!.Id, MealID = 9, ReviewDate = new DateTime(2025, 2, 2), ReviewText = "ممتاز", Rating = 4 },
                new Review { CustomerID = user1!.Id, MealID = 9, ReviewDate = new DateTime(2025, 2, 3), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 9, ReviewDate = new DateTime(2025, 2, 4), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin1!.Id, MealID = 9, ReviewDate = new DateTime(2025, 2, 5), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = admin2!.Id, MealID = 10, ReviewDate = new DateTime(2025, 2, 6), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin3!.Id, MealID = 10, ReviewDate = new DateTime(2025, 2, 7), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user1!.Id, MealID = 10, ReviewDate = new DateTime(2025, 2, 8), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = user2!.Id, MealID = 11, ReviewDate = new DateTime(2025, 2, 9), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin1!.Id, MealID = 11, ReviewDate = new DateTime(2025, 2, 10), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 11, ReviewDate = new DateTime(2025, 2, 11), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 11, ReviewDate = new DateTime(2025, 2, 12), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = user1!.Id, MealID = 12, ReviewDate = new DateTime(2025, 2, 13), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = user2!.Id, MealID = 12, ReviewDate = new DateTime(2025, 2, 14), ReviewText = "ممتاز", Rating = 4 },
                new Review { CustomerID = admin1!.Id, MealID = 12, ReviewDate = new DateTime(2025, 2, 15), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 12, ReviewDate = new DateTime(2025, 2, 16), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 12, ReviewDate = new DateTime(2025, 2, 17), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = user1!.Id, MealID = 13, ReviewDate = new DateTime(2025, 2, 18), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin1!.Id, MealID = 13, ReviewDate = new DateTime(2025, 2, 19), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 13, ReviewDate = new DateTime(2025, 2, 20), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin3!.Id, MealID = 14, ReviewDate = new DateTime(2025, 2, 21), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user2!.Id, MealID = 14, ReviewDate = new DateTime(2025, 2, 22), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin1!.Id, MealID = 14, ReviewDate = new DateTime(2025, 2, 23), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin2!.Id, MealID = 14, ReviewDate = new DateTime(2025, 2, 24), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = admin3!.Id, MealID = 15, ReviewDate = new DateTime(2025, 2, 25), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 15, ReviewDate = new DateTime(2025, 2, 26), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 15, ReviewDate = new DateTime(2025, 2, 27), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin1!.Id, MealID = 15, ReviewDate = new DateTime(2025, 2, 28), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 15, ReviewDate = new DateTime(2025, 3, 1), ReviewText = "جيد", Rating = 4 },

                new Review { CustomerID = user1!.Id, MealID = 16, ReviewDate = new DateTime(2025, 3, 2), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = user2!.Id, MealID = 16, ReviewDate = new DateTime(2025, 3, 3), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin1!.Id, MealID = 16, ReviewDate = new DateTime(2025, 3, 4), ReviewText = "جيد", Rating = 4 },

                new Review { CustomerID = admin2!.Id, MealID = 17, ReviewDate = new DateTime(2025, 3, 5), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 17, ReviewDate = new DateTime(2025, 3, 6), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 17, ReviewDate = new DateTime(2025, 3, 7), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 17, ReviewDate = new DateTime(2025, 3, 8), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin1!.Id, MealID = 18, ReviewDate = new DateTime(2025, 3, 9), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 18, ReviewDate = new DateTime(2025, 3, 10), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin3!.Id, MealID = 18, ReviewDate = new DateTime(2025, 3, 11), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user1!.Id, MealID = 18, ReviewDate = new DateTime(2025, 3, 12), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = user2!.Id, MealID = 18, ReviewDate = new DateTime(2025, 3, 13), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = admin1!.Id, MealID = 19, ReviewDate = new DateTime(2025, 3, 14), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 19, ReviewDate = new DateTime(2025, 3, 15), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user1!.Id, MealID = 19, ReviewDate = new DateTime(2025, 3, 16), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin3!.Id, MealID = 20, ReviewDate = new DateTime(2025, 3, 17), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user2!.Id, MealID = 20, ReviewDate = new DateTime(2025, 3, 18), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin1!.Id, MealID = 20, ReviewDate = new DateTime(2025, 3, 19), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin2!.Id, MealID = 20, ReviewDate = new DateTime(2025, 3, 20), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = user1!.Id, MealID = 21, ReviewDate = new DateTime(2025, 3, 21), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = user2!.Id, MealID = 21, ReviewDate = new DateTime(2025, 3, 22), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin1!.Id, MealID = 21, ReviewDate = new DateTime(2025, 3, 23), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 21, ReviewDate = new DateTime(2025, 3, 24), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 21, ReviewDate = new DateTime(2025, 3, 25), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = user1!.Id, MealID = 22, ReviewDate = new DateTime(2025, 3, 26), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin3!.Id, MealID = 22, ReviewDate = new DateTime(2025, 3, 27), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 22, ReviewDate = new DateTime(2025, 3, 28), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin1!.Id, MealID = 23, ReviewDate = new DateTime(2025, 3, 29), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 23, ReviewDate = new DateTime(2025, 3, 30), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user1!.Id, MealID = 23, ReviewDate = new DateTime(2025, 3, 31), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 23, ReviewDate = new DateTime(2025, 4, 1), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = user2!.Id, MealID = 24, ReviewDate = new DateTime(2025, 4, 2), ReviewText = "مذهل", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 24, ReviewDate = new DateTime(2025, 4, 3), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin1!.Id, MealID = 24, ReviewDate = new DateTime(2025, 4, 4), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 24, ReviewDate = new DateTime(2025, 4, 5), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 24, ReviewDate = new DateTime(2025, 4, 6), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = admin1!.Id, MealID = 25, ReviewDate = new DateTime(2025, 4, 7), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 25, ReviewDate = new DateTime(2025, 4, 8), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user1!.Id, MealID = 25, ReviewDate = new DateTime(2025, 4, 9), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin1!.Id, MealID = 26, ReviewDate = new DateTime(2025, 4, 10), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin2!.Id, MealID = 26, ReviewDate = new DateTime(2025, 4, 11), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin3!.Id, MealID = 26, ReviewDate = new DateTime(2025, 4, 12), ReviewText = "مذهل", Rating = 5 },

                new Review { CustomerID = user1!.Id, MealID = 27, ReviewDate = new DateTime(2025, 4, 13), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user2!.Id, MealID = 27, ReviewDate = new DateTime(2025, 4, 14), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin1!.Id, MealID = 27, ReviewDate = new DateTime(2025, 4, 15), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin2!.Id, MealID = 27, ReviewDate = new DateTime(2025, 4, 16), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = admin3!.Id, MealID = 28, ReviewDate = new DateTime(2025, 4, 17), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = user1!.Id, MealID = 28, ReviewDate = new DateTime(2025, 4, 18), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = user2!.Id, MealID = 28, ReviewDate = new DateTime(2025, 4, 19), ReviewText = "متوسط", Rating = 3 },

                new Review { CustomerID = admin1!.Id, MealID = 29, ReviewDate = new DateTime(2025, 4, 20), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 29, ReviewDate = new DateTime(2025, 4, 21), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin3!.Id, MealID = 29, ReviewDate = new DateTime(2025, 4, 22), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = user1!.Id, MealID = 29, ReviewDate = new DateTime(2025, 4, 23), ReviewText = "ممتاز", Rating = 5 },

                new Review { CustomerID = user2!.Id, MealID = 30, ReviewDate = new DateTime(2025, 4, 24), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = admin1!.Id, MealID = 30, ReviewDate = new DateTime(2025, 4, 25), ReviewText = "ممتاز", Rating = 5 },
                new Review { CustomerID = admin2!.Id, MealID = 30, ReviewDate = new DateTime(2025, 4, 26), ReviewText = "جيد", Rating = 4 },
                new Review { CustomerID = admin3!.Id, MealID = 30, ReviewDate = new DateTime(2025, 4, 27), ReviewText = "متوسط", Rating = 3 },
                new Review { CustomerID = user1!.Id, MealID = 30, ReviewDate = new DateTime(2025, 4, 28), ReviewText = "ممتاز", Rating = 5 }
            };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }
    }
}
