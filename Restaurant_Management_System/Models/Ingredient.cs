using Restaurant_Management_System.ENUM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        [Required]
        [StringLength(100)]
        public string IngredientName { get; set; } = null!;

        [Required]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentStock { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ReorderLevel { get; set; }

        public StockStatus StockStatus { get; set; }

        public ICollection<ItemRecipe> ItemRecipes { get; set; }
            = new List<ItemRecipe>();
    }
}