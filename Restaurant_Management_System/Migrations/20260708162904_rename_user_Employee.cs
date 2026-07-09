using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class rename_user_Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "EmpId", "Name", "Role", "password" },
                values: new object[,]
                {
                    { 1, "AD01", "Shaik", "Admin", "AD01@123" },
                    { 2, "CH01", "Sai", "Chef", "CH01@123" },
                    { 3, "MN01", "Satya", "Manager", "MN01@123" },
                    { 4, "WA01", "Anusha", "Waiter", "WA01@123" },
                    { 5, "CS01", "Om Prakash", "Cashier", "CS01@123" },
                    { 6, "IC01", "Rishab", "Inventory Clerk", "IC01@123" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Role", "UserId", "password" },
                values: new object[,]
                {
                    { 1, "Shaik", "Admin", "AD01", "AD01@123" },
                    { 2, "Sai", "Chef", "CH01", "CH01@123" },
                    { 3, "Satya", "Manager", "MN01", "MN01@123" },
                    { 4, "Anusha", "Waiter", "WA01", "WA01@123" },
                    { 5, "Om Prakash", "Cashier", "CS01", "CS01@123" },
                    { 6, "Rishab", "Inventory Clerk", "IC01", "IC01@123" }
                });
        }
    }
}
