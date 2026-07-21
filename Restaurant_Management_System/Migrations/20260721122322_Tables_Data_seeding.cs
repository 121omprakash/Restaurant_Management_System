using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class Tables_Data_seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CustomerOrders",
                columns: new[] { "OrderId", "CustomerName", "OrderStatus", "OrderTime", "OrderType", "TableNumber" },
                values: new object[,]
                {
                    { 1001, "Ravi", 3, new DateTime(2026, 5, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), 0, "T07" },
                    { 1002, "Anita", 1, new DateTime(2026, 5, 21, 13, 15, 0, 0, DateTimeKind.Unspecified), 0, "T08" },
                    { 1003, "Karan", 0, new DateTime(2026, 5, 21, 13, 45, 0, 0, DateTimeKind.Unspecified), 1, "T03" },
                    { 1004, "Sita", 4, new DateTime(2026, 5, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 2, "T02" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "IngredientId", "CurrentStock", "IngredientName", "ReorderLevel", "StockStatus", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 1, 100.00m, "Flour", 20.00m, "AVAILABLE", "KG" },
                    { 2, 50.00m, "Chicken", 10.00m, "AVAILABLE", "KG" },
                    { 3, 40.00m, "Cheese", 10.00m, "AVAILABLE", "KG" },
                    { 4, 60.00m, "Potato", 15.00m, "AVAILABLE", "KG" },
                    { 5, 30.00m, "Tomato", 5.00m, "LOW", "KG" },
                    { 6, 500.00m, "Garlic", 100.00m, "AVAILABLE", "G" },
                    { 7, 10.00m, "Oil", 2.00m, "AVAILABLE", "L" },
                    { 8, 20.00m, "Sugar", 5.00m, "AVAILABLE", "KG" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "Category", "ImagePath", "ItemName", "ItemStatus", "PreparationTime", "Price", "RecipeSteps" },
                values: new object[,]
                {
                    { 1, "Pizza", null, "BBQ Chicken Pizza", 0, 20, 550.00m, null },
                    { 2, "Pizza", null, "Farm House Pizza", 0, 18, 500.00m, null },
                    { 3, "Pizza", null, "Margherita", 0, 15, 350.00m, null },
                    { 4, "Sides", null, "Fries", 0, 8, 120.00m, null },
                    { 5, "Beverage", null, "Coke", 0, 1, 50.00m, null },
                    { 6, "Sides", null, "Garlic Bread", 0, 7, 90.00m, null }
                });

            migrationBuilder.InsertData(
                table: "BillInvoices",
                columns: new[] { "InvoiceId", "OrderId", "PaymentStatus", "SubtotalAmount", "TaxAmount", "TipAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 5001, 1001, 1, 1220.00m, 219.60m, 50.00m, 1489.60m },
                    { 5002, 1002, 0, 500.00m, 90.00m, 0.00m, 590.00m },
                    { 5003, 1003, 1, 240.00m, 43.20m, 0.00m, 283.20m },
                    { 5004, 1004, 2, 350.00m, 63.00m, 0.00m, 413.00m }
                });

            migrationBuilder.InsertData(
                table: "ItemRecipes",
                columns: new[] { "RecipeId", "IngredientId", "MenuItemId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 0.30m },
                    { 2, 2, 1, 0.20m },
                    { 3, 3, 1, 0.15m },
                    { 4, 1, 2, 0.28m },
                    { 5, 3, 2, 0.12m },
                    { 6, 4, 4, 0.25m },
                    { 7, 8, 5, 0.33m },
                    { 8, 6, 6, 0.02m }
                });

            migrationBuilder.InsertData(
                table: "KitchenTickets",
                columns: new[] { "TicketId", "AssignedChef", "CompletionTime", "OrderId", "StartTime", "Station", "TicketStatus" },
                values: new object[,]
                {
                    { 3001, "Sai", null, 1002, new DateTime(2026, 5, 21, 13, 20, 0, 0, DateTimeKind.Unspecified), "Pizza Oven", 1 },
                    { 3002, "Rishab", new DateTime(2026, 5, 21, 12, 50, 0, 0, DateTimeKind.Unspecified), 1001, new DateTime(2026, 5, 21, 12, 35, 0, 0, DateTimeKind.Unspecified), "Grill", 3 },
                    { 3003, null, null, 1003, null, "Fryer", 0 },
                    { 3004, null, null, 1004, null, "Oven", 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "MenuItemId", "OrderId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 2001, 1, 1001, 550.00m, 2 },
                    { 2002, 4, 1001, 120.00m, 1 },
                    { 2003, 2, 1002, 500.00m, 1 },
                    { 2004, 4, 1003, 120.00m, 2 },
                    { 2005, 3, 1004, 350.00m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BillInvoices",
                keyColumn: "InvoiceId",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "BillInvoices",
                keyColumn: "InvoiceId",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "BillInvoices",
                keyColumn: "InvoiceId",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "BillInvoices",
                keyColumn: "InvoiceId",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ItemRecipes",
                keyColumn: "RecipeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "KitchenTickets",
                keyColumn: "TicketId",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "KitchenTickets",
                keyColumn: "TicketId",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "KitchenTickets",
                keyColumn: "TicketId",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "KitchenTickets",
                keyColumn: "TicketId",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2003);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2004);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2005);

            migrationBuilder.DeleteData(
                table: "CustomerOrders",
                keyColumn: "OrderId",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "CustomerOrders",
                keyColumn: "OrderId",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "CustomerOrders",
                keyColumn: "OrderId",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "CustomerOrders",
                keyColumn: "OrderId",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6);
        }
    }
}
