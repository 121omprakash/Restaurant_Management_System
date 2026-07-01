using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class SettlementViewModel
    {
        public CustomerOrder Order { get; set; } = new CustomerOrder();
        public decimal SubtotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TipAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}