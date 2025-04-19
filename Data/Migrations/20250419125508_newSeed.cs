using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class newSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 1,
                columns: new[] { "FridayBreakfastCloseTime", "FridayBreakfastOpenTime", "FridayDinnerCloseTime", "FridayDinnerOpenTime", "FridayLunchCloseTime", "FridayLunchOpenTime", "FridayServesBreakfast", "FridayServesDinner", "FridayServesLunch", "IsFridayOpen", "IsSaturdayOpen", "SaturdayBreakfastCloseTime", "SaturdayBreakfastOpenTime", "SaturdayDinnerCloseTime", "SaturdayDinnerOpenTime", "SaturdayLunchCloseTime", "SaturdayLunchOpenTime", "SaturdayServesBreakfast", "SaturdayServesDinner", "SaturdayServesLunch" },
                values: new object[] { new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), true, true, true, true, true, new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), true, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 1,
                columns: new[] { "FridayBreakfastCloseTime", "FridayBreakfastOpenTime", "FridayDinnerCloseTime", "FridayDinnerOpenTime", "FridayLunchCloseTime", "FridayLunchOpenTime", "FridayServesBreakfast", "FridayServesDinner", "FridayServesLunch", "IsFridayOpen", "IsSaturdayOpen", "SaturdayBreakfastCloseTime", "SaturdayBreakfastOpenTime", "SaturdayDinnerCloseTime", "SaturdayDinnerOpenTime", "SaturdayLunchCloseTime", "SaturdayLunchOpenTime", "SaturdayServesBreakfast", "SaturdayServesDinner", "SaturdayServesLunch" },
                values: new object[] { null, null, null, null, null, null, false, false, false, false, false, null, null, null, null, null, null, false, false, false });
        }
    }
}
