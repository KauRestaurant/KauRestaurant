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
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Menu data
            modelBuilder.Entity<Menu>().HasData(
                new Menu { MenuID = 1, Day = "الأحد" },
                new Menu { MenuID = 2, Day = "الإثنين" },
                new Menu { MenuID = 3, Day = "الثلاثاء" },
                new Menu { MenuID = 4, Day = "الأربعاء" },
                new Menu { MenuID = 5, Day = "الخميس" }
            );

            // Seed Meal data
            modelBuilder.Entity<Meal>().HasData(
                // Sunday (الأحد) Breakfast (الإفطار)
                new Meal
                {
                    MealID = 1,
                    MenuID = 1,
                    MealName = "فطائر بالجبن",
                    Calories = 250,
                    Protein = 8,
                    Carbs = 45,
                    Fat = 10,
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 2,
                    MenuID = 1,
                    MealName = "بيض مقلي",
                    Calories = 185,
                    Protein = 12,
                    Carbs = 2,
                    Fat = 14,
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 3,
                    MenuID = 1,
                    MealName = "فول مدمس",
                    Calories = 220,
                    Protein = 15,
                    Carbs = 35,
                    Fat = 5,
                    MealType = "طبق جانبي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 4,
                    MenuID = 1,
                    MealName = "حمص بالطحينة",
                    Calories = 180,
                    Protein = 8,
                    Carbs = 25,
                    Fat = 9,
                    MealType = "طبق جانبي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 5,
                    MenuID = 1,
                    MealName = "شاي عربي",
                    Calories = 5,
                    Protein = 0,
                    Carbs = 1,
                    Fat = 0,
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 6,
                    MenuID = 1,
                    MealName = "عصير برتقال طازج",
                    Calories = 120,
                    Protein = 1,
                    Carbs = 28,
                    Fat = 0,
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },

                // Sunday (الأحد) Lunch (الغداء)
                new Meal
                {
                    MealID = 7,
                    MenuID = 1,
                    MealName = "كبسة لحم",
                    Calories = 450,
                    Protein = 28,
                    Carbs = 55,
                    Fat = 15,
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 8,
                    MenuID = 1,
                    MealName = "دجاج مشوي",
                    Calories = 350,
                    Protein = 30,
                    Carbs = 0,
                    Fat = 20,
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 9,
                    MenuID = 1,
                    MealName = "سلطة خضراء",
                    Calories = 65,
                    Protein = 3,
                    Carbs = 12,
                    Fat = 2,
                    MealType = "طبق جانبي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 10,
                    MenuID = 1,
                    MealName = "شوربة عدس",
                    Calories = 180,
                    Protein = 10,
                    Carbs = 30,
                    Fat = 5,
                    MealType = "طبق جانبي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 11,
                    MenuID = 1,
                    MealName = "أم علي",
                    Calories = 350,
                    Protein = 8,
                    Carbs = 52,
                    Fat = 15,
                    MealType = "حلوى",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 12,
                    MenuID = 1,
                    MealName = "لبن عيران",
                    Calories = 90,
                    Protein = 8,
                    Carbs = 12,
                    Fat = 5,
                    MealType = "مشروب",
                    MealCategory = "الغداء"
                },

                // Sunday (الأحد) Dinner (العشاء)
                new Meal
                {
                    MealID = 13,
                    MenuID = 1,
                    MealName = "شاورما دجاج",
                    Calories = 380,
                    Protein = 25,
                    Carbs = 40,
                    Fat = 20,
                    MealType = "الطبق الرئيسي",
                    MealCategory = "العشاء"
                },
                new Meal
                {
                    MealID = 14,
                    MenuID = 1,
                    MealName = "برجر لحم",
                    Calories = 420,
                    Protein = 28,
                    Carbs = 35,
                    Fat = 25,
                    MealType = "الطبق الرئيسي",
                    MealCategory = "العشاء"
                },
                new Meal
                {
                    MealID = 15,
                    MenuID = 1,
                    MealName = "بطاطس مقلية",
                    Calories = 365,
                    Protein = 4,
                    Carbs = 48,
                    Fat = 18,
                    MealType = "طبق جانبي",
                    MealCategory = "العشاء"
                },
                new Meal
                {
                    MealID = 16,
                    MenuID = 1,
                    MealName = "سلطة سيزر",
                    Calories = 150,
                    Protein = 8,
                    Carbs = 15,
                    Fat = 10,
                    MealType = "طبق جانبي",
                    MealCategory = "العشاء"
                },
                new Meal
                {
                    MealID = 17,
                    MenuID = 1,
                    MealName = "كنافة",
                    Calories = 400,
                    Protein = 6,
                    Carbs = 58,
                    Fat = 20,
                    MealType = "حلوى",
                    MealCategory = "العشاء"
                },
                new Meal
                {
                    MealID = 18,
                    MenuID = 1,
                    MealName = "عصير ليمون بالنعناع",
                    Calories = 80,
                    Protein = 1,
                    Carbs = 20,
                    Fat = 0,
                    MealType = "مشروب",
                    MealCategory = "العشاء"
                },

                // Keep some existing meals for other days
                new Meal
                {
                    MealID = 19,
                    MenuID = 2,
                    MealName = "عصير برتقال طازج",
                    Calories = 120,
                    Protein = 1,
                    Carbs = 28,
                    Fat = 0,
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 20,
                    MenuID = 3,
                    MealName = "كعكة الشوكولاتة",
                    Calories = 420,
                    Protein = 5,
                    Carbs = 63,
                    Fat = 22,
                    MealType = "حلوى",
                    MealCategory = "العشاء"
                }
            );
        }
    }
}
