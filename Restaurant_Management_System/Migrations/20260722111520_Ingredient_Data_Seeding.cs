using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class Ingredient_Data_Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 200.00m, "Pizza Flour", 40.00m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel", "UnitOfMeasure" },
                values: new object[] { 5000.00m, "Yeast", 1000.00m, "G" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 25.00m, "Sea Salt", 5.00m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 20.00m, "Salt", 4.00m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "IngredientName", "StockStatus", "UnitOfMeasure" },
                values: new object[] { "Olive Oil", "AVAILABLE", "L" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel", "UnitOfMeasure" },
                values: new object[] { 80.00m, "Tomato Puree", 15.00m, "KG" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel", "UnitOfMeasure" },
                values: new object[] { 30.00m, "Tomato Paste", 5.00m, "KG" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 15.00m, "Garlic", 3.00m });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "IngredientId", "CurrentStock", "IngredientName", "ReorderLevel", "StockStatus", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 9, 4000.00m, "Oregano", 500.00m, "AVAILABLE", "G" },
                    { 10, 3000.00m, "Basil", 500.00m, "AVAILABLE", "G" },
                    { 11, 100.00m, "Mozzarella Cheese", 20.00m, "AVAILABLE", "KG" },
                    { 12, 50.00m, "Cheddar Cheese", 10.00m, "AVAILABLE", "KG" },
                    { 13, 20.00m, "Parmesan Cheese", 5.00m, "AVAILABLE", "KG" },
                    { 14, 40.00m, "Onion", 10.00m, "AVAILABLE", "KG" },
                    { 15, 35.00m, "Capsicum", 8.00m, "AVAILABLE", "KG" },
                    { 16, 30.00m, "Tomato", 8.00m, "AVAILABLE", "KG" },
                    { 17, 20.00m, "Mushroom", 5.00m, "AVAILABLE", "KG" },
                    { 18, 15.00m, "Black Olive", 3.00m, "AVAILABLE", "KG" },
                    { 19, 15.00m, "Green Olive", 3.00m, "AVAILABLE", "KG" },
                    { 20, 12.00m, "Jalapeno", 2.00m, "AVAILABLE", "KG" },
                    { 21, 30.00m, "Sweet Corn", 5.00m, "AVAILABLE", "KG" },
                    { 22, 10.00m, "Pineapple", 2.00m, "AVAILABLE", "KG" },
                    { 23, 12.00m, "Spinach", 2.00m, "AVAILABLE", "KG" },
                    { 24, 80.00m, "Chicken Breast", 15.00m, "AVAILABLE", "KG" },
                    { 25, 40.00m, "Chicken Sausage", 8.00m, "AVAILABLE", "KG" },
                    { 26, 30.00m, "Pepperoni", 5.00m, "AVAILABLE", "KG" },
                    { 27, 25.00m, "Ham", 5.00m, "AVAILABLE", "KG" },
                    { 28, 20.00m, "Bacon", 4.00m, "AVAILABLE", "KG" },
                    { 29, 4000.00m, "Black Pepper", 500.00m, "AVAILABLE", "G" },
                    { 30, 5000.00m, "Red Chili Flakes", 500.00m, "AVAILABLE", "G" },
                    { 31, 3000.00m, "Italian Seasoning", 500.00m, "AVAILABLE", "G" },
                    { 32, 20.00m, "Mayonnaise", 5.00m, "AVAILABLE", "KG" },
                    { 33, 15.00m, "Barbecue Sauce", 3.00m, "AVAILABLE", "KG" },
                    { 34, 10.00m, "Hot Sauce", 2.00m, "AVAILABLE", "L" },
                    { 35, 10.00m, "Ranch Dressing", 2.00m, "AVAILABLE", "L" },
                    { 36, 15.00m, "Chocolate Syrup", 3.00m, "AVAILABLE", "L" },
                    { 37, 5.00m, "Vanilla Essence", 1.00m, "AVAILABLE", "L" },
                    { 38, 500.00m, "Carbonated Water", 100.00m, "AVAILABLE", "L" },
                    { 39, 80.00m, "Cola Syrup", 15.00m, "AVAILABLE", "L" },
                    { 40, 50.00m, "Lemon Syrup", 10.00m, "AVAILABLE", "L" },
                    { 41, 50.00m, "Orange Syrup", 10.00m, "AVAILABLE", "L" },
                    { 42, 100.00m, "Soda Base", 20.00m, "AVAILABLE", "L" },
                    { 43, 200.00m, "Ice Cubes", 25.00m, "AVAILABLE", "KG" },
                    { 44, 25.00m, "Lemon", 5.00m, "AVAILABLE", "KG" },
                    { 45, 10.00m, "Mint Leaves", 2.00m, "AVAILABLE", "KG" },
                    { 46, 500.00m, "Pizza Box Small", 100.00m, "AVAILABLE", "P" },
                    { 47, 500.00m, "Pizza Box Medium", 100.00m, "AVAILABLE", "P" },
                    { 48, 500.00m, "Pizza Box Large", 100.00m, "AVAILABLE", "P" },
                    { 49, 1000.00m, "Cold Drink Cup", 200.00m, "AVAILABLE", "P" },
                    { 50, 1000.00m, "Plastic Lid", 200.00m, "AVAILABLE", "P" },
                    { 51, 1500.00m, "Paper Straw", 300.00m, "AVAILABLE", "P" },
                    { 52, 3000.00m, "Napkins", 500.00m, "AVAILABLE", "P" },
                    { 53, 25.00m, "Cheese Dip", 5.00m, "AVAILABLE", "KG" },
                    { 54, 20.00m, "Garlic Dip", 5.00m, "AVAILABLE", "KG" },
                    { 55, 3000.00m, "Peri Peri Seasoning", 500.00m, "AVAILABLE", "G" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 55);

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 100.00m, "Flour", 20.00m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel", "UnitOfMeasure" },
                values: new object[] { 50.00m, "Chicken", 10.00m, "KG" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 40.00m, "Cheese", 10.00m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 60.00m, "Potato", 15.00m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "IngredientName", "StockStatus", "UnitOfMeasure" },
                values: new object[] { "Tomato", "LOW", "KG" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel", "UnitOfMeasure" },
                values: new object[] { 500.00m, "Garlic", 100.00m, "G" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel", "UnitOfMeasure" },
                values: new object[] { 10.00m, "Oil", 2.00m, "L" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "CurrentStock", "IngredientName", "ReorderLevel" },
                values: new object[] { 20.00m, "Sugar", 5.00m });
        }
    }
}
