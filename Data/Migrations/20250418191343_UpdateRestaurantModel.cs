using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRestaurantModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOpen",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "ServesLunch",
                table: "Restaurants",
                newName: "WednesdayServesLunch");

            migrationBuilder.RenameColumn(
                name: "ServesDinner",
                table: "Restaurants",
                newName: "WednesdayServesDinner");

            migrationBuilder.RenameColumn(
                name: "ServesBreakfast",
                table: "Restaurants",
                newName: "WednesdayServesBreakfast");

            migrationBuilder.RenameColumn(
                name: "LunchOpenTime",
                table: "Restaurants",
                newName: "WednesdayLunchOpenTime");

            migrationBuilder.RenameColumn(
                name: "LunchCloseTime",
                table: "Restaurants",
                newName: "WednesdayLunchCloseTime");

            migrationBuilder.RenameColumn(
                name: "IsOpen",
                table: "Restaurants",
                newName: "TuesdayServesLunch");

            migrationBuilder.RenameColumn(
                name: "DinnerOpenTime",
                table: "Restaurants",
                newName: "WednesdayDinnerOpenTime");

            migrationBuilder.RenameColumn(
                name: "DinnerCloseTime",
                table: "Restaurants",
                newName: "WednesdayDinnerCloseTime");

            migrationBuilder.RenameColumn(
                name: "BreakfastOpenTime",
                table: "Restaurants",
                newName: "WednesdayBreakfastOpenTime");

            migrationBuilder.RenameColumn(
                name: "BreakfastCloseTime",
                table: "Restaurants",
                newName: "WednesdayBreakfastCloseTime");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayBreakfastCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayBreakfastOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayDinnerCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayDinnerOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayLunchCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayLunchOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FridayServesBreakfast",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FridayServesDinner",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FridayServesLunch",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFridayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMondayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSaturdayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSundayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsThursdayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTuesdayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWednesdayOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayBreakfastCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayBreakfastOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayDinnerCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayDinnerOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayLunchCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayLunchOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MondayServesBreakfast",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MondayServesDinner",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MondayServesLunch",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SaturdayBreakfastCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SaturdayBreakfastOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SaturdayDinnerCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SaturdayDinnerOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SaturdayLunchCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SaturdayLunchOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SaturdayServesBreakfast",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SaturdayServesDinner",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SaturdayServesLunch",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SundayBreakfastCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SundayBreakfastOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SundayDinnerCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SundayDinnerOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SundayLunchCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SundayLunchOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SundayServesBreakfast",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SundayServesDinner",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SundayServesLunch",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayBreakfastCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayBreakfastOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayDinnerCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayDinnerOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayLunchCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayLunchOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ThursdayServesBreakfast",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThursdayServesDinner",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThursdayServesLunch",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayBreakfastCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayBreakfastOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayDinnerCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayDinnerOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayLunchCloseTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayLunchOpenTime",
                table: "Restaurants",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TuesdayServesBreakfast",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TuesdayServesDinner",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 1,
                columns: new[] { "FridayBreakfastCloseTime", "FridayBreakfastOpenTime", "FridayDinnerCloseTime", "FridayDinnerOpenTime", "FridayLunchCloseTime", "FridayLunchOpenTime", "FridayServesBreakfast", "FridayServesDinner", "FridayServesLunch", "IsFridayOpen", "IsMondayOpen", "IsSaturdayOpen", "IsSundayOpen", "IsThursdayOpen", "IsTuesdayOpen", "IsWednesdayOpen", "MondayBreakfastCloseTime", "MondayBreakfastOpenTime", "MondayDinnerCloseTime", "MondayDinnerOpenTime", "MondayLunchCloseTime", "MondayLunchOpenTime", "MondayServesBreakfast", "MondayServesDinner", "MondayServesLunch", "SaturdayBreakfastCloseTime", "SaturdayBreakfastOpenTime", "SaturdayDinnerCloseTime", "SaturdayDinnerOpenTime", "SaturdayLunchCloseTime", "SaturdayLunchOpenTime", "SaturdayServesBreakfast", "SaturdayServesDinner", "SaturdayServesLunch", "SundayBreakfastCloseTime", "SundayBreakfastOpenTime", "SundayDinnerCloseTime", "SundayDinnerOpenTime", "SundayLunchCloseTime", "SundayLunchOpenTime", "SundayServesBreakfast", "SundayServesDinner", "SundayServesLunch", "ThursdayBreakfastCloseTime", "ThursdayBreakfastOpenTime", "ThursdayDinnerCloseTime", "ThursdayDinnerOpenTime", "ThursdayLunchCloseTime", "ThursdayLunchOpenTime", "ThursdayServesBreakfast", "ThursdayServesDinner", "ThursdayServesLunch", "TuesdayBreakfastCloseTime", "TuesdayBreakfastOpenTime", "TuesdayDinnerCloseTime", "TuesdayDinnerOpenTime", "TuesdayLunchCloseTime", "TuesdayLunchOpenTime", "TuesdayServesBreakfast", "TuesdayServesDinner" },
                values: new object[] { null, null, null, null, null, null, false, false, false, false, true, false, true, true, true, true, new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0), true, true, true, null, null, null, null, null, null, false, false, false, new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0), true, true, true, new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0), true, true, true, new TimeSpan(0, 10, 30, 0, 0), new TimeSpan(0, 7, 0, 0, 0), new TimeSpan(0, 22, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0), true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FridayBreakfastCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayBreakfastOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayDinnerCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayDinnerOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayLunchCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayLunchOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayServesBreakfast",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayServesDinner",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "FridayServesLunch",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsFridayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsMondayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsSaturdayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsSundayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsThursdayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsTuesdayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsWednesdayOpen",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayBreakfastCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayBreakfastOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayDinnerCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayDinnerOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayLunchCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayLunchOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayServesBreakfast",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayServesDinner",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MondayServesLunch",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayBreakfastCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayBreakfastOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayDinnerCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayDinnerOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayLunchCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayLunchOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayServesBreakfast",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayServesDinner",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SaturdayServesLunch",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayBreakfastCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayBreakfastOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayDinnerCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayDinnerOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayLunchCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayLunchOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayServesBreakfast",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayServesDinner",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SundayServesLunch",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayBreakfastCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayBreakfastOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayDinnerCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayDinnerOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayLunchCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayLunchOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayServesBreakfast",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayServesDinner",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ThursdayServesLunch",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayBreakfastCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayBreakfastOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayDinnerCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayDinnerOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayLunchCloseTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayLunchOpenTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayServesBreakfast",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TuesdayServesDinner",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "WednesdayServesLunch",
                table: "Restaurants",
                newName: "ServesLunch");

            migrationBuilder.RenameColumn(
                name: "WednesdayServesDinner",
                table: "Restaurants",
                newName: "ServesDinner");

            migrationBuilder.RenameColumn(
                name: "WednesdayServesBreakfast",
                table: "Restaurants",
                newName: "ServesBreakfast");

            migrationBuilder.RenameColumn(
                name: "WednesdayLunchOpenTime",
                table: "Restaurants",
                newName: "LunchOpenTime");

            migrationBuilder.RenameColumn(
                name: "WednesdayLunchCloseTime",
                table: "Restaurants",
                newName: "LunchCloseTime");

            migrationBuilder.RenameColumn(
                name: "WednesdayDinnerOpenTime",
                table: "Restaurants",
                newName: "DinnerOpenTime");

            migrationBuilder.RenameColumn(
                name: "WednesdayDinnerCloseTime",
                table: "Restaurants",
                newName: "DinnerCloseTime");

            migrationBuilder.RenameColumn(
                name: "WednesdayBreakfastOpenTime",
                table: "Restaurants",
                newName: "BreakfastOpenTime");

            migrationBuilder.RenameColumn(
                name: "WednesdayBreakfastCloseTime",
                table: "Restaurants",
                newName: "BreakfastCloseTime");

            migrationBuilder.RenameColumn(
                name: "TuesdayServesLunch",
                table: "Restaurants",
                newName: "IsOpen");

            migrationBuilder.AddColumn<string>(
                name: "DaysOpen",
                table: "Restaurants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 1,
                column: "DaysOpen",
                value: "من الأحد إلى الخميس");
        }
    }
}
