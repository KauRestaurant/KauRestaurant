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
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
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
                name: "TicketPrices",
                columns: table => new
                {
                    TicketPriceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPrices", x => x.TicketPriceID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    LunchTicketsCount = table.Column<int>(type: "int", nullable: false)
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
                table: "Meals",
                columns: new[] { "MealID", "Calories", "Carbs", "Description", "Fat", "MealCategory", "MealName", "MealType", "PicturePath", "Protein" },
                values: new object[,]
                {
                    { 1, 268, 30, "فطائر طازجة محشوة بالجبنة المشكلة المذابة، تقدم ساخنة مع زيت الزيتون والزعتر.", 12, "الإفطار", "فطائر بالجبن", "الطبق الرئيسي", "/images/meal.png", 10 },
                    { 2, 182, 2, "بيض مقلي على الطريقة العربية، يقدم مع الخبز العربي والخضروات الطازجة.", 14, "الإفطار", "بيض مقلي", "الطبق الرئيسي", "/images/meal.png", 12 },
                    { 3, 245, 35, "فول مدمس مطبوخ بالطريقة التقليدية مع زيت الزيتون والثوم والليمون، غني بالبروتين النباتي.", 5, "الإفطار", "فول مدمس", "طبق جانبي", "/images/meal.png", 15 },
                    { 4, 213, 25, "حمص بالطحينة متبل بزيت الزيتون والليمون، مزين بحبات الحمص الكاملة والبقدونس.", 9, "الإفطار", "حمص بالطحينة", "طبق جانبي", "/images/meal.png", 8 },
                    { 5, 4, 1, "شاي عربي معطر بالهيل والزعفران، يقدم ساخناً في أكواب تقليدية.", 0, "الإفطار", "شاي عربي", "مشروب", "/images/meal.png", 0 },
                    { 6, 116, 28, "عصير برتقال طازج معصور في المطعم، غني بفيتامين سي والمذاق المنعش.", 0, "الإفطار", "عصير برتقال طازج", "مشروب", "/images/meal.png", 1 },
                    { 7, 256, 40, "مناقيش زعتر تقليدية مخبوزة في فرن طيني، مع مزيج الزعتر والسماق وزيت الزيتون.", 8, "الإفطار", "مناقيش زعتر", "الطبق الرئيسي", "/images/meal.png", 6 },
                    { 8, 235, 15, "لبنة طازجة تقدم مع زيت الزيتون والزعتر والخبز العربي الطازج.", 15, "الإفطار", "لبنة بالزيت", "طبق جانبي", "/images/meal.png", 10 },
                    { 9, 204, 10, "بيض مطبوخ في صلصة طماطم غنية بالتوابل والخضروات، طبق عربي شهير.", 12, "الإفطار", "شكشوكة", "الطبق الرئيسي", "/images/meal.png", 14 },
                    { 10, 286, 45, "فطيرة تفاح محلية الصنع مع القرفة والعسل، مخبوزة حتى ذهبية اللون.", 10, "الإفطار", "فطيرة التفاح", "حلوى", "/images/meal.png", 4 },
                    { 11, 100, 25, "عصير تفاح طازج مصنوع من تفاح موسمي، غني بالفيتامينات ومنعش المذاق.", 0, "الإفطار", "عصير تفاح", "مشروب", "/images/meal.png", 0 },
                    { 12, 323, 60, "عصيدة تقليدية محلاة بالتمر والعسل، مزينة بالمكسرات المحمصة.", 7, "الإفطار", "عصيدة بالتمر", "حلوى", "/images/meal.png", 5 },
                    { 13, 343, 30, "خبز طابون مغطى بالبصل المكرمل والسماق مع قطع الدجاج المشوي.", 15, "الإفطار", "مسخن دجاج", "الطبق الرئيسي", "/images/meal.png", 22 },
                    { 14, 177, 25, "زبادي طبيعي محلى بالعسل ومزين بالمكسرات والفواكه الطازجة.", 5, "الإفطار", "زبادي بالعسل", "طبق جانبي", "/images/meal.png", 8 },
                    { 15, 326, 35, "كرواسون طازج محشو بالشوكولاتة الغنية، يقدم دافئاً.", 18, "الإفطار", "كرواسون بالشوكولاتة", "حلوى", "/images/meal.png", 6 },
                    { 16, 467, 55, "كبسة لحم سعودية تقليدية، مطبوخة ببهارات الكبسة المميزة مع قطع اللحم الطرية والأرز البسمتي.", 15, "الغداء", "كبسة لحم", "الطبق الرئيسي", "/images/meal.png", 28 },
                    { 17, 210, 0, "دجاج مشوي متبل بالأعشاب والبهارات العربية، مشوي على الفحم ليكتسب نكهة مميزة.", 10, "الغداء", "دجاج مشوي", "الطبق الرئيسي", "/images/meal.png", 30 },
                    { 18, 105, 12, "سلطة خضراء منعشة مع خضروات موسمية طازجة وتتبيلة خاصة بالمطعم.", 5, "الغداء", "سلطة خضراء", "طبق جانبي", "/images/meal.png", 3 },
                    { 19, 205, 30, "شوربة عدس تقليدية، مطبوخة بالطريقة العربية مع البهارات والليمون وزيت الزيتون.", 5, "الغداء", "شوربة عدس", "طبق جانبي", "/images/meal.png", 10 },
                    { 20, 375, 52, "أم علي، حلوى مصرية شهيرة مصنوعة من العجينة الهشة والمكسرات والحليب والقشطة.", 15, "الغداء", "أم علي", "حلوى", "/images/meal.png", 8 },
                    { 21, 98, 12, "لبن عيران منعش، مشروب تقليدي من اللبن المخفوق مع الماء والنعناع والملح.", 2, "الغداء", "لبن عيران", "مشروب", "/images/meal.png", 8 },
                    { 22, 448, 60, "برياني دجاج هندي تقليدي مع الأرز البسمتي والبهارات الهندية والخضروات.", 12, "الغداء", "برياني دجاج", "الطبق الرئيسي", "/images/meal.png", 25 },
                    { 23, 482, 50, "مندي لحم يمني تقليدي مع الأرز المطبوخ على الفحم وبهارات المندي الخاصة.", 18, "الغداء", "مندي لحم", "الطبق الرئيسي", "/images/meal.png", 30 },
                    { 24, 382, 45, "مجبوس سمك خليجي تقليدي مع الأرز والبهارات المميزة وقطع السمك الطازج.", 10, "الغداء", "مجبوس سمك", "الطبق الرئيسي", "/images/meal.png", 28 },
                    { 25, 235, 35, "طاجين خضار مغربي تقليدي مع البهارات المغربية والخضروات الموسمية.", 7, "الغداء", "طاجين خضار", "طبق جانبي", "/images/meal.png", 8 },
                    { 26, 172, 20, "سلطة فتوش لبنانية مع الخضروات الطازجة وقطع الخبز المحمص والسماق.", 8, "الغداء", "سلطة فتوش", "طبق جانبي", "/images/meal.png", 5 },
                    { 27, 505, 60, "كنافة عربية تقليدية محشوة بالجبنة الحلوة ومغطاة بالقطر ومزينة بالفستق الحلبي.", 25, "الغداء", "كنافة", "حلوى", "/images/meal.png", 10 },
                    { 28, 84, 20, "عصير ليمون طازج ومنعش مع النعناع، مثالي لتنشيط الجسم وتعزيز المناعة.", 0, "الغداء", "عصير ليمون بالنعناع", "مشروب", "/images/meal.png", 1 },
                    { 29, 415, 65, "بسبوسة تقليدية مصنوعة من السميد والمغطاة بشراب السكر، مزينة بالمكسرات.", 15, "الغداء", "بسبوسة", "حلوى", "/images/meal.png", 5 },
                    { 30, 8, 2, "شاي أخضر محضَّر طازجًا ومليء بمضادات الأكسدة، يُقدَّم ساخنًا أو باردًا مع لمسة نعناع.", 0, "الغداء", "شاي أخضر", "مشروب", "/images/meal.png", 0 }
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
                table: "MenuMeals",
                columns: new[] { "MenuMealID", "MealID", "MenuID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 10, 1 },
                    { 6, 15, 1 },
                    { 7, 5, 1 },
                    { 8, 6, 1 },
                    { 9, 16, 1 },
                    { 10, 17, 1 },
                    { 11, 18, 1 },
                    { 12, 19, 1 },
                    { 13, 20, 1 },
                    { 14, 27, 1 },
                    { 15, 21, 1 },
                    { 16, 28, 1 },
                    { 17, 7, 2 },
                    { 18, 9, 2 },
                    { 19, 8, 2 },
                    { 20, 14, 2 },
                    { 21, 12, 2 },
                    { 22, 15, 2 },
                    { 23, 5, 2 },
                    { 24, 11, 2 },
                    { 25, 22, 2 },
                    { 26, 24, 2 },
                    { 27, 25, 2 },
                    { 28, 26, 2 },
                    { 29, 29, 2 },
                    { 30, 20, 2 },
                    { 31, 21, 2 },
                    { 32, 30, 2 },
                    { 33, 1, 3 },
                    { 34, 7, 3 },
                    { 35, 3, 3 },
                    { 36, 8, 3 },
                    { 37, 10, 3 },
                    { 38, 12, 3 },
                    { 39, 6, 3 },
                    { 40, 11, 3 },
                    { 41, 23, 3 },
                    { 42, 17, 3 },
                    { 43, 18, 3 },
                    { 44, 25, 3 },
                    { 45, 27, 3 },
                    { 46, 29, 3 },
                    { 47, 28, 3 },
                    { 48, 30, 3 },
                    { 49, 2, 4 },
                    { 50, 9, 4 },
                    { 51, 4, 4 },
                    { 52, 14, 4 },
                    { 53, 15, 4 },
                    { 54, 10, 4 },
                    { 55, 5, 4 },
                    { 56, 6, 4 },
                    { 57, 16, 4 },
                    { 58, 22, 4 },
                    { 59, 19, 4 },
                    { 60, 26, 4 },
                    { 61, 20, 4 },
                    { 62, 27, 4 },
                    { 63, 21, 4 },
                    { 64, 28, 4 },
                    { 65, 13, 5 },
                    { 66, 7, 5 },
                    { 67, 3, 5 },
                    { 68, 8, 5 },
                    { 69, 12, 5 },
                    { 70, 15, 5 },
                    { 71, 11, 5 },
                    { 72, 5, 5 },
                    { 73, 24, 5 },
                    { 74, 23, 5 },
                    { 75, 18, 5 },
                    { 76, 25, 5 },
                    { 77, 29, 5 },
                    { 78, 20, 5 },
                    { 79, 30, 5 },
                    { 80, 21, 5 },
                    { 81, 1, 6 },
                    { 82, 2, 6 },
                    { 83, 4, 6 },
                    { 84, 14, 6 },
                    { 85, 10, 6 },
                    { 86, 12, 6 },
                    { 87, 5, 6 },
                    { 88, 6, 6 },
                    { 89, 17, 6 },
                    { 90, 22, 6 },
                    { 91, 19, 6 },
                    { 92, 26, 6 },
                    { 93, 27, 6 },
                    { 94, 29, 6 },
                    { 95, 28, 6 },
                    { 96, 30, 6 },
                    { 97, 9, 7 },
                    { 98, 13, 7 },
                    { 99, 3, 7 },
                    { 100, 8, 7 },
                    { 101, 15, 7 },
                    { 102, 10, 7 },
                    { 103, 11, 7 },
                    { 104, 5, 7 },
                    { 105, 23, 7 },
                    { 106, 24, 7 },
                    { 108, 25, 7 },
                    { 109, 20, 7 },
                    { 110, 27, 7 },
                    { 111, 21, 7 },
                    { 112, 28, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "MenuMeals");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "TicketPrices");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
