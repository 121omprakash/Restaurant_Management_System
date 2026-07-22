namespace Restaurant_Management_System.ViewModel
{
    public class ManagerInventoryViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal CurrentStock { get; set; }
        public string Status { get; set; } = null!;
    }
}
