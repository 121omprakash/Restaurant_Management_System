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

        // This list will hold the orders we show in the dashboard table
        public List<CustomerOrder> ActiveOrders { get; set; } = new List<CustomerOrder>();
    }
}