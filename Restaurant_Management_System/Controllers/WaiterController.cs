using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers
{
    public class WaiterController : Controller
    {
        private readonly rmsDbContext _context;
        public WaiterController(rmsDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }


        public IActionResult TableReservation()
        {
            return View();
        }


        public IActionResult TakeOrder()
        {
            return View();
        }


        public IActionResult ManageOrders()
        {
            return View();
        }


        public IActionResult OrderStatus()
        {
            return View();
        }


        public IActionResult ServeOrders()
        {
            return View();
        }


        public IActionResult Billing()
        {
            return View();
        }

        public async Task<IActionResult> OrderMonitoring()
        {
            var vm = new WaiterOrderMonitoringViewModel();

            var tickets = await _context.KitchenTickets
                .Include(k => k.CustomerOrder)
                .ThenInclude(o => o.OrderItems)
                .Where(k => k.TicketStatus == ENUM.TicketStatus.IN_PROGRESS || k.TicketStatus == ENUM.TicketStatus.READY)
                .ToListAsync();

            foreach (var t in tickets)
            {
                var item = new OrderMonitoringItemViewModel
                {
                    KitchenTicketId = t.TicketId,
                    OrderId = t.OrderId,
                    TableNumber = t.CustomerOrder?.TableNumber ?? string.Empty,
                    CustomerName = t.CustomerOrder?.CustomerName ?? string.Empty,
                    OrderType = t.CustomerOrder?.OrderType.ToString() ?? string.Empty,
                    TotalItems = t.CustomerOrder?.OrderItems?.Count ?? 0,
                    OrderTime = t.CustomerOrder?.OrderTime ?? DateTime.MinValue,
                    Status = t.TicketStatus.ToString()
                };

                if (t.TicketStatus == ENUM.TicketStatus.IN_PROGRESS)
                {
                    vm.DeployedOrders.Add(item);
                }
                else if (t.TicketStatus == ENUM.TicketStatus.READY)
                {
                    vm.DelayedOrders.Add(item);
                }
            }

            vm.DeployedOrderCount = vm.DeployedOrders.Count;
            vm.DelayedOrderCount = vm.DelayedOrders.Count;

            // Count today's served orders (CompletionTime on current date)
            var today = DateTime.Today;
            vm.ServedOrderCount = await _context.KitchenTickets
                .Where(k => k.TicketStatus == ENUM.TicketStatus.SERVED && k.CompletionTime.HasValue && k.CompletionTime.Value.Date == today)
                .CountAsync();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> MarkServed(int ticketId)
        {
            var ticket = await _context.KitchenTickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.TicketStatus = ENUM.TicketStatus.SERVED;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(OrderMonitoring));
        }
    }
}
