using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using System.Collections.Generic;

namespace Restaurant_Management_System.Services
{
    public interface IInventory
    {
        InventoryDashboardViewModel GetDashboard();
        InventoryListViewModel GetAll(string searchTerm = null);
        InventoryItemViewModel GetItem(int id);
        void Create(Ingredient ingredient);
        void Update(Ingredient ingredient);
        void Delete(int id);
        InventoryListViewModel GetLowStock();
    }
}
