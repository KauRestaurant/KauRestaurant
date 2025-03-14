using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuMealsAndReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Reviews table
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

            // 2. Create MenuMeals table
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

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerID",
                table: "Reviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MealID",
                table: "Reviews",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuMeals_MealID",
                table: "MenuMeals",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuMeals_MenuID",
                table: "MenuMeals",
                column: "MenuID");

            // Seed MenuMeals junction table data
            migrationBuilder.InsertData(
                table: "MenuMeals",
                columns: new[] { "MenuMealID", "MenuID", "MealID" },
                values: new object[,]
                {
                    // Sunday (الأحد) - MenuID = 1
                    { 1, 1, 1 },  // Breakfast meals
                    { 2, 1, 2 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 5, 1, 5 },
                    { 6, 1, 6 },
                    { 7, 1, 7 },  // Lunch meals
                    { 8, 1, 8 },
                    { 9, 1, 9 },
                    { 10, 1, 10 },
                    { 11, 1, 11 },
                    { 12, 1, 12 },
                    { 13, 1, 13 }, // Dinner meals
                    { 14, 1, 14 },
                    { 15, 1, 15 },
                    { 16, 1, 16 },
                    { 17, 1, 17 },
                    { 18, 1, 18 },

                    // Monday (الإثنين) - MenuID = 2
                    { 19, 2, 2 },  // Breakfast
                    { 20, 2, 3 },
                    { 21, 2, 5 },
                    { 22, 2, 6 },
                    { 23, 2, 8 },  // Lunch
                    { 24, 2, 9 },
                    { 25, 2, 10 },
                    { 26, 2, 12 },
                    { 27, 2, 14 }, // Dinner
                    { 28, 2, 15 },
                    { 29, 2, 16 },
                    { 30, 2, 18 },

                    // Tuesday (الثلاثاء) - MenuID = 3
                    { 31, 3, 1 },  // Breakfast
                    { 32, 3, 4 },
                    { 33, 3, 5 },
                    { 34, 3, 19 },
                    { 35, 3, 7 },  // Lunch
                    { 36, 3, 9 },
                    { 37, 3, 11 },
                    { 38, 3, 12 },
                    { 39, 3, 13 }, // Dinner
                    { 40, 3, 15 },
                    { 41, 3, 17 },
                    { 42, 3, 18 },

                    // Wednesday (الأربعاء) - MenuID = 4
                    { 43, 4, 2 },  // Breakfast
                    { 44, 4, 3 },
                    { 45, 4, 5 },
                    { 46, 4, 6 },
                    { 47, 4, 8 },  // Lunch
                    { 48, 4, 10 },
                    { 49, 4, 11 },
                    { 50, 4, 12 },
                    { 51, 4, 14 }, // Dinner
                    { 52, 4, 16 },
                    { 53, 4, 20 },
                    { 54, 4, 18 },

                    // Thursday (الخميس) - MenuID = 5
                    { 55, 5, 1 },  // Breakfast
                    { 56, 5, 4 },
                    { 57, 5, 5 },
                    { 58, 5, 19 },
                    { 59, 5, 7 },  // Lunch
                    { 60, 5, 9 },
                    { 61, 5, 10 },
                    { 62, 5, 12 },
                    { 63, 5, 13 }, // Dinner
                    { 64, 5, 15 },
                    { 65, 5, 17 },
                    { 66, 5, 18 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MenuMeals");
            migrationBuilder.DropTable(name: "Reviews");
        }
    }
}
