using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Restaurant_Management_System.ENUM;

namespace Restaurant_Management_Error.Models
{
    public class Ingredient
    {

        [Key]
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Ingredient name is required.")]
        [StringLength(100)]
        public string IngredientName { get; set; }

        [Required(ErrorMessage = "Unit of Measure is required")]
        [StringLength(20)]
        public string UnitOfMeasure { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentStock { get; set; }
        
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ReorderLevel { get; set; }

        public StockStatus StockStatus { get; set; }
    }
}
