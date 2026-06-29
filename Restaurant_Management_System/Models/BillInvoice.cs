namespace Restaurant_Management_System.Models
{
    public class BillInvoice
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TipAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = ""; // PAID, PENDING
    }
}