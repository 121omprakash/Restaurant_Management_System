using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
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

            vm.OccupiedTables = _context.RestaurantTables
                .Count(x => x.IsOccupied);

            vm.PendingOrders = _context.CustomerOrders
                .Count(x => x.OrderStatus == OrderStatus.NEW);

            vm.PreparingOrders = _context.CustomerOrders
                .Count(x => x.OrderStatus == OrderStatus.PREPARING);

            vm.ReadyOrders = _context.CustomerOrders
                .Count(x => x.OrderStatus == OrderStatus.READY);

            vm.ServedOrders = _context.CustomerOrders
                .Count(x => x.OrderStatus == OrderStatus.SERVED);

            vm.CancelledOrders = _context.CustomerOrders
                .Count(x => x.OrderStatus == OrderStatus.CANCEL);

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
            var menuItems = _context.MenuItems.ToList();

            return View(menuItems);
        }


        public IActionResult MenuAdd()
        {
            var vm = new ViewModel.MenuItemCreateViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MenuAdd(ViewModel.MenuItemCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // if an image file was uploaded, save it to wwwroot/images/menu and set ImagePath
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

                // store web-relative path
                savedImagePath = $"/images/menu/{fileName}";
            }

            // create menu item (variable name: menuItem)
            var menuItem = new Models.MenuItem
            {
                ItemName = vm.ItemName,
                Category = vm.Category,
                Price = vm.Price,
                // keep model default for PreparationTime (removed from form)
                ItemStatus = vm.ItemStatus,
                RecipeSteps = null, // recipiePath/steps null by default
                ImagePath = savedImagePath
            };

            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            // Ingredients removed from create flow per request

            return RedirectToAction("MenuManagement");
        }

        public IActionResult MenuView()
        {
            return View();
        }

        public IActionResult MenuEdit()
        {
            return View();
        }

        public IActionResult MenuDelete()
        {
            return View();
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
                TotalAmount = o.BillInvoice != null
                    ? o.BillInvoice.TotalAmount
                    : 0
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
            var ingredients = _context.Ingredients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                ingredients = ingredients.Where(x =>
                    x.IngredientName.Contains(search));
            }

            return View(ingredients.ToList());
        }

        public IActionResult Reports()
        {
            var vm = new Restaurant_Management_System.ViewModel.ReportsViewModel();

            var today = DateTime.Today;

            // Ensure related CustomerOrder is available for date filtering
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

            // Revenue trend: last 7 days totals
            for (int i = 6; i >= 0; i--)
            {
                var day = today.AddDays(-i);
                var dayTotal = invoices
                    .Where(b => b.CustomerOrder != null && b.CustomerOrder.OrderTime.Date == day)
                    .Sum(b => (decimal?)b.TotalAmount) ?? 0m;
                vm.RevenueTrend.Add(dayTotal);
            }

            // Top selling items by quantity
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

            // Recent invoices (latest 5)
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