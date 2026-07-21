using System.Collections.Generic;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class CashierDashboardViewModel
    {
        public int PendingBillsCount { get; set; }
        public int SettledTodayCount { get; set; }
        public decimal TipsCollectedToday { get; set; }
        public decimal TotalRevenueToday { get; set; }

        public string ActiveTab { get; set; } = "ALL";
        
        // This list will hold the orders we show in the dashboard table
        public List<CustomerOrder> ActiveOrders { get; set; } = new List<CustomerOrder>();

        // Drilldown properties
        public bool ShowDrillDown { get; set; }
        public string DrillCustomer { get; set; } = string.Empty;
        public string DrillTable { get; set; } = string.Empty;
        public string DrillItems { get; set; } = string.Empty;
        public string DrillTip { get; set; } = string.Empty;
        public string DrillTotal { get; set; } = string.Empty;
    }
}