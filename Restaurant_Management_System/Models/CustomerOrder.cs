using Restaurant_Management_System.ENUM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class CustomerOrder
    {
        [Key]
        public int OrderId { get; set; }

        public string TableNumber { get; set; } = null!;

        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }

        [ForeignKey(nameof(TableNumber))]
        public TableStatus TableStatus { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

        public ICollection<KitchenTicket> KitchenTickets { get; set; }
            = new List<KitchenTicket>();

        public BillInvoice? BillInvoice { get; set; }
    }
}