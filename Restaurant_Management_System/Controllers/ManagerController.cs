using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers
{
    public class ManagerController : Controller
    {
        private readonly rmsDbContext _context;

        public ManagerController(rmsDbContext context)
        {
            _context = context;
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
            return View();
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
            return View();
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
            return View();
        }
    }
}