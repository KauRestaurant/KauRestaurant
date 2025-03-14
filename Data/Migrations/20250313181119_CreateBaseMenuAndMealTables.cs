using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateBaseMenuAndMealTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Menus table
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

            // 2. Create Meals table
            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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

            // Seed Menus data
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "MenuID", "Day" },
                values: new object[,]
                {
                    { 1, "الأحد" },
                    { 2, "الإثنين" },
                    { 3, "الثلاثاء" },
                    { 4, "الأربعاء" },
                    { 5, "الخميس" }
                });

            // Seed Meals data
            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "MealID", "MealName", "Calories", "Protein", "Carbs", "Fat", "MealCategory", "MealType" },
                values: new object[,]
                {
                    { 1, "فطائر بالجبن", 250, 8, 45, 10, "الإفطار", "الطبق الرئيسي" },
                    { 2, "بيض مقلي", 185, 12, 2, 14, "الإفطار", "الطبق الرئيسي" },
                    { 3, "فول مدمس", 220, 15, 35, 5, "الإفطار", "طبق جانبي" },
                    { 4, "حمص بالطحينة", 180, 8, 25, 9, "الإفطار", "طبق جانبي" },
                    { 5, "شاي عربي", 5, 0, 1, 0, "الإفطار", "مشروب" },
                    { 6, "عصير برتقال طازج", 120, 1, 28, 0, "الإفطار", "مشروب" },
                    { 7, "كبسة لحم", 450, 28, 55, 15, "الغداء", "الطبق الرئيسي" },
                    { 8, "دجاج مشوي", 350, 30, 0, 20, "الغداء", "الطبق الرئيسي" },
                    { 9, "سلطة خضراء", 65, 3, 12, 2, "الغداء", "طبق جانبي" },
                    { 10, "شوربة عدس", 180, 10, 30, 5, "الغداء", "طبق جانبي" },
                    { 11, "أم علي", 350, 8, 52, 15, "الغداء", "حلوى" },
                    { 12, "لبن عيران", 90, 8, 12, 5, "الغداء", "مشروب" },
                    { 13, "شاورما دجاج", 380, 25, 40, 20, "العشاء", "الطبق الرئيسي" },
                    { 14, "برجر لحم", 420, 28, 35, 25, "العشاء", "الطبق الرئيسي" },
                    { 15, "بطاطس مقلية", 365, 4, 48, 18, "العشاء", "طبق جانبي" },
                    { 16, "سلطة سيزر", 150, 8, 15, 10, "العشاء", "طبق جانبي" },
                    { 17, "كنافة", 400, 6, 58, 20, "العشاء", "حلوى" },
                    { 18, "عصير ليمون بالنعناع", 80, 1, 20, 0, "العشاء", "مشروب" },
                    { 19, "عصير برتقال طازج", 120, 1, 28, 0, "الإفطار", "مشروب" },
                    { 20, "كعكة الشوكولاتة", 420, 5, 63, 22, "العشاء", "حلوى" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Meals");
            migrationBuilder.DropTable(name: "Menus");
        }
    }
}
