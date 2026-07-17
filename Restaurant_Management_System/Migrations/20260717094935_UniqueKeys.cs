using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class UniqueKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowStockThreshold",
                table: "SystemSettings");

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "MenuItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ItemName",
                table: "MenuItems",
                column: "ItemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients",
                column: "IngredientName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ItemName",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "LowStockThreshold",
                table: "SystemSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "LowStockThreshold",
                value: 15);
        }
    }
}
