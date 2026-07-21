using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class RecipeStepsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipeSteps",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeSteps",
                table: "MenuItems");
        }
    }
}
