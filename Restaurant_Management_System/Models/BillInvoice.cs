using Restaurant_Management_System.ENUM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class BillInvoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal SubtotalAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TipAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public CustomerOrder CustomerOrder { get; set; } = null!;
    }
}