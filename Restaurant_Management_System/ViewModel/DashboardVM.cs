using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class DashboardVM
    {
        public int TotalOrders { get; set; }

        public decimal Revenue { get; set; }

        public int OccupiedTables { get; set; }

        public int TotalTables { get; set; }

        public int PendingOrders { get; set; }

        public int PreparingOrders { get; set; }

        public int ReadyOrders { get; set; }

        public int ServedOrders { get; set; }

        public int CancelledOrders { get; set; }

        public int DelayedOrders { get; set; }

        public List<Ingredient> LowStockItems { get; set; }
            = new();
    }
}
