using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Restaurant_Management_System.Models
{
    public enum ItemStatus
    {
        AVAILABLE,
        OUT_OF_STOCK,
        RETIRED
    }
    public class MenuItem    {
        [Key]

        public int MenuItemId { get; set; }

        public string ItemName { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int PreparationTime { get; set; }

        public ItemStatus ItemStatus { get; set; }


    }
}
