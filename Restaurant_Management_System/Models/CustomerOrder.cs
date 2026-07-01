using System;

namespace Restaurant_Management_System.Models
{
    public class CustomerOrder
    {
        public int OrderId { get; set; }
        public string TableNumber { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string OrderType { get; set; } = ""; // DINE_IN, TAKEAWAY
        public DateTime OrderTime { get; set; }
        public string OrderStatus { get; set; } = ""; // READY, PREPARING
    }
}