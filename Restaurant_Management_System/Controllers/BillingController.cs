using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Services;

namespace Restaurant_Management_System.Controllers
{
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        public IActionResult Index(string currentTab = "ALL", int? selectedOrderId = null)
        {
            var dashboardData =
                _billingService.GetDashboardData(currentTab, selectedOrderId);

            ViewData["ActiveTab"] = currentTab;

            return View(dashboardData);
        }

        public IActionResult ProcessPayment(int orderId)
        {
            var checkoutData =
                _billingService.GetSettlementData(orderId);

            if (checkoutData == null)
            {
                return NotFound();
            }

            return View(checkoutData);
        }

        [HttpPost]
        public IActionResult CompleteSettlement(int orderId, decimal tipAmount)
        {
            var completedInvoice =
                _billingService.CompleteSettlement(orderId, tipAmount);

            if (completedInvoice == null)
            {
                return NotFound();
            }

            return View("Invoice", completedInvoice);
        }

        // GET: Billing/Invoice?invoiceId=123
        public IActionResult Invoice(int invoiceId)
        {
            var invoice = _billingService.GetInvoiceById(invoiceId);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
    }
}