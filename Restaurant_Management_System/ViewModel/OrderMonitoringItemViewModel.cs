namespace Restaurant_Management_System.ViewModel
{
    public class OrderMonitoringItemViewModel
    {
        public int KitchenTicketId { get; set; }

        public int OrderId { get; set; }

        public string TableNumber { get; set; }

        public string CustomerName { get; set; }

        public string OrderType { get; set; }

        public int TotalItems { get; set; }

        public DateTime OrderTime { get; set; }

        public string Status
        {
            get; set;

        }
    }
}
