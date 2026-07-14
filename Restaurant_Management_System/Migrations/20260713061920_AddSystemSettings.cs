using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimaryPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CorporateEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhysicalAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TaxIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseCgstPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaseSgstPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "BaseCgstPercentage", "BaseSgstPercentage", "CorporateEmail", "LowStockThreshold", "PhysicalAddress", "PrimaryPhone", "RestaurantName", "TaxIdentifier" },
                values: new object[] { 1, 9.00m, 9.00m, "operations@pizzahub.com", 15, "1024, Banjara Hills, Hyderabad, Telangana", "+91 987654321", "Pizza Hub", "GSTIN9283471029B1Z4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");
        }
    }
}
