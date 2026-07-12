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
        public string Category { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int PreparationTime { get; set; }

        public ItemStatus ItemStatus { get; set; }

        public ICollection<ItemRecipe> ItemRecipes { get; set; }
            = new List<ItemRecipe>();
    }
}