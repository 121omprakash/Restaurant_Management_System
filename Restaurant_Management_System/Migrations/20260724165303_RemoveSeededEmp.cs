using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeededEmp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "EmpId", "IsActive", "Name", "Role", "password" },
                values: new object[,]
                {
                    { 1, "AD01", true, "Shaik", "Admin", "AD01@123" },
                    { 2, "CH01", true, "Sai", "Chef", "CH01@123" },
                    { 3, "MN01", true, "Satya", "Manager", "MN01@123" },
                    { 4, "WA01", true, "Anusha", "Waiter", "WA01@123" },
                    { 5, "CS01", true, "Om Prakash", "Cashier", "CS01@123" },
                    { 6, "IC01", true, "Rishab", "Inventory Clerk", "IC01@123" }
                });
        }
    }
}
