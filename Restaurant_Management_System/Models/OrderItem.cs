using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Restaurant_Management_System.Models
{
    public class OrderItem
    {
        [Key]
        int OrderItemId { get; set; }
        [Required]
        int OrderId { get; set; }
        [Required]
        int MenuItemId { get; set; }
        int Quantity { get; set; }
        int Price { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual CustomerOrder CustomerOrder { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        public virtual MenuItem MenuItem { get; set; }


    }
}
