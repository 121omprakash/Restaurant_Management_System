using Restaurant_Management_System.Models;
using System.Collections.Generic;

namespace Restaurant_Management_System.ViewModel
{
    public class InventoryDashboardViewModel
    {
        public int TotalItems { get; set; }               
        public int LowStockCount { get; set; }
        public int CriticalCount { get; set; }
        public int PurchaseRequests { get; set; }
        public List<Ingredient> LowItems { get; set; } = new List<Ingredient>();
        public List<Ingredient> AllItems { get; set; } = new List<Ingredient>();
    }
}
