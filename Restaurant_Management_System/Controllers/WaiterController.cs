using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
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

  

        private void SeedRestaurantTablesIfMissing()
        {
            // Ensures RestaurantTables has records for "Table 1" through "Table 12"
            var existingTables = _context.RestaurantTables.Select(t => t.TableNumber).ToList();

            for (int i = 1; i <= 12; i++)
            {
                string tableName = $"Table {i}";
                if (!existingTables.Contains(tableName))
                {
                    _context.RestaurantTables.Add(new RestaurantTable
                    {
                        TableNumber = tableName
                    });
                }
            }

            _context.SaveChanges();
        }

        

        [HttpGet]
        public IActionResult Menu()
        {
            var menuItems = _context.MenuItems != null
                ? _context.MenuItems.ToList()
                : new List<MenuItem>();

            return View(menuItems);
        }

        [HttpGet]
        public IActionResult Cart()
        {
            return View();
        }


        // GET: /Waiter/GetTables
        [HttpGet]
        public IActionResult GetTables()
        {
            // 1. Ensure Table 1 through Table 12 exist in the foreign key table
            SeedRestaurantTablesIfMissing();

            // 2. Fetch occupied tables from active orders where kitchen tickets exist in DB
            var activeOrdersWithTables = _context.CustomerOrders
                .Where(o => !string.IsNullOrEmpty(o.TableNumber))
                .Select(o => o.TableNumber)
                .ToList();

            // Extract table number digits reliably regardless of format (e.g. "Table 1", "Table T08", "Table Table 2")
            int ExtractTableNum(string? input)
            {
                if (string.IsNullOrWhiteSpace(input)) return 0;
                var digits = new string(input.Where(char.IsDigit).ToArray());
                return int.TryParse(digits, out int num) ? num : 0;
            }

            var occupiedTableNumbers = activeOrdersWithTables
                .Select(ExtractTableNum)
                .Where(num => num > 0)
                .ToHashSet();

            var tablesList = new List<object>();

            // 3. Generate status response for all 12 tables
            for (int i = 1; i <= 12; i++)
            {
                bool isOccupied = occupiedTableNumbers.Contains(i);

                tablesList.Add(new
                {
                    Id = i,
                    Name = $"Table {i}",
                    Status = isOccupied ? "Occupied" : "Available"
                });
            }

            return Json(tablesList);
        }

        // POST: /Waiter/PlaceOrder
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.CustomerName) || dto.Items == null || !dto.Items.Any())
            {
                return Json(new { success = false, message = "Invalid order data. Please check item selections." });
            }

            try
            {
                // Ensure foreign key target records exist before inserting order
                SeedRestaurantTablesIfMissing();

                string? tableNumber = null;

                // 1. Validate Table Selection for Dine-In
                if (dto.OrderType.Equals("DineIn", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(dto.TableName))
                    {
                        return Json(new { success = false, message = "Please select a table for Dine-In orders." });
                    }

                    // Format table name to standard "Table X" format
                    var digits = new string(dto.TableName.Where(char.IsDigit).ToArray());
                    tableNumber = !string.IsNullOrEmpty(digits) ? $"Table {digits}" : dto.TableName;
                }

                Enum.TryParse(dto.OrderType, true, out OrderType parsedOrderType);

                // 2. Create Customer Order
                var order = new CustomerOrder
                {
                    CustomerName = dto.CustomerName,
                    OrderType = parsedOrderType,
                    TableNumber = tableNumber,
                    OrderTime = DateTime.Now,
                    OrderItems = dto.Items.Select(item => new OrderItem
                    {
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };

                _context.CustomerOrders.Add(order);
                _context.SaveChanges(); // Generates OrderId

                // 3. Create Kitchen Ticket
                var chef = _context.Employees.Where(e => e.Role == "Chef" & e.IsActive).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                var kitchenTicket = new KitchenTicket
                {
                    OrderId = order.OrderId,
                    TicketStatus = TicketStatus.QUEUED,

                    AssignedChef = chef?.Name,
                    StartTime = DateTime.Now

                };

                _context.KitchenTickets.Add(kitchenTicket);

                // 4. Create Bill Invoice
                var billInvoice = new BillInvoice
                {
                    OrderId = order.OrderId,
                    TotalAmount = dto.TotalAmount,
                    TaxAmount = dto.TotalAmount * 0.05m,
                    TipAmount = 0,
                    SubtotalAmount = dto.TotalAmount + dto.TotalAmount * 0.05m,
                    PaymentStatus = PaymentStatus.PENDING
                };

                _context.BillInvoices.Add(billInvoice);

                _context.SaveChanges();

                return Json(new { success = true, message = "Order successfully created!" });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Debug.WriteLine($"[PlaceOrder Exception]: {ex}");

                return Json(new { success = false, message = $"Database Error: {innerMsg}" });
            }
        }



        [HttpGet]
        public IActionResult OrderMonitoring()
        {
            if (_context.KitchenTickets == null)
            {
                return View("WaiterOrderMonitoring", new WaiterOrderMonitoringViewModel());
            }

            var inProgressList = GetOrdersByStatus(TicketStatus.IN_PROGRESS, "IN_PROGRESS");
            var readyList = GetOrdersByStatus(TicketStatus.READY, "READY");
            var servedList = GetOrdersByStatus(TicketStatus.SERVED, "SERVED");

            var viewModel = new WaiterOrderMonitoringViewModel
            {
                DeployedOrders = inProgressList,
                DeployedOrderCount = inProgressList.Count,

                DelayedOrders = readyList,
                DelayedOrderCount = readyList.Count,

                ServedOrders = servedList,
                ServedOrderCount = servedList.Count
            };

            return View("WaiterOrderMonitoring", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkServed(int ticketId)
        {
            var ticket = _context.KitchenTickets?.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.TicketStatus = TicketStatus.SERVED;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(OrderMonitoring));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrder(int ticketId)
        {
            var ticket = _context.KitchenTickets
                .Include(t => t.CustomerOrder)
                    .ThenInclude(o => o!.OrderItems)
                .Include(t => t.CustomerOrder)
                    .ThenInclude(o => o!.BillInvoice)
                .FirstOrDefault(t => t.TicketId == ticketId);

            if (ticket != null)
            {
                var customerOrder = ticket.CustomerOrder;

                // 1. Remove Kitchen Ticket
                _context.KitchenTickets.Remove(ticket);

                // 2. Remove Order items, Bill, and Customer Order
                if (customerOrder != null)
                {
                    if (customerOrder.BillInvoice != null)
                    {
                        _context.BillInvoices.Remove(customerOrder.BillInvoice);
                    }

                    if (customerOrder.OrderItems != null && customerOrder.OrderItems.Any())
                    {
                        _context.OrderItems.RemoveRange(customerOrder.OrderItems);
                    }

                    _context.CustomerOrders.Remove(customerOrder);
                }

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(OrderMonitoring));
        }


        private List<WaiterOrderMonitoringItemViewModel> GetOrdersByStatus(TicketStatus ticketStatus, string statusString)
        {
            return _context.KitchenTickets
                .Include(t => t.CustomerOrder)
                    .ThenInclude(o => o.BillInvoice)
                .Where(t => t.TicketStatus == ticketStatus)
                .Select(t => new WaiterOrderMonitoringItemViewModel
                {
                    OrderId = t.OrderId,
                    KitchenTicketId = t.TicketId,
                    TableNumber = t.CustomerOrder != null && !string.IsNullOrEmpty(t.CustomerOrder.TableNumber)
                        ? t.CustomerOrder.TableNumber
                        : "N/A",
                    CustomerName = !string.IsNullOrEmpty(t.CustomerOrder != null ? t.CustomerOrder.CustomerName : null)
                        ? t.CustomerOrder!.CustomerName
                        : "Guest",
                    Status = statusString,
                    OrderStatus = statusString,
                    OrderTime = t.CustomerOrder != null ? t.CustomerOrder.OrderTime : DateTime.Now,
                    TotalAmount = t.CustomerOrder != null && t.CustomerOrder.BillInvoice != null
                        ? t.CustomerOrder.BillInvoice.TotalAmount
                        : 0
                }).ToList();
        }

    }



    public class PlaceOrderDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<PlaceOrderItemDto> Items { get; set; } = new();
    }

    public class PlaceOrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }


}