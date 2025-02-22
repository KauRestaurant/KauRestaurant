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
                    Day = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
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
                    NutritionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealCategory = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
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
                    { 1, "SAT" },
                    { 2, "SUN" },
                    { 3, "MON" },
                    { 4, "TUE" },
                    { 5, "THU" }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "MealID", "MealCategory", "MealName", "MenuID", "NutritionalInfo" },
                values: new object[,]
                {
                    { 1, "Breakfast", "Pancakes", 1, "{\"calories\": 250, \"protein\": 8, \"carbs\": 45}" },
                    { 2, "Lunch", "Grilled Chicken", 1, "{\"calories\": 350, \"protein\": 30, \"carbs\": 0}" }
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
