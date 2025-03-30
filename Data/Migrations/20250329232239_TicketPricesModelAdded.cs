using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KauRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class TicketPricesModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialMedia");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketPrices");

            migrationBuilder.CreateTable(
                name: "SocialMedia",
                columns: table => new
                {
                    SocialMediaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedia", x => x.SocialMediaID);
                });

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
        }
    }
}
