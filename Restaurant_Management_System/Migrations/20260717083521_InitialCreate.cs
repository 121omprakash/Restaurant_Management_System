using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrentStock = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StockStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PreparationTime = table.Column<int>(type: "int", nullable: false),
                    ItemStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemId);
                });

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

            migrationBuilder.CreateTable(
                name: "TableStatuses",
                columns: table => new
                {
                    TableNumber = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableStatuses", x => x.TableNumber);
                });

            migrationBuilder.CreateTable(
                name: "ItemRecipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRecipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_ItemRecipes_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemRecipes_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableNumber = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_TableStatuses_TableNumber",
                        column: x => x.TableNumber,
                        principalTable: "TableStatuses",
                        principalColumn: "TableNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillInvoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubtotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TipAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillInvoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_BillInvoices_CustomerOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "CustomerOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenTickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Station = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedChef = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TicketStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenTickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_KitchenTickets_CustomerOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "CustomerOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_CustomerOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "CustomerOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "BaseCgstPercentage", "BaseSgstPercentage", "CorporateEmail", "LowStockThreshold", "PhysicalAddress", "PrimaryPhone", "RestaurantName", "TaxIdentifier" },
                values: new object[] { 1, 9.00m, 9.00m, "operations@pizzahub.com", 15, "1024, Banjara Hills, Hyderabad, Telangana", "+91 987654321", "Pizza Hub", "GSTIN9283471029B1Z4" });

            migrationBuilder.CreateIndex(
                name: "IX_BillInvoices_OrderId",
                table: "BillInvoices",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_TableNumber",
                table: "CustomerOrders",
                column: "TableNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmpId",
                table: "Employees",
                column: "EmpId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecipes_IngredientId",
                table: "ItemRecipes",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecipes_MenuItemId",
                table: "ItemRecipes",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTickets_OrderId",
                table: "KitchenTickets",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillInvoices");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "ItemRecipes");

            migrationBuilder.DropTable(
                name: "KitchenTickets");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "TableStatuses");
        }
    }
}
