using Restaurant_Management_System.ENUM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class CustomerOrder
    {
        [Key]
        public int OrderId { get; set; }

        [StringLength(50)]
        public string? TableNumber { get; set; }

        [StringLength(100)]
        public string? CustomerName { get; set; }

        public OrderType OrderType { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }

        [ForeignKey(nameof(TableNumber))]
        public RestaurantTable? TableStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

        public ICollection<KitchenTicket> KitchenTickets { get; set; }
            = new List<KitchenTicket>();

        public BillInvoice? BillInvoice { get; set; }
    }
}