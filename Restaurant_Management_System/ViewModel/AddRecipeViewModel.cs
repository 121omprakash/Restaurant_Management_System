using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Models.ViewModels
{
    public class AddRecipeViewModel
    {
        public int MenuItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int PreparationTime { get; set; }

        public string? RecipeSteps { get; set; }

        public List<int> IngredientIds { get; set; } = new();

        public List<decimal> Quantities { get; set; } = new();

        public List<string> IngredientNames { get; set; } = new();

        public List<string> Units { get; set; } = new();

        public bool IsReadOnly { get; set; }

        public bool IsEdit {  get; set; }
    }
}