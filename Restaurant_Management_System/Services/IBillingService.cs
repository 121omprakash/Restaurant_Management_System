using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Services
{
    public interface IBillingService
    {
        CashierDashboardViewModel GetDashboardData(string currentTab, int? selectedOrderId);
        SettlementViewModel GetSettlementData(int orderId);
        BillInvoice CompleteSettlement(int orderId, decimal tipAmount);
        BillInvoice? GetInvoiceById(int invoiceId);
    }
}
