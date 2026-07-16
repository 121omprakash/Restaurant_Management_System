using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using System.Linq;
using System.Net.NetworkInformation;


namespace Restaurant_Management_System.Controllers
{
    //[Authorize(Roles = "Chef")]
    public class kitchenController : Controller
    {
        private readonly rmsDbContext _context;

        public kitchenController(rmsDbContext context)
        {
            _context = context;
        } 
        public IActionResult Index()
        {
            return RedirectToAction("OrderManagement");
        }

        public IActionResult OrderManagement(TicketStatus? status, string? search)
        {
            // Dashboard Card Counts
            ViewBag.PendingCount = _context.KitchenTickets.Count(x => x.TicketStatus == TicketStatus.QUEUED);
            ViewBag.PreparingCount = _context.KitchenTickets.Count(x => x.TicketStatus == TicketStatus.IN_PROGRESS);
            ViewBag.CompletedCount = _context.KitchenTickets.Count(x => x.TicketStatus == TicketStatus.READY);
            ViewBag.DelayedCount = 0;

            ViewBag.Search = search;
            ViewBag.SelectedStatus = status;

            var tickets = _context.KitchenTickets
                .Include(k => k.CustomerOrder)
                .AsQueryable();

            // Filter by Status
            if (status.HasValue)
            {
                tickets = tickets.Where(k => k.TicketStatus == status.Value);
            }

            // Search by Order ID or Customer Name
            if (!string.IsNullOrWhiteSpace(search))
            {
                tickets = tickets.Where(k =>
                    k.OrderId.ToString().Contains(search) ||
                    k.CustomerOrder.CustomerName.Contains(search));
            }

            return View(tickets.ToList());
        }


        public IActionResult ViewItems(int orderId)
        {
            var items = _context.OrderItems
                .Where(o => o.OrderId == orderId)
                .Include(o => o.MenuItem)
                .ToList();
               ViewBag.OrderId = orderId;

            return View(items);
        }

        [HttpPost]
        public IActionResult markItemPrepared(int id)
        {
            var ticket = _context.KitchenTickets
                .FirstOrDefault(k => k.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.TicketStatus = TicketStatus.IN_PROGRESS;
            ticket.StartTime = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(OrderManagement));
        }

        [HttpPost]
        public IActionResult completeOrder(int id)
        {
            var ticket = _context.KitchenTickets
                .FirstOrDefault(k => k.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.TicketStatus = TicketStatus.READY;
            ticket.CompletionTime = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(OrderManagement));
        }

       
        public IActionResult RecipeManagement()
        {
            return View();
        }


    }
}
