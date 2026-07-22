using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Restaurant_Management_System.ENUM;
using System.ComponentModel.DataAnnotations;
namespace Restaurant_Management_System.ViewModel
{
    public class MenuItemCreateViewModel
    {
        public int MenuItemId { get; set; }

        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public MenuCategory Category { get; set; } = MenuCategory.Veg;
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public ItemStatus ItemStatus { get; set; } = ItemStatus.AVAILABLE;

        // RecipeSteps/Path left null by default when creating MenuItem
        public string? ImagePath { get; set; }

        // File uploaded from the form
        public IFormFile? ImageFile { get; set; }

        // Ingredients removed from create form per request
    }
}
