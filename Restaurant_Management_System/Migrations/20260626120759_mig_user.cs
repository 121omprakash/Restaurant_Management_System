using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:Restaurant_Management_System/Migrations/20260626114528_mig_addUser.cs
    public partial class mig_addUser : Migration
========
    public partial class mig_user : Migration
>>>>>>>> 6f9a044579301035bdac907b788c8ac78d91a0ec:Restaurant_Management_System/Migrations/20260626120759_mig_user.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Role", "UserId", "password" },
                values: new object[,]
                {
                    { 1, "Admin", "AD01", "AD01@123" },
                    { 2, "Chef", "CH01", "CH01@123" },
                    { 3, "Manager", "MN01", "MN01@123" },
                    { 4, "Waiter", "WA01", "WA01@123" },
                    { 5, "Cashier", "CS01", "CS01@123" },
                    { 6, "Inventory Clerk", "IC01", "IC01@123" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
