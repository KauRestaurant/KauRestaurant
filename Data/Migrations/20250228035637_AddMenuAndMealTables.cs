using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuAndMealTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Meals",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuID = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Meals_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Cascade);
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
                    { 5, "الخميس" }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "MealID", "Calories", "Carbs", "Fat", "MealCategory", "MealName", "MealType", "MenuID", "Protein" },
                values: new object[,]
                {
                    { 1, 250, 45, 10, "الإفطار", "فطائر بالجبن", "الطبق الرئيسي", 1, 8 },
                    { 2, 185, 2, 14, "الإفطار", "بيض مقلي", "الطبق الرئيسي", 1, 12 },
                    { 3, 220, 35, 5, "الإفطار", "فول مدمس", "طبق جانبي", 1, 15 },
                    { 4, 180, 25, 9, "الإفطار", "حمص بالطحينة", "طبق جانبي", 1, 8 },
                    { 5, 5, 1, 0, "الإفطار", "شاي عربي", "مشروب", 1, 0 },
                    { 6, 120, 28, 0, "الإفطار", "عصير برتقال طازج", "مشروب", 1, 1 },
                    { 7, 450, 55, 15, "الغداء", "كبسة لحم", "الطبق الرئيسي", 1, 28 },
                    { 8, 350, 0, 20, "الغداء", "دجاج مشوي", "الطبق الرئيسي", 1, 30 },
                    { 9, 65, 12, 2, "الغداء", "سلطة خضراء", "طبق جانبي", 1, 3 },
                    { 10, 180, 30, 5, "الغداء", "شوربة عدس", "طبق جانبي", 1, 10 },
                    { 11, 350, 52, 15, "الغداء", "أم علي", "حلوى", 1, 8 },
                    { 12, 90, 12, 5, "الغداء", "لبن عيران", "مشروب", 1, 8 },
                    { 13, 380, 40, 20, "العشاء", "شاورما دجاج", "الطبق الرئيسي", 1, 25 },
                    { 14, 420, 35, 25, "العشاء", "برجر لحم", "الطبق الرئيسي", 1, 28 },
                    { 15, 365, 48, 18, "العشاء", "بطاطس مقلية", "طبق جانبي", 1, 4 },
                    { 16, 150, 15, 10, "العشاء", "سلطة سيزر", "طبق جانبي", 1, 8 },
                    { 17, 400, 58, 20, "العشاء", "كنافة", "حلوى", 1, 6 },
                    { 18, 80, 20, 0, "العشاء", "عصير ليمون بالنعناع", "مشروب", 1, 1 },
                    { 19, 120, 28, 0, "الإفطار", "عصير برتقال طازج", "مشروب", 2, 1 },
                    { 20, 420, 63, 22, "العشاء", "كعكة الشوكولاتة", "حلوى", 3, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_MenuID",
                table: "Meals",
                column: "MenuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Menus");
        }
    }
}
