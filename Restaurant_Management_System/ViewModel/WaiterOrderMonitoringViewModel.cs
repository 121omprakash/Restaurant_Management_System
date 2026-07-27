using System;
using System.Collections.Generic;

namespace Restaurant_Management_System.ViewModel
{
    // Existing Classes
    public class WaiterOrderMonitoringViewModel
    {
        public int DeployedOrderCount { get; set; }
        public int DelayedOrderCount { get; set; }
        public int ServedOrderCount { get; set; }

        public List<WaiterOrderMonitoringItemViewModel> DeployedOrders { get; set; } = new();
        public List<WaiterOrderMonitoringItemViewModel> DelayedOrders { get; set; } = new();
        public List<WaiterOrderMonitoringItemViewModel> ServedOrders { get; set; } = new();
    }

    public class WaiterOrderMonitoringItemViewModel
    {
        public int OrderId { get; set; }
        public int KitchenTicketId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string TableNumber { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

    // ALIASES for legacy class references
    public class OrderMonitoringViewModel : WaiterOrderMonitoringViewModel { }
    public class OrderMonitoringItemViewModel : WaiterOrderMonitoringItemViewModel { }
}