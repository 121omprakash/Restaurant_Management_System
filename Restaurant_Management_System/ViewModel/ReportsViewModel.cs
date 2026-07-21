using System;
using System.Collections.Generic;

namespace Restaurant_Management_System.ViewModel
{
    public class ReportsViewModel
    {
        public decimal TodaysRevenue { get; set; }

        public decimal ThisWeekRevenue { get; set; }

        public decimal ThisMonthRevenue { get; set; }

        public decimal AverageBillValue { get; set; }

        // Simple revenue trend numbers (e.g. last 7 days)
        public List<decimal> RevenueTrend { get; set; } = new();

        public List<TopSellingItem> TopSellingItems { get; set; } = new();

        public List<RecentInvoice> RecentInvoices { get; set; } = new();
    }

    public class TopSellingItem
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Percentage { get; set; }
    }

    public class RecentInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceLabel { get; set; } = string.Empty; // e.g. INV-1001
        public DateTime Date { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
