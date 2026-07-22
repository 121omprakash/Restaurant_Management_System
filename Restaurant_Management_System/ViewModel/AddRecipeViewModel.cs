using System.ComponentModel.DataAnnotations;
using Restaurant_Management_System.ENUM;

namespace Restaurant_Management_System.Models.ViewModels
{
    public class AddRecipeViewModel
    {
        public int MenuItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int PreparationTime { get; set; }

        [Required(ErrorMessage = "Recipe steps are required.")]
        public string RecipeSteps { get; set; }= string.Empty;

        public List<int> IngredientIds { get; set; } = new();

        public List<decimal> Quantities { get; set; } = new();

        public List<string> IngredientNames { get; set; } = new();

        public List<UnitOfMeasure> Units { get; set; } = new();

        public bool IsReadOnly { get; set; }

        public bool IsEdit {  get; set; }
    }
}