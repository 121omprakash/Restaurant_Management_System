using System.Collections.Generic;

namespace Restaurant_Management_Error.Models
{
    public class InventoryDashboardViewModel
    {
        public int TotalItems { get; set; }               
        public int LowStockCount { get; set; }
        public int CriticalCount { get; set; }
        public int PurchaseRequests { get; set; }
        public List<StockItem> LowItems { get; set; } = new List<StockItem>();
        public List<StockItem> AllItems { get; set; } = new List<StockItem>();
    }
}
