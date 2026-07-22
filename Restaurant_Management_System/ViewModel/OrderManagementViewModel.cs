using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class OrderManagementViewModel
    {

        
            public List<KitchenTicket> Tickets { get; set; } = new();

            public int PendingCount { get; set; }
            public int PreparingCount { get; set; }
            public int CompletedCount { get; set; }
            public int DelayedCount { get; set; }

            public string? Search { get; set; }
            public TicketStatus? SelectedStatus { get; set; }
        
    }
}
