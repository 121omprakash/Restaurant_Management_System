using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class Table_Data_Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RestaurantTables",
                columns: new[] { "TableNumber", "IsOccupied" },
                values: new object[,]
                {
                    { "T01", false },
                    { "T02", false },
                    { "T03", false },
                    { "T04", false },
                    { "T05", false },
                    { "T06", false },
                    { "T07", true },
                    { "T08", true },
                    { "T09", true },
                    { "T10", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T01");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T02");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T03");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T04");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T05");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T06");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T07");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T08");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T09");

            migrationBuilder.DeleteData(
                table: "RestaurantTables",
                keyColumn: "TableNumber",
                keyValue: "T10");
        }
    }
}
