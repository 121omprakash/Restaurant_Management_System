using Restaurant_Management_System.Models;
using System.Collections.Generic;

namespace Restaurant_Management_System.ViewModel
{
    public class InventoryListViewModel
    {
        public List<Ingredient> Items { get; set; } = new List<Ingredient>();

        // Role or permission hint for the current user
        public string UserRole { get; set; } = string.Empty;
        // current search term used to filter results
        public string SearchTerm { get; set; } = string.Empty;
    }
}
