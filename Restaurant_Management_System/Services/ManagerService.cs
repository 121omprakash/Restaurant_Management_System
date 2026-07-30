using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using Microsoft.AspNetCore.Hosting;
namespace Restaurant_Management_System.Services
{
    public class ManagerService : IManagerService
    {
        private readonly rmsDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ManagerService> _logger;

        public ManagerService(rmsDbContext context, IWebHostEnvironment env, ILogger<ManagerService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<DashboardVM> GetDashboardDataAsync()
        {
            DashboardVM vm = new();

            vm.TotalOrders = await _context.CustomerOrders.CountAsync();

            vm.Revenue = await _context.BillInvoices.AnyAsync()
                ? await _context.BillInvoices.SumAsync(x => x.TotalAmount)
                : 0;

            vm.TotalTables = await _context.RestaurantTables.CountAsync();
            vm.OccupiedTables = await _context.RestaurantTables.CountAsync(x => x.IsOccupied);

            vm.PendingOrders = await _context.CustomerOrders.CountAsync(x => x.OrderStatus == OrderStatus.NEW);
            vm.PreparingOrders = await _context.CustomerOrders.CountAsync(x => x.OrderStatus == OrderStatus.PREPARING);
            vm.ReadyOrders = await _context.CustomerOrders.CountAsync(x => x.OrderStatus == OrderStatus.READY);
            vm.ServedOrders = await _context.CustomerOrders.CountAsync(x => x.OrderStatus == OrderStatus.SERVED);
            vm.CancelledOrders = await _context.CustomerOrders.CountAsync(x => x.OrderStatus == OrderStatus.CANCEL);

            vm.DelayedOrders = await _context.CustomerOrders
                .CountAsync(x => x.OrderStatus == OrderStatus.PREPARING
                              && x.OrderTime < DateTime.Now.AddMinutes(-30));

            vm.LowStockItems = await _context.Ingredients
                .Where(x => x.CurrentStock <= x.ReorderLevel)
                .ToListAsync();

            return vm;
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemWithRecipeAsync(int id)
        {
            return await _context.MenuItems
                .Include(m => m.ItemRecipes)
                    .ThenInclude(r => r.Ingredient)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
        }

        public async Task<MenuItemCreateViewModel?> GetMenuItemForEditAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return null;

            return new MenuItemCreateViewModel
            {
                MenuItemId = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Category = menuItem.Category,
                Price = menuItem.Price,
                ItemStatus = menuItem.ItemStatus,
                ImagePath = menuItem.ImagePath
            };
        }

        public async Task<(bool Success, string Message)> AddMenuItemAsync(MenuItemCreateViewModel vm)
        {
            try
            {
                string? savedImagePath = SaveImageFile(vm.ImageFile);

                var menuItem = new MenuItem
                {
                    ItemName = vm.ItemName,
                    Category = vm.Category,
                    Price = vm.Price,
                    ItemStatus = vm.ItemStatus,
                    RecipeSteps = null,
                    ImagePath = savedImagePath
                };

                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();

                return (true, "Menu item added successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding menu item.");
                return (false, "Database error occurred while saving menu item.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding menu item.");
                return (false, "Something went wrong.");
            }
        }

        public async Task<(bool Success, string Message)> UpdateMenuItemAsync(MenuItemCreateViewModel vm)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(vm.MenuItemId);
                if (menuItem == null)
                {
                    return (false, "Menu item not found.");
                }

                string? savedImagePath = vm.ImagePath;

                if (vm.ImageFile != null && vm.ImageFile.Length > 0)
                {
                    savedImagePath = SaveImageFile(vm.ImageFile);
                }

                menuItem.ItemName = vm.ItemName;
                menuItem.Category = vm.Category;
                menuItem.Price = vm.Price;
                menuItem.ItemStatus = vm.ItemStatus;
                menuItem.ImagePath = savedImagePath;

                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();

                return (true, "Menu item updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating menu item.");
                return (false, "Database error occurred while updating menu item.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating menu item.");
                return (false, "Something went wrong.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteMenuItemAsync(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem == null)
                {
                    return (false, "Menu item not found.");
                }

                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();

                return (true, "Menu item deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting menu item.");
                return (false, "Unable to delete menu item.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting menu item.");
                return (false, "Something went wrong.");
            }
        }

        public async Task<List<OrderMonitoringViewModel>> GetMonitoredOrdersAsync(OrderStatus? status)
        {
            var query = _context.CustomerOrders
                .Include(o => o.BillInvoice)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.OrderStatus == status.Value);
            }

            return await query.Select(o => new OrderMonitoringViewModel
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                TableNumber = o.TableNumber,
                OrderTime = o.OrderTime,
                OrderStatus = o.OrderStatus,
                TotalAmount = o.BillInvoice != null ? o.BillInvoice.TotalAmount : 0
            }).ToListAsync();
        }

        public async Task<TableManagementVM> GetTableManagementDataAsync()
        {
            var tables = await _context.RestaurantTables
                .OrderBy(t => t.TableNumber)
                .ToListAsync();

            return new TableManagementVM
            {
                AvailableTables = tables.Where(t => !t.IsOccupied).ToList(),
                OccupiedTables = tables.Where(t => t.IsOccupied).ToList()
            };
        }

        public async Task<InventoryListViewModel> GetInventoryDataAsync(string? search)
        {
            var query = _context.Ingredients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();
                query = query.Where(x => EF.Functions.Like(x.IngredientName, $"%{term}%"));
            }

            return new InventoryListViewModel
            {
                Items = await query.ToListAsync(),
                SearchTerm = search ?? string.Empty,
                UserRole = "Manager"
            };
        }

        public async Task<ReportsViewModel> GetReportsDataAsync()
        {
            var vm = new ReportsViewModel();
            var today = DateTime.Today;

            var invoices = _context.BillInvoices
                .Include(b => b.CustomerOrder)
                .AsQueryable();

            vm.TodaysRevenue = await invoices
                .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date == today)
                .SumAsync(b => (decimal?)b.TotalAmount) ?? 0m;

            vm.ThisWeekRevenue = await invoices
                .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date >= today.AddDays(-6))
                .SumAsync(b => (decimal?)b.TotalAmount) ?? 0m;

            vm.ThisMonthRevenue = await invoices
                .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Month == today.Month && b.CustomerOrder.OrderTime.Year == today.Year)
                .SumAsync(b => (decimal?)b.TotalAmount) ?? 0m;

            vm.AverageBillValue = await _context.BillInvoices.AnyAsync()
                ? Math.Round(await _context.BillInvoices.AverageAsync(b => b.TotalAmount), 2)
                : 0m;

            for (int i = 6; i >= 0; i--)
            {
                var day = today.AddDays(-i);
                var dayTotal = await invoices
                    .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date == day)
                    .SumAsync(b => (decimal?)b.TotalAmount) ?? 0m;
                vm.RevenueTrend.Add(dayTotal);
            }

            var topItems = await _context.OrderItems
                .Include(oi => oi.MenuItem)
                .GroupBy(oi => oi.MenuItem.ItemName)
                .Select(g => new { Name = g.Key, Qty = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Qty)
                .Take(5)
                .ToListAsync();

            var totalQty = topItems.Sum(x => x.Qty);
            foreach (var it in topItems)
            {
                vm.TopSellingItems.Add(new TopSellingItem
                {
                    Name = it.Name ?? string.Empty,
                    Quantity = it.Qty,
                    Percentage = totalQty > 0 ? (int)Math.Round(it.Qty * 100.0 / totalQty) : 0
                });
            }

            var recent = await invoices
                .OrderByDescending(b => b.CustomerOrder != null ? b.CustomerOrder.OrderTime : DateTime.MinValue)
                .Take(5)
                .ToListAsync();

            foreach (var inv in recent)
            {
                vm.RecentInvoices.Add(new RecentInvoice
                {
                    InvoiceId = inv.InvoiceId,
                    InvoiceLabel = $"INV-{inv.InvoiceId}",
                    Date = inv.CustomerOrder?.OrderTime ?? DateTime.MinValue,
                    TableNumber = inv.CustomerOrder?.TableNumber ?? string.Empty,
                    Amount = inv.TotalAmount,
                    PaymentStatus = inv.PaymentStatus.ToString()
                });
            }

            return vm;
        }

        private string? SaveImageFile(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "menu");
            if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

            var fileExt = Path.GetExtension(imageFile.FileName);
            var fileName = $"menu_{Guid.NewGuid():N}{fileExt}";
            var fullPath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return $"/images/menu/{fileName}";
        }
    }
}
