using Restaurant_Management_System.ENUM;

namespace Restaurant_Management_System.ViewModel
{
    public class OrderMonitoringViewModel
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string TableNumber { get; set; } = null!;

        public OrderStatus OrderStatus { get; set; }

        public DateTime OrderTime { get; set; }

        public decimal TotalAmount { get; set; }
    }
}