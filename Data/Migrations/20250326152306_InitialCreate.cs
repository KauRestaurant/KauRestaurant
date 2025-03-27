using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    FAQID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.FAQID);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PicturePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    Protein = table.Column<int>(type: "int", nullable: false),
                    Carbs = table.Column<int>(type: "int", nullable: false),
                    Fat = table.Column<int>(type: "int", nullable: false),
                    MealCategory = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.MealID);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPaid = table.Column<float>(type: "real", nullable: false),
                    BreakfastTicketsCount = table.Column<int>(type: "int", nullable: false),
                    LunchTicketsCount = table.Column<int>(type: "int", nullable: false),
                    DinnerTicketsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    RestaurantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LocationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BreakfastOpenTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakfastCloseTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ServesBreakfast = table.Column<bool>(type: "bit", nullable: false),
                    LunchOpenTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchCloseTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ServesLunch = table.Column<bool>(type: "bit", nullable: false),
                    DinnerOpenTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    DinnerCloseTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ServesDinner = table.Column<bool>(type: "bit", nullable: false),
                    DaysOpen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.RestaurantID);
                });

            migrationBuilder.CreateTable(
                name: "SocialMedia",
                columns: table => new
                {
                    SocialMediaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedia", x => x.SocialMediaID);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    TermID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TermID);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MealID = table.Column<int>(type: "int", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Meals_MealID",
                        column: x => x.MealID,
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuMeals",
                columns: table => new
                {
                    MenuMealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    MealID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuMeals", x => x.MenuMealID);
                    table.ForeignKey(
                        name: "FK_MenuMeals_Meals_MealID",
                        column: x => x.MealID,
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuMeals_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRedeemed = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_Tickets_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "FAQID", "Answer", "DisplayOrder", "Question" },
                values: new object[,]
                {
                    { 1, "يمكنك شراء تذاكر الوجبات من خلال تسجيل الدخول إلى حسابك، ثم الانتقال إلى صفحة شراء التذاكر واختيار عدد الوجبات التي ترغب بها لكل فترة (الإفطار، الغداء، العشاء).", 1, "كيف يمكنني شراء تذاكر وجبات؟" },
                    { 2, "يعمل المطعم من الأحد إلى الخميس، وساعات العمل هي: الإفطار من 7:00 صباحًا إلى 10:30 صباحًا، الغداء من 12:00 ظهرًا إلى 3:00 عصرًا، والعشاء من 6:00 مساءً إلى 10:00 مساءً.", 2, "ما هي أوقات عمل المطعم؟" },
                    { 3, "بعد شراء التذاكر، يمكنك عرض جميع تذاكرك في صفحة 'تذاكري'. عند زيارة المطعم، ما عليك سوى إظهار رمز QR الخاص بالتذكرة للموظف ليتم مسحه وتأكيد استخدام الوجبة.", 3, "كيف يمكنني استخدام التذاكر التي اشتريتها؟" },
                    { 4, "لا يمكن إلغاء التذاكر بعد الشراء. لذا يرجى التأكد من اختيارك قبل إتمام عملية الشراء.", 4, "هل يمكنني إلغاء التذاكر التي اشتريتها؟" },
                    { 5, "نعم، يمكنك الاطلاع على قائمة الطعام الأسبوعية في صفحة 'القائمة' على موقعنا. يتم تحديث القائمة أسبوعياً.", 5, "هل يمكنني معرفة قائمة الطعام مسبقاً؟" }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "MealID", "Calories", "Carbs", "Description", "Fat", "MealCategory", "MealName", "MealType", "PicturePath", "Protein" },
                values: new object[,]
                {
                    { 1, 250, 45, "فطائر طازجة محشوة بالجبنة المشكلة المذابة، تقدم ساخنة مع زيت الزيتون والزعتر.", 10, "الإفطار", "فطائر بالجبن", "الطبق الرئيسي", "/images/meal.png", 8 },
                    { 2, 185, 2, "بيض مقلي على الطريقة العربية، يقدم مع الخبز العربي والخضروات الطازجة.", 14, "الإفطار", "بيض مقلي", "الطبق الرئيسي", "/images/meal.png", 12 },
                    { 3, 220, 35, "فول مدمس مطبوخ بالطريقة التقليدية مع زيت الزيتون والثوم والليمون، غني بالبروتين النباتي.", 5, "الإفطار", "فول مدمس", "طبق جانبي", "/images/meal.png", 15 },
                    { 4, 180, 25, "حمص بالطحينة متبل بزيت الزيتون والليمون، مزين بحبات الحمص الكاملة والبقدونس.", 9, "الإفطار", "حمص بالطحينة", "طبق جانبي", "/images/meal.png", 8 },
                    { 5, 5, 1, "شاي عربي معطر بالهيل والزعفران، يقدم ساخناً في أكواب تقليدية.", 0, "الإفطار", "شاي عربي", "مشروب", "/images/meal.png", 0 },
                    { 6, 120, 28, "عصير برتقال طازج معصور في المطعم، غني بفيتامين سي والمذاق المنعش.", 0, "الإفطار", "عصير برتقال طازج", "مشروب", "/images/meal.png", 1 },
                    { 7, 450, 55, "كبسة لحم سعودية تقليدية، مطبوخة ببهارات الكبسة المميزة مع قطع اللحم الطرية والأرز البسمتي.", 15, "الغداء", "كبسة لحم", "الطبق الرئيسي", "/images/meal.png", 28 },
                    { 8, 350, 0, "دجاج مشوي متبل بالأعشاب والبهارات العربية، مشوي على الفحم ليكتسب نكهة مميزة.", 20, "الغداء", "دجاج مشوي", "الطبق الرئيسي", "/images/meal.png", 30 },
                    { 9, 65, 12, "سلطة خضراء منعشة مع خضروات موسمية طازجة وتتبيلة خاصة بالمطعم.", 2, "الغداء", "سلطة خضراء", "طبق جانبي", "/images/meal.png", 3 },
                    { 10, 180, 30, "شوربة عدس تقليدية، مطبوخة بالطريقة العربية مع البهارات والليمون وزيت الزيتون.", 5, "الغداء", "شوربة عدس", "طبق جانبي", "/images/meal.png", 10 },
                    { 11, 350, 52, "أم علي، حلوى مصرية شهيرة مصنوعة من العجينة الهشة والمكسرات والحليب والقشطة.", 15, "الغداء", "أم علي", "حلوى", "/images/meal.png", 8 },
                    { 12, 90, 12, "لبن عيران منعش، مشروب تقليدي من اللبن المخفوق مع الماء والنعناع والملح.", 5, "الغداء", "لبن عيران", "مشروب", "/images/meal.png", 8 },
                    { 13, 380, 40, "شاورما دجاج عربية تقليدية، مشوية على السيخ ومقدمة مع الخبز العربي والطحينة والخضروات.", 20, "العشاء", "شاورما دجاج", "الطبق الرئيسي", "/images/meal.png", 25 },
                    { 14, 420, 35, "برجر لحم محضر من اللحم البقري الطازج 100% مع الخضروات والصلصة الخاصة، يقدم مع خبز البرجر المحمص.", 25, "العشاء", "برجر لحم", "الطبق الرئيسي", "/images/meal.png", 28 },
                    { 15, 365, 48, "بطاطس مقلية مقرمشة من الخارج وطرية من الداخل، تقدم مع الكاتشب والمايونيز.", 18, "العشاء", "بطاطس مقلية", "طبق جانبي", "/images/meal.png", 4 },
                    { 16, 150, 15, "سلطة سيزر كلاسيكية مع خس رومين، جبن البارميزان، قطع خبز محمصة، وصلصة سيزر المميزة.", 10, "العشاء", "سلطة سيزر", "طبق جانبي", "/images/meal.png", 8 },
                    { 17, 400, 58, "كنافة عربية تقليدية محشوة بالجبنة الحلوة ومغطاة بالقطر ومزينة بالفستق الحلبي.", 20, "العشاء", "كنافة", "حلوى", "/images/meal.png", 6 },
                    { 18, 80, 20, "عصير ليمون طازج ومنعش مع النعناع، مثالي لتنشيط الجسم وتعزيز المناعة.", 0, "العشاء", "عصير ليمون بالنعناع", "مشروب", "/images/meal.png", 1 },
                    { 19, 120, 28, "عصير برتقال طازج معصور من أجود أنواع البرتقال، غني بالفيتامينات والمعادن.", 0, "الإفطار", "عصير برتقال طازج", "مشروب", "/images/meal.png", 1 },
                    { 20, 420, 63, "كعكة الشوكولاتة الغنية بطبقات الشوكولاتة الداكنة والكريمة، مثالية لمحبي الحلويات.", 22, "العشاء", "كعكة الشوكولاتة", "حلوى", "/images/meal.png", 5 }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "MenuID", "Day" },
                values: new object[,]
                {
                    { 1, "الأحد" },
                    { 2, "الإثنين" },
                    { 3, "الثلاثاء" },
                    { 4, "الأربعاء" },
                    { 5, "الخميس" },
                    { 6, "الجمعة" },
                    { 7, "السبت" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantID", "Address", "BreakfastCloseTime", "BreakfastOpenTime", "DaysOpen", "Description", "DinnerCloseTime", "DinnerOpenTime", "Email", "LocationUrl", "LunchCloseTime", "LunchOpenTime", "Name", "PhoneNumber", "PhotoPath", "ServesBreakfast", "ServesDinner", "ServesLunch" },
                values: new object[] { 1, "جامعة الملك عبد العزيز، جدة، المملكة العربية السعودية", new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), "من الأحد إلى الخميس", "في المطعم الجامعي الرسمي لجامعة الملك عبد العزيز، نقدم وجبات طازجة وعالية الجودة للطلاب وأعضاء هيئة التدريس. نحرص على تقديم أطباق متنوعة ومغذية في بيئة نظيفة ومرحبة.", new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), "restaurant@kau.edu.sa", "https://maps.app.goo.gl/KFBdpmH7E88Lzvy49", new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0), "مطعم جامعة الملك عبدالعزيز", "+9665********", "/images/restaurant.png", true, true, true });

            migrationBuilder.InsertData(
                table: "SocialMedia",
                columns: new[] { "SocialMediaID", "DisplayOrder", "Icon", "IsActive", "Link", "Name" },
                values: new object[,]
                {
                    { 1, 1, "bi-twitter", true, "https://x.com/kauedu_sa", "تويتر" },
                    { 2, 2, "bi-instagram", true, "https://www.instagram.com/kauedu_sa/", "انستغرام" },
                    { 3, 3, "bi-youtube", true, "https://www.youtube.com/@kauedu_sa", "يوتيوب" },
                    { 4, 4, "bi-linkedin", true, "https://www.linkedin.com/school/king-abdulaziz-university/posts/?feedView=all", "لينكد إن" }
                });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "TermID", "Content", "DisplayOrder", "LastUpdated", "Title" },
                values: new object[,]
                {
                    { 1, "تنطبق هذه الشروط والأحكام على جميع الخدمات المقدمة من مطعم جامعة الملك عبدالعزيز.", 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "عام" },
                    { 2, "جميع المبيعات نهائية ولا يمكن استرداد قيمة التذاكر بعد الشراء. يجب استخدام التذاكر خلال الفصل الدراسي الذي تم شراؤها فيه. التذاكر غير قابلة للتحويل ويجب استخدامها من قبل مالك الحساب فقط.", 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "التذاكر والدفع" },
                    { 3, "يتعهد المستخدم بعدم إساءة استخدام الخدمة أو محاولة التحايل على النظام. يحتفظ المطعم بالحق في رفض الخدمة لأي شخص ينتهك هذه الشروط.", 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "الاستخدام" },
                    { 4, "نحن نحترم خصوصيتك ونلتزم بحماية بياناتك الشخصية وفقاً لسياسة الخصوصية الخاصة بنا.", 4, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "الخصوصية" },
                    { 5, "يحتفظ المطعم بالحق في تعديل هذه الشروط في أي وقت. سيتم نشر التغييرات على موقعنا.", 5, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "التغييرات على الشروط" }
                });

            migrationBuilder.InsertData(
                table: "MenuMeals",
                columns: new[] { "MenuMealID", "MealID", "MenuID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 5, 1 },
                    { 6, 6, 1 },
                    { 7, 7, 1 },
                    { 8, 8, 1 },
                    { 9, 9, 1 },
                    { 10, 10, 1 },
                    { 11, 11, 1 },
                    { 12, 12, 1 },
                    { 13, 13, 1 },
                    { 14, 14, 1 },
                    { 15, 15, 1 },
                    { 16, 16, 1 },
                    { 17, 17, 1 },
                    { 18, 18, 1 },
                    { 19, 19, 2 },
                    { 20, 1, 2 },
                    { 21, 7, 2 },
                    { 22, 13, 2 },
                    { 23, 20, 3 },
                    { 24, 2, 3 },
                    { 25, 8, 3 },
                    { 26, 14, 3 },
                    { 27, 3, 4 },
                    { 28, 9, 4 },
                    { 29, 15, 4 },
                    { 30, 19, 4 },
                    { 31, 4, 5 },
                    { 32, 10, 5 },
                    { 33, 16, 5 },
                    { 34, 20, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserID",
                table: "Feedbacks",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuMeals_MealID",
                table: "MenuMeals",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuMeals_MenuID",
                table: "MenuMeals",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerID",
                table: "Reviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MealID",
                table: "Reviews",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OrderID",
                table: "Tickets",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "MenuMeals");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SocialMedia");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
