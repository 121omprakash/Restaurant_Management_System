using Restaurant_Management_System.Models;
namespace Restaurant_Management_System.ViewModel
{
    public class RecipeManagementViewModel
    {
        public List<MenuItem> ExistingRecipes { get; set; } = new();

        public List<MenuItem> PendingRecipes { get; set; } = new();

        public string ActiveTab { get; set; } = "existing";
    }
}
