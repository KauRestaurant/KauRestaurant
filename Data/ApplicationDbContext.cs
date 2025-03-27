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
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Terms> Terms { get; set; }

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

            // Seed Meal data
            modelBuilder.Entity<Meal>().HasData(
                // Breakfast (الإفطار)
                new Meal
                {
                    MealID = 1,
                    MealName = "فطائر بالجبن",
                    Description = "فطائر طازجة محشوة بالجبنة المشكلة المذابة، تقدم ساخنة مع زيت الزيتون والزعتر.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "بيض مقلي",
                    Description = "بيض مقلي على الطريقة العربية، يقدم مع الخبز العربي والخضروات الطازجة.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "فول مدمس",
                    Description = "فول مدمس مطبوخ بالطريقة التقليدية مع زيت الزيتون والثوم والليمون، غني بالبروتين النباتي.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "حمص بالطحينة",
                    Description = "حمص بالطحينة متبل بزيت الزيتون والليمون، مزين بحبات الحمص الكاملة والبقدونس.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "شاي عربي",
                    Description = "شاي عربي معطر بالهيل والزعفران، يقدم ساخناً في أكواب تقليدية.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "عصير برتقال طازج",
                    Description = "عصير برتقال طازج معصور في المطعم، غني بفيتامين سي والمذاق المنعش.",
                    PicturePath = "/images/meal.png",
                    Calories = 120,
                    Protein = 1,
                    Carbs = 28,
                    Fat = 0,
                    MealType = "مشروب",
                    MealCategory = "الإفطار"
                },

                // Lunch (الغداء)
                new Meal
                {
                    MealID = 7,
                    MealName = "كبسة لحم",
                    Description = "كبسة لحم سعودية تقليدية، مطبوخة ببهارات الكبسة المميزة مع قطع اللحم الطرية والأرز البسمتي.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "دجاج مشوي",
                    Description = "دجاج مشوي متبل بالأعشاب والبهارات العربية، مشوي على الفحم ليكتسب نكهة مميزة.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "سلطة خضراء",
                    Description = "سلطة خضراء منعشة مع خضروات موسمية طازجة وتتبيلة خاصة بالمطعم.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "شوربة عدس",
                    Description = "شوربة عدس تقليدية، مطبوخة بالطريقة العربية مع البهارات والليمون وزيت الزيتون.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "أم علي",
                    Description = "أم علي، حلوى مصرية شهيرة مصنوعة من العجينة الهشة والمكسرات والحليب والقشطة.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "لبن عيران",
                    Description = "لبن عيران منعش، مشروب تقليدي من اللبن المخفوق مع الماء والنعناع والملح.",
                    PicturePath = "/images/meal.png",
                    Calories = 90,
                    Protein = 8,
                    Carbs = 12,
                    Fat = 5,
                    MealType = "مشروب",
                    MealCategory = "الغداء"
                },

                // Dinner (العشاء)
                new Meal
                {
                    MealID = 13,
                    MealName = "شاورما دجاج",
                    Description = "شاورما دجاج عربية تقليدية، مشوية على السيخ ومقدمة مع الخبز العربي والطحينة والخضروات.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "برجر لحم",
                    Description = "برجر لحم محضر من اللحم البقري الطازج 100% مع الخضروات والصلصة الخاصة، يقدم مع خبز البرجر المحمص.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "بطاطس مقلية",
                    Description = "بطاطس مقلية مقرمشة من الخارج وطرية من الداخل، تقدم مع الكاتشب والمايونيز.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "سلطة سيزر",
                    Description = "سلطة سيزر كلاسيكية مع خس رومين، جبن البارميزان، قطع خبز محمصة، وصلصة سيزر المميزة.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "كنافة",
                    Description = "كنافة عربية تقليدية محشوة بالجبنة الحلوة ومغطاة بالقطر ومزينة بالفستق الحلبي.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "عصير ليمون بالنعناع",
                    Description = "عصير ليمون طازج ومنعش مع النعناع، مثالي لتنشيط الجسم وتعزيز المناعة.",
                    PicturePath = "/images/meal.png",
                    Calories = 80,
                    Protein = 1,
                    Carbs = 20,
                    Fat = 0,
                    MealType = "مشروب",
                    MealCategory = "العشاء"
                },

                new Meal
                {
                    MealID = 19,
                    MealName = "عصير برتقال طازج",
                    Description = "عصير برتقال طازج معصور من أجود أنواع البرتقال، غني بالفيتامينات والمعادن.",
                    PicturePath = "/images/meal.png",
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
                    MealName = "كعكة الشوكولاتة",
                    Description = "كعكة الشوكولاتة الغنية بطبقات الشوكولاتة الداكنة والكريمة، مثالية لمحبي الحلويات.",
                    PicturePath = "/images/meal.png",
                    Calories = 420,
                    Protein = 5,
                    Carbs = 63,
                    Fat = 22,
                    MealType = "حلوى",
                    MealCategory = "العشاء"
                }
            );


            // Seed MenuMeal join table with the relationships
            modelBuilder.Entity<MenuMeal>().HasData(
                // Sunday (الأحد) meals - MenuID = 1
                new MenuMeal { MenuMealID = 1, MenuID = 1, MealID = 1 },
                new MenuMeal { MenuMealID = 2, MenuID = 1, MealID = 2 },
                new MenuMeal { MenuMealID = 3, MenuID = 1, MealID = 3 },
                new MenuMeal { MenuMealID = 4, MenuID = 1, MealID = 4 },
                new MenuMeal { MenuMealID = 5, MenuID = 1, MealID = 5 },
                new MenuMeal { MenuMealID = 6, MenuID = 1, MealID = 6 },
                new MenuMeal { MenuMealID = 7, MenuID = 1, MealID = 7 },
                new MenuMeal { MenuMealID = 8, MenuID = 1, MealID = 8 },
                new MenuMeal { MenuMealID = 9, MenuID = 1, MealID = 9 },
                new MenuMeal { MenuMealID = 10, MenuID = 1, MealID = 10 },
                new MenuMeal { MenuMealID = 11, MenuID = 1, MealID = 11 },
                new MenuMeal { MenuMealID = 12, MenuID = 1, MealID = 12 },
                new MenuMeal { MenuMealID = 13, MenuID = 1, MealID = 13 },
                new MenuMeal { MenuMealID = 14, MenuID = 1, MealID = 14 },
                new MenuMeal { MenuMealID = 15, MenuID = 1, MealID = 15 },
                new MenuMeal { MenuMealID = 16, MenuID = 1, MealID = 16 },
                new MenuMeal { MenuMealID = 17, MenuID = 1, MealID = 17 },
                new MenuMeal { MenuMealID = 18, MenuID = 1, MealID = 18 },

                // Monday (الإثنين) meals - MenuID = 2
                new MenuMeal { MenuMealID = 19, MenuID = 2, MealID = 19 },
                new MenuMeal { MenuMealID = 20, MenuID = 2, MealID = 1 },
                new MenuMeal { MenuMealID = 21, MenuID = 2, MealID = 7 },
                new MenuMeal { MenuMealID = 22, MenuID = 2, MealID = 13 },

                // Tuesday (الثلاثاء) meals - MenuID = 3
                new MenuMeal { MenuMealID = 23, MenuID = 3, MealID = 20 },
                new MenuMeal { MenuMealID = 24, MenuID = 3, MealID = 2 },
                new MenuMeal { MenuMealID = 25, MenuID = 3, MealID = 8 },
                new MenuMeal { MenuMealID = 26, MenuID = 3, MealID = 14 },

                // Wednesday (الأربعاء) meals - MenuID = 4
                new MenuMeal { MenuMealID = 27, MenuID = 4, MealID = 3 },
                new MenuMeal { MenuMealID = 28, MenuID = 4, MealID = 9 },
                new MenuMeal { MenuMealID = 29, MenuID = 4, MealID = 15 },
                new MenuMeal { MenuMealID = 30, MenuID = 4, MealID = 19 },

                // Thursday (الخميس) meals - MenuID = 5
                new MenuMeal { MenuMealID = 31, MenuID = 5, MealID = 4 },
                new MenuMeal { MenuMealID = 32, MenuID = 5, MealID = 10 },
                new MenuMeal { MenuMealID = 33, MenuID = 5, MealID = 16 },
                new MenuMeal { MenuMealID = 34, MenuID = 5, MealID = 20 }
            );

            // Seed Restaurant data
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    RestaurantID = 1,
                    Name = "مطعم جامعة الملك عبدالعزيز",
                    Description = "في المطعم الجامعي الرسمي لجامعة الملك عبد العزيز، نقدم وجبات طازجة وعالية الجودة للطلاب وأعضاء هيئة التدريس. نحرص على تقديم أطباق متنوعة ومغذية في بيئة نظيفة ومرحبة.",
                    PhotoPath = "/images/restaurant.png",
                    LocationUrl = "https://maps.app.goo.gl/KFBdpmH7E88Lzvy49",
                    Address = "جامعة الملك عبد العزيز، جدة، المملكة العربية السعودية",
                    PhoneNumber = "+9665********",
                    Email = "restaurant@kau.edu.sa",
                    BreakfastOpenTime = new TimeSpan(7, 0, 0), // 7:00 AM
                    BreakfastCloseTime = new TimeSpan(10, 30, 0), // 10:30 AM
                    ServesBreakfast = true,
                    LunchOpenTime = new TimeSpan(12, 0, 0), // 12:00 PM
                    LunchCloseTime = new TimeSpan(15, 0, 0), // 3:00 PM
                    ServesLunch = true,
                    DinnerOpenTime = new TimeSpan(18, 0, 0), // 6:00 PM
                    DinnerCloseTime = new TimeSpan(22, 0, 0), // 10:00 PM
                    ServesDinner = true,
                    DaysOpen = "من الأحد إلى الخميس"
                }
            );

            // Seed FAQ data
            modelBuilder.Entity<FAQ>().HasData(
                new FAQ
                {
                    FAQID = 1,
                    Question = "كيف يمكنني شراء تذاكر وجبات؟",
                    Answer = "يمكنك شراء تذاكر الوجبات من خلال تسجيل الدخول إلى حسابك، ثم الانتقال إلى صفحة شراء التذاكر واختيار عدد الوجبات التي ترغب بها لكل فترة (الإفطار، الغداء، العشاء).",
                    DisplayOrder = 1
                },
                new FAQ
                {
                    FAQID = 2,
                    Question = "ما هي أوقات عمل المطعم؟",
                    Answer = "يعمل المطعم من الأحد إلى الخميس، وساعات العمل هي: الإفطار من 7:00 صباحًا إلى 10:30 صباحًا، الغداء من 12:00 ظهرًا إلى 3:00 عصرًا، والعشاء من 6:00 مساءً إلى 10:00 مساءً.",
                    DisplayOrder = 2
                },
                new FAQ
                {
                    FAQID = 3,
                    Question = "كيف يمكنني استخدام التذاكر التي اشتريتها؟",
                    Answer = "بعد شراء التذاكر، يمكنك عرض جميع تذاكرك في صفحة 'تذاكري'. عند زيارة المطعم، ما عليك سوى إظهار رمز QR الخاص بالتذكرة للموظف ليتم مسحه وتأكيد استخدام الوجبة.",
                    DisplayOrder = 3
                },
                new FAQ
                {
                    FAQID = 4,
                    Question = "هل يمكنني إلغاء التذاكر التي اشتريتها؟",
                    Answer = "لا يمكن إلغاء التذاكر بعد الشراء. لذا يرجى التأكد من اختيارك قبل إتمام عملية الشراء.",
                    DisplayOrder = 4
                },
                new FAQ
                {
                    FAQID = 5,
                    Question = "هل يمكنني معرفة قائمة الطعام مسبقاً؟",
                    Answer = "نعم، يمكنك الاطلاع على قائمة الطعام الأسبوعية في صفحة 'القائمة' على موقعنا. يتم تحديث القائمة أسبوعياً.",
                    DisplayOrder = 5
                }
            );

            // Seed Terms data as individual items
            modelBuilder.Entity<Terms>().HasData(
                new Terms
                {
                    TermID = 1,
                    Title = "عام",
                    Content = "تنطبق هذه الشروط والأحكام على جميع الخدمات المقدمة من مطعم جامعة الملك عبدالعزيز.",
                    LastUpdated = new DateTime(2025, 1, 15),
                    DisplayOrder = 1
                },
                new Terms
                {
                    TermID = 2,
                    Title = "التذاكر والدفع",
                    Content = "جميع المبيعات نهائية ولا يمكن استرداد قيمة التذاكر بعد الشراء. يجب استخدام التذاكر خلال الفصل الدراسي الذي تم شراؤها فيه. التذاكر غير قابلة للتحويل ويجب استخدامها من قبل مالك الحساب فقط.",
                    LastUpdated = new DateTime(2025, 1, 15),
                    DisplayOrder = 2
                },
                new Terms
                {
                    TermID = 3,
                    Title = "الاستخدام",
                    Content = "يتعهد المستخدم بعدم إساءة استخدام الخدمة أو محاولة التحايل على النظام. يحتفظ المطعم بالحق في رفض الخدمة لأي شخص ينتهك هذه الشروط.",
                    LastUpdated = new DateTime(2025, 1, 15),
                    DisplayOrder = 3
                },
                new Terms
                {
                    TermID = 4,
                    Title = "الخصوصية",
                    Content = "نحن نحترم خصوصيتك ونلتزم بحماية بياناتك الشخصية وفقاً لسياسة الخصوصية الخاصة بنا.",
                    LastUpdated = new DateTime(2025, 1, 15),
                    DisplayOrder = 4
                },
                new Terms
                {
                    TermID = 5,
                    Title = "التغييرات على الشروط",
                    Content = "يحتفظ المطعم بالحق في تعديل هذه الشروط في أي وقت. سيتم نشر التغييرات على موقعنا.",
                    LastUpdated = new DateTime(2025, 1, 15),
                    DisplayOrder = 5
                }
            );
        }
    }
}
