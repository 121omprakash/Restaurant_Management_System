using Restaurant_Management_System.ENUM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required]
        public string ItemName { get; set; } = null!;

        [Required]
        public MenuCategory Category { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // PreparationTime removed from create form; property retained on model for compatibility
        public int PreparationTime { get; set; }
        public ItemStatus ItemStatus { get; set; }

        public string? RecipeSteps { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<ItemRecipe> ItemRecipes { get; set; }
            = new List<ItemRecipe>();
    }
}