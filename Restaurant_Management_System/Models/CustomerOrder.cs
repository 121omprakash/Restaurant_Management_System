using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Models
{
    public enum OrderType
    {
        DINE_IN,
        TAKEAWAY,
        DELIVERY
    }

    public enum OrderStatus
    {
        NEW , 
        PREPARING,
        READY,
        SERVED,
        CANCEL
    }
    public class CustomerOrder
    {
        [Key]
        public int OrderId { get; set; }

        public string TableNumber { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; } 
    }
}