using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class InventoryItemViewModel
    {
        public Ingredient Ingredient { get; set; } = new Ingredient();

        // Simple role info for view adjustments (e.g., show/hide controls)
        public string UserRole { get; set; } = string.Empty;
    }
}
