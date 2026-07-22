using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using System.IO;

namespace Restaurant_Management_System.Controllers
{
    public class ManagerController : Controller
    {
        private readonly rmsDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public ManagerController(rmsDbContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Dashboard()
        {
            DashboardVM vm = new();

            vm.TotalOrders = _context.CustomerOrders.Count();

            vm.Revenue = _context.BillInvoices.Any()
                ? _context.BillInvoices.Sum(x => x.TotalAmount)
                : 0;

            vm.TotalTables = _context.RestaurantTables.Count();

            vm.OccupiedTables = _context.RestaurantTables.Count(x => x.IsOccupied);

            vm.PendingOrders = _context.CustomerOrders.Count(x => x.OrderStatus == OrderStatus.NEW);
            vm.PreparingOrders = _context.CustomerOrders.Count(x => x.OrderStatus == OrderStatus.PREPARING);
            vm.ReadyOrders = _context.CustomerOrders.Count(x => x.OrderStatus == OrderStatus.READY);
            vm.ServedOrders = _context.CustomerOrders.Count(x => x.OrderStatus == OrderStatus.SERVED);
            vm.CancelledOrders = _context.CustomerOrders.Count(x => x.OrderStatus == OrderStatus.CANCEL);

            vm.DelayedOrders = _context.CustomerOrders
                .Count(x => x.OrderStatus == OrderStatus.PREPARING
                         && x.OrderTime < DateTime.Now.AddMinutes(-30));

            vm.LowStockItems = _context.Ingredients
                .Where(x => x.CurrentStock <= x.ReorderLevel)
                .ToList();

            return View(vm);
        }

        public IActionResult MenuManagement()
        {
            var menuItems = _context.MenuItems
                .AsNoTracking()
                .ToList();

            return View(menuItems);
        }

        public IActionResult MenuView(int id)
        {
            var menuItem = _context.MenuItems
                .Include(m => m.ItemRecipes)
                    .ThenInclude(r => r.Ingredient)
                .FirstOrDefault(m => m.MenuItemId == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [HttpGet]
        public IActionResult MenuAdd()
        {
            var vm = new MenuItemCreateViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MenuAdd(MenuItemCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string? savedImagePath = null;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "menu");
                if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

                var fileExt = Path.GetExtension(vm.ImageFile.FileName);
                var fileName = $"menu_{Guid.NewGuid():N}{fileExt}";
                var fullPath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    vm.ImageFile.CopyTo(stream);
                }

                savedImagePath = $"/images/menu/{fileName}";
            }

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
            _context.SaveChanges();

            return RedirectToAction(nameof(MenuManagement));
        }

        [HttpGet]
        public IActionResult MenuEdit(int id)
        {
            var menuItem = _context.MenuItems.Find(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            var vm = new MenuItemCreateViewModel
            {
                MenuItemId = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Category = menuItem.Category,
                Price = menuItem.Price,
                ItemStatus = menuItem.ItemStatus,
                ImagePath = menuItem.ImagePath
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MenuEdit(MenuItemCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var menuItem = _context.MenuItems.Find(vm.MenuItemId);

            if (menuItem == null)
            {
                return NotFound();
            }

            string? savedImagePath = vm.ImagePath;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "menu");
                if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

                var fileExt = Path.GetExtension(vm.ImageFile.FileName);
                var fileName = $"menu_{Guid.NewGuid():N}{fileExt}";
                var fullPath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    vm.ImageFile.CopyTo(stream);
                }

                savedImagePath = $"/images/menu/{fileName}";
            }

            menuItem.ItemName = vm.ItemName;
            menuItem.Category = vm.Category;
            menuItem.Price = vm.Price;
            menuItem.ItemStatus = vm.ItemStatus;
            menuItem.ImagePath = savedImagePath;

            _context.MenuItems.Update(menuItem);
            _context.SaveChanges();

            return RedirectToAction(nameof(MenuManagement));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MenuDelete(int id)
        {
            var menuItem = _context.MenuItems.Find(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            _context.MenuItems.Remove(menuItem);
            _context.SaveChanges();

            return RedirectToAction(nameof(MenuManagement));
        }
        
        public IActionResult OrderMonitoring(OrderStatus? status)
        {
            var query = _context.CustomerOrders
                .Include(o => o.BillInvoice)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.OrderStatus == status.Value);
            }

            var orders = query.Select(o => new OrderMonitoringViewModel
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                TableNumber = o.TableNumber,
                OrderTime = o.OrderTime,
                OrderStatus = o.OrderStatus,
                TotalAmount = o.BillInvoice != null ? o.BillInvoice.TotalAmount : 0
            }).ToList();

            return View(orders);
        }

        public IActionResult TableManagement()
        {
            var tables = _context.RestaurantTables
                .OrderBy(t => t.TableNumber)
                .ToList();

            var vm = new TableManagementVM
            {
                AvailableTables = tables.Where(t => !t.IsOccupied).ToList(),
                OccupiedTables = tables.Where(t => t.IsOccupied).ToList()
            };

            return View(vm);
        }
        

        public IActionResult Inventory(string search)
        {
            var query = _context.Ingredients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.IngredientName.Contains(search));
            }

            var vm = new InventoryListViewModel
            {
                Items = query.ToList(),
                SearchTerm = search ?? string.Empty,
                UserRole = "Manager" // Sets role context for view controls
            };

            return View(vm);
        }
       
        
        public IActionResult Reports()
        {
            var vm = new ReportsViewModel();
            var today = DateTime.Today;

            var invoices = _context.BillInvoices
                .Include(b => b.CustomerOrder)
                .AsQueryable();

            vm.TodaysRevenue = invoices
                .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date == today)
                .Sum(b => (decimal?)b.TotalAmount) ?? 0m;

            vm.ThisWeekRevenue = invoices
                .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date >= today.AddDays(-6))
                .Sum(b => (decimal?)b.TotalAmount) ?? 0m;

            vm.ThisMonthRevenue = invoices
                .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Month == today.Month && b.CustomerOrder.OrderTime.Year == today.Year)
                .Sum(b => (decimal?)b.TotalAmount) ?? 0m;

            vm.AverageBillValue = _context.BillInvoices.Any()
                ? Math.Round(_context.BillInvoices.Average(b => b.TotalAmount), 2)
                : 0m;

            for (int i = 6; i >= 0; i--)
            {
                var day = today.AddDays(-i);
                var dayTotal = invoices
                    .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date == day)
                    .Sum(b => (decimal?)b.TotalAmount) ?? 0m;
                vm.RevenueTrend.Add(dayTotal);
            }

            var topItems = _context.OrderItems
                .Include(oi => oi.MenuItem)
                .GroupBy(oi => oi.MenuItem.ItemName)
                .Select(g => new { Name = g.Key, Qty = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Qty)
                .Take(5)
                .ToList();

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

            var recent = invoices
                .OrderByDescending(b => b.CustomerOrder != null ? b.CustomerOrder.OrderTime : DateTime.MinValue)
                .Take(5)
                .ToList();

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

            return View(vm);
        }
       
    }
}
