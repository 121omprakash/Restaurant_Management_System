using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Models
{
    public enum itemStatus
    {
        AVAILABLE,
        OUT_OF_STOCK,
        RETIRED
    }
    public class Menu_item
    {
        [Key]

        public int menuItemId { get; set; }

        public string itemName { get; set; }

        public string category { get; set; }

        public decimal price { get; set; }

        public int preparationTime { get; set; }

        public itemStatus itemStatus { get; set; }


    }
}
