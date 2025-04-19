using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class addOpenStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantID",
                keyValue: 1,
                column: "IsOpen",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Restaurants");
        }
    }
}
