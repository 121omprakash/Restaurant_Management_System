using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    
        public class WaiterOrderMonitoringViewModel
        {
        // Card counts
        public int DeployedOrderCount { get; set; }
        public int DelayedOrderCount { get; set; }
        public int ServedOrderCount { get; set; }

        // Orders for the table
        public List<OrderMonitoringItemViewModel> DeployedOrders { get; set; } = new();
        public List<OrderMonitoringItemViewModel> DelayedOrders { get; set; } = new();
        }
    
}
