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
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<MenuMeal> MenuMeals { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<TicketPrice> TicketPrices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Price property in Ticket table
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<MenuMeal>().ToTable("MenuMeals");

            // Configure the many-to-many relationship
            modelBuilder.Entity<MenuMeal>()
                .HasKey(mm => mm.MenuMealID);

            modelBuilder.Entity<MenuMeal>()
                .HasOne(mm => mm.Menu)
                .WithMany(m => m.MenuMeals)
                .HasForeignKey(mm => mm.MenuID);

            modelBuilder.Entity<MenuMeal>()
                .HasOne(mm => mm.Meal)
                .WithMany(m => m.MenuMeals)
                .HasForeignKey(mm => mm.MealID);

            // Seed Menu data
            modelBuilder.Entity<Menu>().HasData(
                new Menu { MenuID = 1, Day = "الأحد" },
                new Menu { MenuID = 2, Day = "الإثنين" },
                new Menu { MenuID = 3, Day = "الثلاثاء" },
                new Menu { MenuID = 4, Day = "الأربعاء" },
                new Menu { MenuID = 5, Day = "الخميس" },
                new Menu { MenuID = 6, Day = "الجمعة" },
                new Menu { MenuID = 7, Day = "السبت" }
            );

            // Seed meals data
            modelBuilder.Entity<Meal>().HasData(
                // Breakfast - 15 meals
                new Meal
                {
                    MealID = 1,
                    MealName = "فطائر بالجبن",
                    Description = "فطائر طازجة محشوة بالجبنة المشكلة المذابة، تقدم ساخنة مع زيت الزيتون والزعتر.",
                    PicturePath = "/images/meal.png",
                    Protein = 10,
                    Carbs = 30,
                    Fat = 12,
                    Calories = 10 * 4 + 30 * 4 + 12 * 9, // 268 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 2,
                    MealName = "بيض مقلي",
                    Description = "بيض مقلي على الطريقة العربية، يقدم مع الخبز العربي والخضروات الطازجة.",
                    PicturePath = "/images/meal.png",
                    Protein = 12,
                    Carbs = 2,
                    Fat = 14,
                    Calories = 12 * 4 + 2 * 4 + 14 * 9, // 182 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 3,
                    MealName = "فول مدمس",
                    Description = "فول مدمس مطبوخ بالطريقة التقليدية مع زيت الزيتون والثوم والليمون، غني بالبروتين النباتي.",
                    PicturePath = "/images/meal.png",
                    Protein = 15,
                    Carbs = 35,
                    Fat = 5,
                    Calories = 15 * 4 + 35 * 4 + 5 * 9, // 245 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 4,
                    MealName = "حمص بالطحينة",
                    Description = "حمص بالطحينة متبل بزيت الزيتون والليمون، مزين بحبات الحمص الكاملة والبقدونس.",
                    PicturePath = "/images/meal.png",
                    Protein = 8,
                    Carbs = 25,
                    Fat = 9,
                    Calories = 8 * 4 + 25 * 4 + 9 * 9, // 213 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 5,
                    MealName = "شاي عربي",
                    Description = "شاي عربي معطر بالهيل والزعفران، يقدم ساخناً في أكواب تقليدية.",
                    PicturePath = "/images/meal.png",
                    Protein = 0,
                    Carbs = 1,
                    Fat = 0,
                    Calories = 0 * 4 + 1 * 4 + 0 * 9, // 4 calories
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 6,
                    MealName = "عصير برتقال طازج",
                    Description = "عصير برتقال طازج معصور في المطعم، غني بفيتامين سي والمذاق المنعش.",
                    PicturePath = "/images/meal.png",
                    Protein = 1,
                    Carbs = 28,
                    Fat = 0,
                    Calories = 1 * 4 + 28 * 4 + 0 * 9, // 116 calories
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 7,
                    MealName = "مناقيش زعتر",
                    Description = "مناقيش زعتر تقليدية مخبوزة في فرن طيني، مع مزيج الزعتر والسماق وزيت الزيتون.",
                    PicturePath = "/images/meal.png",
                    Protein = 6,
                    Carbs = 40,
                    Fat = 8,
                    Calories = 6 * 4 + 40 * 4 + 8 * 9, // 256 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 8,
                    MealName = "لبنة بالزيت",
                    Description = "لبنة طازجة تقدم مع زيت الزيتون والزعتر والخبز العربي الطازج.",
                    PicturePath = "/images/meal.png",
                    Protein = 10,
                    Carbs = 15,
                    Fat = 15,
                    Calories = 10 * 4 + 15 * 4 + 15 * 9, // 235 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 9,
                    MealName = "شكشوكة",
                    Description = "بيض مطبوخ في صلصة طماطم غنية بالتوابل والخضروات، طبق عربي شهير.",
                    PicturePath = "/images/meal.png",
                    Protein = 14,
                    Carbs = 10,
                    Fat = 12,
                    Calories = 14 * 4 + 10 * 4 + 12 * 9, // 204 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 10,
                    MealName = "فطيرة التفاح",
                    Description = "فطيرة تفاح محلية الصنع مع القرفة والعسل، مخبوزة حتى ذهبية اللون.",
                    PicturePath = "/images/meal.png",
                    Protein = 4,
                    Carbs = 45,
                    Fat = 10,
                    Calories = 4 * 4 + 45 * 4 + 10 * 9, // 286 calories
                    MealType = "حلوى",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 11,
                    MealName = "عصير تفاح",
                    Description = "عصير تفاح طازج مصنوع من تفاح موسمي، غني بالفيتامينات ومنعش المذاق.",
                    PicturePath = "/images/meal.png",
                    Protein = 0,
                    Carbs = 25,
                    Fat = 0,
                    Calories = 0 * 4 + 25 * 4 + 0 * 9, // ١٠٠ سعر حراري
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 12,
                    MealName = "عصيدة بالتمر",
                    Description = "عصيدة تقليدية محلاة بالتمر والعسل، مزينة بالمكسرات المحمصة.",
                    PicturePath = "/images/meal.png",
                    Protein = 5,
                    Carbs = 60,
                    Fat = 7,
                    Calories = 5 * 4 + 60 * 4 + 7 * 9, // 323 calories
                    MealType = "حلوى",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 13,
                    MealName = "مسخن دجاج",
                    Description = "خبز طابون مغطى بالبصل المكرمل والسماق مع قطع الدجاج المشوي.",
                    PicturePath = "/images/meal.png",
                    Protein = 22,
                    Carbs = 30,
                    Fat = 15,
                    Calories = 22 * 4 + 30 * 4 + 15 * 9, // 343 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 14,
                    MealName = "زبادي بالعسل",
                    Description = "زبادي طبيعي محلى بالعسل ومزين بالمكسرات والفواكه الطازجة.",
                    PicturePath = "/images/meal.png",
                    Protein = 8,
                    Carbs = 25,
                    Fat = 5,
                    Calories = 8 * 4 + 25 * 4 + 5 * 9, // 177 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الإفطار"
                },
                new Meal
                {
                    MealID = 15,
                    MealName = "كرواسون بالشوكولاتة",
                    Description = "كرواسون طازج محشو بالشوكولاتة الغنية، يقدم دافئاً.",
                    PicturePath = "/images/meal.png",
                    Protein = 6,
                    Carbs = 35,
                    Fat = 18,
                    Calories = 6 * 4 + 35 * 4 + 18 * 9, // 326 calories
                    MealType = "حلوى",
                    MealCategory = "الإفطار"
                },

                // Lunch - 15 different meals
                new Meal
                {
                    MealID = 16,
                    MealName = "كبسة لحم",
                    Description = "كبسة لحم سعودية تقليدية، مطبوخة ببهارات الكبسة المميزة مع قطع اللحم الطرية والأرز البسمتي.",
                    PicturePath = "/images/meal.png",
                    Protein = 28,
                    Carbs = 55,
                    Fat = 15,
                    Calories = 28 * 4 + 55 * 4 + 15 * 9, // 467 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 17,
                    MealName = "دجاج مشوي",
                    Description = "دجاج مشوي متبل بالأعشاب والبهارات العربية، مشوي على الفحم ليكتسب نكهة مميزة.",
                    PicturePath = "/images/meal.png",
                    Protein = 30,
                    Carbs = 0,
                    Fat = 10,
                    Calories = 30 * 4 + 0 * 4 + 10 * 9, // 210 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 18,
                    MealName = "سلطة خضراء",
                    Description = "سلطة خضراء منعشة مع خضروات موسمية طازجة وتتبيلة خاصة بالمطعم.",
                    PicturePath = "/images/meal.png",
                    Protein = 3,
                    Carbs = 12,
                    Fat = 5,
                    Calories = 3 * 4 + 12 * 4 + 5 * 9, // 105 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 19,
                    MealName = "شوربة عدس",
                    Description = "شوربة عدس تقليدية، مطبوخة بالطريقة العربية مع البهارات والليمون وزيت الزيتون.",
                    PicturePath = "/images/meal.png",
                    Protein = 10,
                    Carbs = 30,
                    Fat = 5,
                    Calories = 10 * 4 + 30 * 4 + 5 * 9, // 205 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 20,
                    MealName = "أم علي",
                    Description = "أم علي، حلوى مصرية شهيرة مصنوعة من العجينة الهشة والمكسرات والحليب والقشطة.",
                    PicturePath = "/images/meal.png",
                    Protein = 8,
                    Carbs = 52,
                    Fat = 15,
                    Calories = 8 * 4 + 52 * 4 + 15 * 9, // 375 calories
                    MealType = "حلوى",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 21,
                    MealName = "لبن عيران",
                    Description = "لبن عيران منعش، مشروب تقليدي من اللبن المخفوق مع الماء والنعناع والملح.",
                    PicturePath = "/images/meal.png",
                    Protein = 8,
                    Carbs = 12,
                    Fat = 2,
                    Calories = 8 * 4 + 12 * 4 + 2 * 9, // 98 calories
                    MealType = "مشروب",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 22,
                    MealName = "برياني دجاج",
                    Description = "برياني دجاج هندي تقليدي مع الأرز البسمتي والبهارات الهندية والخضروات.",
                    PicturePath = "/images/meal.png",
                    Protein = 25,
                    Carbs = 60,
                    Fat = 12,
                    Calories = 25 * 4 + 60 * 4 + 12 * 9, // 448 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 23,
                    MealName = "مندي لحم",
                    Description = "مندي لحم يمني تقليدي مع الأرز المطبوخ على الفحم وبهارات المندي الخاصة.",
                    PicturePath = "/images/meal.png",
                    Protein = 30,
                    Carbs = 50,
                    Fat = 18,
                    Calories = 30 * 4 + 50 * 4 + 18 * 9, // 482 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 24,
                    MealName = "مجبوس سمك",
                    Description = "مجبوس سمك خليجي تقليدي مع الأرز والبهارات المميزة وقطع السمك الطازج.",
                    PicturePath = "/images/meal.png",
                    Protein = 28,
                    Carbs = 45,
                    Fat = 10,
                    Calories = 28 * 4 + 45 * 4 + 10 * 9, // 382 calories
                    MealType = "الطبق الرئيسي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 25,
                    MealName = "طاجين خضار",
                    Description = "طاجين خضار مغربي تقليدي مع البهارات المغربية والخضروات الموسمية.",
                    PicturePath = "/images/meal.png",
                    Protein = 8,
                    Carbs = 35,
                    Fat = 7,
                    Calories = 8 * 4 + 35 * 4 + 7 * 9, // 235 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 26,
                    MealName = "سلطة فتوش",
                    Description = "سلطة فتوش لبنانية مع الخضروات الطازجة وقطع الخبز المحمص والسماق.",
                    PicturePath = "/images/meal.png",
                    Protein = 5,
                    Carbs = 20,
                    Fat = 8,
                    Calories = 5 * 4 + 20 * 4 + 8 * 9, // 172 calories
                    MealType = "طبق جانبي",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 27,
                    MealName = "كنافة",
                    Description = "كنافة عربية تقليدية محشوة بالجبنة الحلوة ومغطاة بالقطر ومزينة بالفستق الحلبي.",
                    PicturePath = "/images/meal.png",
                    Protein = 10,
                    Carbs = 60,
                    Fat = 25,
                    Calories = 10 * 4 + 60 * 4 + 25 * 9, // 505 calories
                    MealType = "حلوى",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 28,
                    MealName = "عصير ليمون بالنعناع",
                    Description = "عصير ليمون طازج ومنعش مع النعناع، مثالي لتنشيط الجسم وتعزيز المناعة.",
                    PicturePath = "/images/meal.png",
                    Protein = 1,
                    Carbs = 20,
                    Fat = 0,
                    Calories = 1 * 4 + 20 * 4 + 0 * 9, // 84 calories
                    MealType = "مشروب",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 29,
                    MealName = "بسبوسة",
                    Description = "بسبوسة تقليدية مصنوعة من السميد والمغطاة بشراب السكر، مزينة بالمكسرات.",
                    PicturePath = "/images/meal.png",
                    Protein = 5,
                    Carbs = 65,
                    Fat = 15,
                    Calories = 5 * 4 + 65 * 4 + 15 * 9, // 415 calories
                    MealType = "حلوى",
                    MealCategory = "الغداء"
                },
                new Meal
                {
                    MealID = 30,
                    MealName = "شاي أخضر",
                    Description = "شاي أخضر محضَّر طازجًا ومليء بمضادات الأكسدة، يُقدَّم ساخنًا أو باردًا مع لمسة نعناع.",
                    PicturePath = "/images/meal.png",
                    Protein = 0,
                    Carbs = 2,
                    Fat = 0,
                    Calories = 0 * 4 + 2 * 4 + 0 * 9, // ٨ سعرات حرارية
                    MealType = "مشروب",
                    MealCategory = "الغداء"
                }
            );

            // Seed MenuMeal join table with the relationships
            modelBuilder.Entity<MenuMeal>().HasData(
                //–––– الأَحَد
                // الإفطار
                new MenuMeal { MenuMealID = 1, MenuID = 1, MealID = 1 },
                new MenuMeal { MenuMealID = 2, MenuID = 1, MealID = 2 },
                new MenuMeal { MenuMealID = 3, MenuID = 1, MealID = 3 },
                new MenuMeal { MenuMealID = 4, MenuID = 1, MealID = 4 },
                new MenuMeal { MenuMealID = 5, MenuID = 1, MealID = 10 },
                new MenuMeal { MenuMealID = 6, MenuID = 1, MealID = 15 },
                new MenuMeal { MenuMealID = 7, MenuID = 1, MealID = 5 },
                new MenuMeal { MenuMealID = 8, MenuID = 1, MealID = 6 },

                // الغداء
                new MenuMeal { MenuMealID = 9, MenuID = 1, MealID = 16 },
                new MenuMeal { MenuMealID = 10, MenuID = 1, MealID = 17 },
                new MenuMeal { MenuMealID = 11, MenuID = 1, MealID = 18 },
                new MenuMeal { MenuMealID = 12, MenuID = 1, MealID = 19 },
                new MenuMeal { MenuMealID = 13, MenuID = 1, MealID = 20 },
                new MenuMeal { MenuMealID = 14, MenuID = 1, MealID = 27 },
                new MenuMeal { MenuMealID = 15, MenuID = 1, MealID = 21 },
                new MenuMeal { MenuMealID = 16, MenuID = 1, MealID = 28 },

                //–––– الإثنين
                // الإفطار
                new MenuMeal { MenuMealID = 17, MenuID = 2, MealID = 7 },
                new MenuMeal { MenuMealID = 18, MenuID = 2, MealID = 9 },
                new MenuMeal { MenuMealID = 19, MenuID = 2, MealID = 8 },
                new MenuMeal { MenuMealID = 20, MenuID = 2, MealID = 14 },
                new MenuMeal { MenuMealID = 21, MenuID = 2, MealID = 12 },
                new MenuMeal { MenuMealID = 22, MenuID = 2, MealID = 15 },
                new MenuMeal { MenuMealID = 23, MenuID = 2, MealID = 5 },
                new MenuMeal { MenuMealID = 24, MenuID = 2, MealID = 11 },

                // الغداء
                new MenuMeal { MenuMealID = 25, MenuID = 2, MealID = 22 },
                new MenuMeal { MenuMealID = 26, MenuID = 2, MealID = 24 },
                new MenuMeal { MenuMealID = 27, MenuID = 2, MealID = 25 },
                new MenuMeal { MenuMealID = 28, MenuID = 2, MealID = 26 },
                new MenuMeal { MenuMealID = 29, MenuID = 2, MealID = 29 },
                new MenuMeal { MenuMealID = 30, MenuID = 2, MealID = 20 },
                new MenuMeal { MenuMealID = 31, MenuID = 2, MealID = 21 },
                new MenuMeal { MenuMealID = 32, MenuID = 2, MealID = 30 },

                //–––– الثلاثاء
                // الإفطار
                new MenuMeal { MenuMealID = 33, MenuID = 3, MealID = 1 },
                new MenuMeal { MenuMealID = 34, MenuID = 3, MealID = 7 },
                new MenuMeal { MenuMealID = 35, MenuID = 3, MealID = 3 },
                new MenuMeal { MenuMealID = 36, MenuID = 3, MealID = 8 },
                new MenuMeal { MenuMealID = 37, MenuID = 3, MealID = 10 },
                new MenuMeal { MenuMealID = 38, MenuID = 3, MealID = 12 },
                new MenuMeal { MenuMealID = 39, MenuID = 3, MealID = 6 },
                new MenuMeal { MenuMealID = 40, MenuID = 3, MealID = 11 },

                // الغدا3
                new MenuMeal { MenuMealID = 41, MenuID = 3, MealID = 23 },
                new MenuMeal { MenuMealID = 42, MenuID = 3, MealID = 17 },
                new MenuMeal { MenuMealID = 43, MenuID = 3, MealID = 18 },
                new MenuMeal { MenuMealID = 44, MenuID = 3, MealID = 25 },
                new MenuMeal { MenuMealID = 45, MenuID = 3, MealID = 27 },
                new MenuMeal { MenuMealID = 46, MenuID = 3, MealID = 29 },
                new MenuMeal { MenuMealID = 47, MenuID = 3, MealID = 28 },
                new MenuMeal { MenuMealID = 48, MenuID = 3, MealID = 30 },

                //–––– الأربعاء
                // الإفطار
                new MenuMeal { MenuMealID = 49, MenuID = 4, MealID = 2 },
                new MenuMeal { MenuMealID = 50, MenuID = 4, MealID = 9 },
                new MenuMeal { MenuMealID = 51, MenuID = 4, MealID = 4 },
                new MenuMeal { MenuMealID = 52, MenuID = 4, MealID = 14 },
                new MenuMeal { MenuMealID = 53, MenuID = 4, MealID = 15 },
                new MenuMeal { MenuMealID = 54, MenuID = 4, MealID = 10 },
                new MenuMeal { MenuMealID = 55, MenuID = 4, MealID = 5 },
                new MenuMeal { MenuMealID = 56, MenuID = 4, MealID = 6 },

                // الغداء
                new MenuMeal { MenuMealID = 57, MenuID = 4, MealID = 16 },
                new MenuMeal { MenuMealID = 58, MenuID = 4, MealID = 22 },
                new MenuMeal { MenuMealID = 59, MenuID = 4, MealID = 19 },
                new MenuMeal { MenuMealID = 60, MenuID = 4, MealID = 26 },
                new MenuMeal { MenuMealID = 61, MenuID = 4, MealID = 20 },
                new MenuMeal { MenuMealID = 62, MenuID = 4, MealID = 27 },
                new MenuMeal { MenuMealID = 63, MenuID = 4, MealID = 21 },
                new MenuMeal { MenuMealID = 64, MenuID = 4, MealID = 28 },

                //–––– الخميس
                // الإفطار
                new MenuMeal { MenuMealID = 65, MenuID = 5, MealID = 13 },
                new MenuMeal { MenuMealID = 66, MenuID = 5, MealID = 7 },
                new MenuMeal { MenuMealID = 67, MenuID = 5, MealID = 3 },
                new MenuMeal { MenuMealID = 68, MenuID = 5, MealID = 8 },
                new MenuMeal { MenuMealID = 69, MenuID = 5, MealID = 12 },
                new MenuMeal { MenuMealID = 70, MenuID = 5, MealID = 15 },
                new MenuMeal { MenuMealID = 71, MenuID = 5, MealID = 11 },
                new MenuMeal { MenuMealID = 72, MenuID = 5, MealID = 5 },

                // الغداء
                new MenuMeal { MenuMealID = 73, MenuID = 5, MealID = 24 },
                new MenuMeal { MenuMealID = 74, MenuID = 5, MealID = 23 },
                new MenuMeal { MenuMealID = 75, MenuID = 5, MealID = 18 },
                new MenuMeal { MenuMealID = 76, MenuID = 5, MealID = 25 },
                new MenuMeal { MenuMealID = 77, MenuID = 5, MealID = 29 },
                new MenuMeal { MenuMealID = 78, MenuID = 5, MealID = 20 },
                new MenuMeal { MenuMealID = 79, MenuID = 5, MealID = 30 },
                new MenuMeal { MenuMealID = 80, MenuID = 5, MealID = 21 },

                //–––– الجمعة
                // الإفطار
                new MenuMeal { MenuMealID = 81, MenuID = 6, MealID = 1 },
                new MenuMeal { MenuMealID = 82, MenuID = 6, MealID = 2 },
                new MenuMeal { MenuMealID = 83, MenuID = 6, MealID = 4 },
                new MenuMeal { MenuMealID = 84, MenuID = 6, MealID = 14 },
                new MenuMeal { MenuMealID = 85, MenuID = 6, MealID = 10 },
                new MenuMeal { MenuMealID = 86, MenuID = 6, MealID = 12 },
                new MenuMeal { MenuMealID = 87, MenuID = 6, MealID = 5 },
                new MenuMeal { MenuMealID = 88, MenuID = 6, MealID = 6 },

                // الغداء
                new MenuMeal { MenuMealID = 89, MenuID = 6, MealID = 17 },
                new MenuMeal { MenuMealID = 90, MenuID = 6, MealID = 22 },
                new MenuMeal { MenuMealID = 91, MenuID = 6, MealID = 19 },
                new MenuMeal { MenuMealID = 92, MenuID = 6, MealID = 26 },
                new MenuMeal { MenuMealID = 93, MenuID = 6, MealID = 27 },
                new MenuMeal { MenuMealID = 94, MenuID = 6, MealID = 29 },
                new MenuMeal { MenuMealID = 95, MenuID = 6, MealID = 28 },
                new MenuMeal { MenuMealID = 96, MenuID = 6, MealID = 30 },

                //–––– السبت
                // الإفطار
                new MenuMeal { MenuMealID = 97, MenuID = 7, MealID = 9 },
                new MenuMeal { MenuMealID = 98, MenuID = 7, MealID = 13 },
                new MenuMeal { MenuMealID = 99, MenuID = 7, MealID = 3 },
                new MenuMeal { MenuMealID = 100, MenuID = 7, MealID = 8 },
                new MenuMeal { MenuMealID = 101, MenuID = 7, MealID = 15 },
                new MenuMeal { MenuMealID = 102, MenuID = 7, MealID = 10 },
                new MenuMeal { MenuMealID = 103, MenuID = 7, MealID = 11 },
                new MenuMeal { MenuMealID = 104, MenuID = 7, MealID = 5 },

                // الغداء
                new MenuMeal { MenuMealID = 105, MenuID = 7, MealID = 23 },
                new MenuMeal { MenuMealID = 106, MenuID = 7, MealID = 24 },
                new MenuMeal { MenuMealID = 108, MenuID = 7, MealID = 25 },
                new MenuMeal { MenuMealID = 109, MenuID = 7, MealID = 20 },
                new MenuMeal { MenuMealID = 110, MenuID = 7, MealID = 27 },
                new MenuMeal { MenuMealID = 111, MenuID = 7, MealID = 21 },
                new MenuMeal { MenuMealID = 112, MenuID = 7, MealID = 28 }
            );


            // Add this inside the OnModelCreating method
            // Seed Restaurant data based on RestaurantInfo.cshtml
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1,
                    Location = "جامعة الملك عبد العزيز، طريق الجامعات، جدة، المملكة العربية السعودية",
                    GoogleMapsLink = "https://maps.app.goo.gl/5bbXVZiGK1sJfLPw9",
                    ContactNumber = "0126400000",
                    Email = "restaurant@kau.edu.sa",
                    WorkingDays = "السبت، الأحد، الإثنين، الثلاثاء، الأربعاء، الخميس، الجمعة",
                    BreakfastHours = "7:00 صباحاً—10:00 صباحاً",
                    LunchHours = "11:30 صباحاً—2:00 مساءً",
                    Photo = "/images/restaurant.png",
                    IsOpen = true
                }
            );

        }
    }
}
