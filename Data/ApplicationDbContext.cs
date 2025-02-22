using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KauRestaurant.Models;
using KauRestaurant.Areas.Identity.Data;

namespace KauRestaurant.Data
{
    public class ApplicationDbContext : IdentityDbContext<KauRestaurantUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Meal> Meals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Menu data
            modelBuilder.Entity<Menu>().HasData(
                new Menu { MenuID = 1, Day = "SAT" },
                new Menu { MenuID = 2, Day = "SUN" },
                new Menu { MenuID = 3, Day = "MON" },
                new Menu { MenuID = 4, Day = "TUE" },
                new Menu { MenuID = 5, Day = "THU" }
            );

            // Seed Meal data
            modelBuilder.Entity<Meal>().HasData(
                new Meal
                {
                    MealID = 1,
                    MenuID = 1,
                    MealName = "Pancakes",
                    NutritionalInfo = "{\"calories\": 250, \"protein\": 8, \"carbs\": 45}",
                    MealCategory = "Breakfast"
                },
                new Meal
                {
                    MealID = 2,
                    MenuID = 1,
                    MealName = "Grilled Chicken",
                    NutritionalInfo = "{\"calories\": 350, \"protein\": 30, \"carbs\": 0}",
                    MealCategory = "Lunch"
                }
            );
        }
    }
}
