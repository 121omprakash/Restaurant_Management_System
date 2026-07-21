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
            return View();
        }

        public IActionResult MenuManagement()
        {
            return View();
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

        public IActionResult Inventory()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }
    }
}