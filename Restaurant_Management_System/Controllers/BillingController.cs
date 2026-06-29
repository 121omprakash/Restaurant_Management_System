using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using System;
using System.Collections.Generic;

namespace Restaurant_Management_System.Controllers
{
    public class BillingController : Controller
    {
        // GET: /Billing?currentTab=ALL
        // GET: /Billing?currentTab=PENDING
        public IActionResult Index(string currentTab = "ALL", int? selectedOrderId = null)
        {
            // Initialize main ViewModel container
            CashierDashboardViewModel dashboardData = new CashierDashboardViewModel();
            dashboardData.PendingBillsCount = 2;
            dashboardData.SettledTodayCount = 2;
            dashboardData.TipsCollectedToday = 25.00m;
            dashboardData.TotalRevenueToday = 309.68m;

            // 1. Build out ALL raw mock data elements
            List<CustomerOrder> allMockOrders = new List<CustomerOrder>
            {
                new CustomerOrder { OrderId = 1024, TableNumber = "Table 5", CustomerName = "John Doe", OrderType = "DINE_IN", OrderTime = DateTime.Now.AddMinutes(-30), OrderStatus = "READY" },
                new CustomerOrder { OrderId = 1025, TableNumber = "Takeaway", CustomerName = "Jane Smith", OrderType = "TAKEAWAY", OrderTime = DateTime.Now.AddMinutes(-10), OrderStatus = "PREPARING" },
                new CustomerOrder { OrderId = 9012, TableNumber = "Table 2", CustomerName = "Robert Downey", OrderType = "DINE_IN", OrderTime = DateTime.Now.AddHours(-2), OrderStatus = "SERVED" }, // Mapped as SETTLED/PAID
                new CustomerOrder { OrderId = 9013, TableNumber = "Table 8", CustomerName = "Alice Cooper", OrderType = "DINE_IN", OrderTime = DateTime.Now.AddHours(-1), OrderStatus = "SERVED" }  // Mapped as SETTLED + TIPS
            };

            // 2. Perform Filtering strictly in C# based on the currentTab parameter
            foreach (var order in allMockOrders)
            {
                if (currentTab == "PENDING" && (order.OrderStatus == "READY" || order.OrderStatus == "PREPARING"))
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "SETTLED" && order.OrderStatus == "SERVED")
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "TIPS" && order.OrderId == 9013) // Custom tip item simulation
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "SALES" && order.OrderStatus == "SERVED")
                {
                    dashboardData.ActiveOrders.Add(order);
                }
                else if (currentTab == "ALL")
                {
                    dashboardData.ActiveOrders.Add(order);
                }
            }

            // Keep track of the active tab view inside standard ViewData storage
            ViewData["ActiveTab"] = currentTab;

            // 3. Drill-Down Processing logic: Check if user clicked a specific row item link
            ViewData["ShowDrillDown"] = "FALSE";
            if (selectedOrderId != null)
            {
                ViewData["ShowDrillDown"] = "TRUE";
                if (selectedOrderId == 1024)
                {
                    ViewData["DrillCustomer"] = "John Doe (Order #1024)";
                    ViewData["DrillTable"] = "Assigned Context Location: Table 5";
                    ViewData["DrillItems"] = "Grilled Salmon (Qty: 2)<br/>Truffle Fries (Qty: 1)";
                    ViewData["DrillTip"] = "$0.00";
                    ViewData["DrillTotal"] = "$87.68";
                }
                else if (selectedOrderId == 1025)
                {
                    ViewData["DrillCustomer"] = "Jane Smith (Order #1025)";
                    ViewData["DrillTable"] = "Assigned Context Location: Takeaway";
                    ViewData["DrillItems"] = "Margherita Pizza (Qty: 1)<br/>Soda Drink (Qty: 2)";
                    ViewData["DrillTip"] = "$0.00";
                    ViewData["DrillTotal"] = "$45.50";
                }
                else if (selectedOrderId == 9012)
                {
                    ViewData["DrillCustomer"] = "Robert Downey (Order #9012)";
                    ViewData["DrillTable"] = "Assigned Context Location: Table 2";
                    ViewData["DrillItems"] = "Ribeye Steak (Qty: 1)<br/>Red Wine (Qty: 2)";
                    ViewData["DrillTip"] = "$15.00";
                    ViewData["DrillTotal"] = "$114.50";
                }
                else if (selectedOrderId == 9013)
                {
                    ViewData["DrillCustomer"] = "Alice Cooper (Order #9013)";
                    ViewData["DrillTable"] = "Assigned Context Location: Table 8";
                    ViewData["DrillItems"] = "Caesar Salad (Qty: 1)<br/>Iced Tea (Qty: 2)";
                    ViewData["DrillTip"] = "$10.00";
                    ViewData["DrillTotal"] = "$62.18";
                }
            }

            return View(dashboardData);
        }

        // ProcessPayment and CompleteSettlement target methods stay exactly the same...
        public IActionResult ProcessPayment(int orderId)
        {
            CustomerOrder selectedOrder = new CustomerOrder { OrderId = orderId, CustomerName = "Prototype Customer", TableNumber = "Table 1" };
            SettlementViewModel checkoutData = new SettlementViewModel { Order = selectedOrder, SubtotalAmount = 81.00m, TaxAmount = 6.68m, TotalAmount = 87.68m };
            return View(checkoutData);
        }

        [HttpPost]
        public IActionResult CompleteSettlement(int orderId, decimal tipAmount)
        {
            // 1. Create your invoice data
            BillInvoice completedInvoice = new BillInvoice
            {
                InvoiceId = 55432,
                OrderId = orderId,
                SubtotalAmount = 81.00m,
                TaxAmount = 6.68m,
                TipAmount = tipAmount,
                TotalAmount = 87.68m + tipAmount,
                PaymentStatus = "PAID"
            };

            // 2. Map the BillInvoice data to the SettlementViewModel
            // (Note: You may want to look up the actual order from your database here)
            SettlementViewModel viewModel = new SettlementViewModel
            {
                Order = new CustomerOrder { OrderId = orderId }, // Populating the required Order object
                SubtotalAmount = completedInvoice.SubtotalAmount,
                TaxAmount = completedInvoice.TaxAmount,
                TipAmount = completedInvoice.TipAmount,
                TotalAmount = completedInvoice.TotalAmount
            };

            // 3. Pass the ViewModel to the view
            return View("Invoice", completedInvoice);
        }
    }
}