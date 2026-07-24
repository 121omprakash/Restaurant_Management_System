namespace Restaurant_Management_System.ViewModels
{
    public class MenuItemViewModel
    {
        public int MenuItemId { get; set; }

        public string ItemName { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public string? RecipeSteps { get; set; }

        public int PreparationTime { get; set; }
    }
}
