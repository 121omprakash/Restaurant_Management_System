using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using System.Linq;

namespace Restaurant_Management_System.Services
{
    public class BillingService : IBillingService
    {
        private readonly rmsDbContext _context;

        public BillingService(rmsDbContext context)
        {
            _context = context;
        }

        public CashierDashboardViewModel GetDashboardData(string currentTab, int? selectedOrderId)
        {
            CashierDashboardViewModel dashboardData = new CashierDashboardViewModel();

            dashboardData.PendingBillsCount =
                _context.BillInvoices.Count(b => b.PaymentStatus == PaymentStatus.PENDING);

            dashboardData.SettledTodayCount =
                _context.BillInvoices.Count(b => b.PaymentStatus == PaymentStatus.PAID);

            dashboardData.TipsCollectedToday =
                _context.BillInvoices
                    .Where(b => b.PaymentStatus == PaymentStatus.PAID)
                    .Sum(b => (decimal?)b.TipAmount) ?? 0;

            dashboardData.TotalRevenueToday =
                _context.BillInvoices
                    .Where(b => b.PaymentStatus == PaymentStatus.PAID)
                    .Sum(b => (decimal?)b.TotalAmount) ?? 0;

            var allDbOrders = _context.CustomerOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.BillInvoice)
                .ToList();

            foreach (var order in allDbOrders)
            {
                if (currentTab == "PENDING" &&
                    (order.OrderStatus == OrderStatus.READY ||
                     order.OrderStatus == OrderStatus.PREPARING))
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "SETTLED" &&
                    order.OrderStatus == OrderStatus.SERVED)
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "TIPS" &&
                    order.BillInvoice?.TipAmount > 0)
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "SALES" &&
                    order.OrderStatus == OrderStatus.SERVED)
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "ALL")
                {
                    dashboardData.ActiveOrders.Add(order);
                }
            }

            if (selectedOrderId.HasValue)
            {
                var selectedOrder = allDbOrders
                    .FirstOrDefault(o => o.OrderId == selectedOrderId.Value);

                if (selectedOrder != null)
                {
                    dashboardData.ShowDrillDown = true;

                    dashboardData.DrillCustomer =
                        $"{selectedOrder.CustomerName} (Order #{selectedOrder.OrderId})";

                    dashboardData.DrillTable =
                        $"Assigned Context Location: {selectedOrder.TableNumber}";

                    var itemsList = selectedOrder.OrderItems
                        .Select(oi =>
                            $"{oi.MenuItem?.ItemName ?? "Item"} (Qty: {oi.Quantity})");

                    dashboardData.DrillItems = itemsList.Any()
                        ? string.Join("<br/>", itemsList)
                        : "No items found.";

                    dashboardData.DrillTip =
                        selectedOrder.BillInvoice?.TipAmount.ToString("C") ?? "0.00";

                    dashboardData.DrillTotal =
                        selectedOrder.BillInvoice?.TotalAmount.ToString("C") ?? "0.00";
                }
            }

            return dashboardData;
        }

        public SettlementViewModel GetSettlementData(int orderId)
        {
            var selectedOrder = _context.CustomerOrders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (selectedOrder == null)
            {
                return null;
            }

            decimal subtotal =
                selectedOrder.OrderItems.Sum(oi => oi.Price * oi.Quantity);

            decimal tax = subtotal * 0.0825m;

            return new SettlementViewModel
            {
                Order = selectedOrder,
                SubtotalAmount = subtotal,
                TaxAmount = tax,
                TotalAmount = subtotal + tax
            };
        }

        public BillInvoice CompleteSettlement(int orderId, decimal tipAmount)
        {
            var order = _context.CustomerOrders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return null;
            }

            decimal subtotal = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            decimal tax = subtotal * 0.0825m;

            BillInvoice completedInvoice = new BillInvoice
            {
                OrderId = orderId,
                SubtotalAmount = subtotal,
                TaxAmount = tax,
                TipAmount = tipAmount,
                TotalAmount = subtotal + tax + tipAmount,
                PaymentStatus = PaymentStatus.PAID
            };

            _context.BillInvoices.Add(completedInvoice);

            // Update Order Status
            order.OrderStatus = OrderStatus.SERVED;

            // Free the table
            var table = _context.RestaurantTables
                .FirstOrDefault(t => t.TableNumber == order.TableNumber);

            if (table != null)
            {
                table.IsOccupied = false;
            }

            _context.SaveChanges();

            return completedInvoice;
        }

        public BillInvoice? GetInvoiceById(int invoiceId)
        {
            return _context.BillInvoices
                .FirstOrDefault(b => b.InvoiceId == invoiceId);
        }
    }
}