using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using Restaurant_Management_System.ENUM;
using System.Linq;

namespace Restaurant_Management_System.Services
{
    public class InventoryService : IInventory
    {
        private readonly rmsDbContext _context;

        public InventoryService(rmsDbContext context)
        {
            _context = context;
        }

        public InventoryDashboardViewModel GetDashboard()
        {
            var items = _context.Ingredients.ToList();
            var lowItems = items.Where(i => i.StockStatus == StockStatus.LOW || i.StockStatus == StockStatus.OUT_OF_STOCK).ToList();

            return new InventoryDashboardViewModel
            {
                TotalItems = items.Count,
                LowStockCount = lowItems.Count,
                CriticalCount = items.Count(i => i.StockStatus == StockStatus.OUT_OF_STOCK),
                PurchaseRequests = 1,
                LowItems = lowItems,
                AllItems = items
            };
        }

        public InventoryListViewModel GetAll(string searchTerm = null)
        {
            var query = _context.Ingredients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var s = searchTerm.Trim();
                query = query.Where(i => i.IngredientName.Contains(s));
            }

            var items = query.ToList();
            return new InventoryListViewModel { Items = items, SearchTerm = searchTerm ?? string.Empty };
        }

        public InventoryItemViewModel GetItem(int id)
        {
            var item = _context.Ingredients.FirstOrDefault(i => i.IngredientId == id);
            return new InventoryItemViewModel { Ingredient = item ?? new Ingredient() };
        }

        public void Create(Ingredient ingredient)
        {
            UpdateStockStatus(ingredient);
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
        }

        public void Update(Ingredient ingredient)
        {
            var item = _context.Ingredients.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId);
            if (item == null) return;

            item.IngredientName = ingredient.IngredientName;
            item.UnitOfMeasure = ingredient.UnitOfMeasure;
            item.CurrentStock = ingredient.CurrentStock;
            item.ReorderLevel = ingredient.ReorderLevel;

            UpdateStockStatus(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Ingredients.FirstOrDefault(i => i.IngredientId == id);
            if (item == null) return;

            _context.Ingredients.Remove(item);
            _context.SaveChanges();
        }

        public InventoryListViewModel GetLowStock()
        {
            var lowItems = _context.Ingredients.Where(i => i.StockStatus == StockStatus.LOW || i.StockStatus == StockStatus.OUT_OF_STOCK).ToList();
            return new InventoryListViewModel { Items = lowItems };
        }

        private void UpdateStockStatus(Ingredient item)
        {
            if (item.CurrentStock <= 0)
            {
                item.StockStatus = StockStatus.OUT_OF_STOCK;
            }
            else if (item.CurrentStock <= item.ReorderLevel)
            {
                item.StockStatus = StockStatus.LOW;
            }
            else
            {
                item.StockStatus = StockStatus.AVAILABLE;
            }
        }
    }
}
