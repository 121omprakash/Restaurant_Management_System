using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class TableManagementVM
    {
        public List<RestaurantTable> AvailableTables { get; set; } = new();

        public List<RestaurantTable> OccupiedTables { get; set; } = new();
    }
}
