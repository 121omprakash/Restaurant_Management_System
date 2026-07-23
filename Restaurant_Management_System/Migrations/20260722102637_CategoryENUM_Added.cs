using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class CategoryENUM_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1,
                column: "Category",
                value: "NonVeg");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2,
                column: "Category",
                value: "Veg");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3,
                column: "Category",
                value: "Veg");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4,
                column: "Category",
                value: "Veg");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6,
                column: "Category",
                value: "Veg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1,
                column: "Category",
                value: "Pizza");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2,
                column: "Category",
                value: "Pizza");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3,
                column: "Category",
                value: "Pizza");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4,
                column: "Category",
                value: "Sides");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6,
                column: "Category",
                value: "Sides");
        }
    }
}
