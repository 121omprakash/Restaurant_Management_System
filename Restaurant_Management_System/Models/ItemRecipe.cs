using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class ItemRecipe
    {
        [Key]
        public int RecipeId { get; set; }

        public int MenuItemId { get; set; }

        public int IngredientId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem { get; set; } = null!;

        [ForeignKey(nameof(IngredientId))]
        public Ingredient Ingredient { get; set; } = null!;
    }
}