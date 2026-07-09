using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{

    public enum TicketStatus
    { 
       Queued,
       InProgress,
       Ready,
       Served
    }
    public class KitchenTicket
    {
        
        [Key]
        public int TicketId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string Station { get; set; }

        [StringLength(100)]
        public string AssignedChef { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public  TicketStatus TicketStatus { get; set; }
        
        [ForeignKey(nameof(OrderId))]
        public CustomerOrder CustomerOrder { get; set; }

    }
}
