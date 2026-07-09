namespace Restaurant_Management_Error.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public int ReorderLevel { get; set; }
    }
}
