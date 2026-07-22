using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Models.ViewModels;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Services
{
    public interface IKitchenService
    {
        Task<AddRecipeViewModel?> GetRecipeAsync(int menuItemId);

        Task<AddRecipeViewModel?> GetEditRecipeAsync(int menuItemId);

        Task<bool> AddRecipeAsync(AddRecipeViewModel model);

        Task<RecipeManagementViewModel> GetRecipeManagementAsync(string tab);

        Task<AddRecipeViewModel?> GetAddRecipeAsync(int id);

        Task<OrderManagementViewModel> GetOrderManagementAsync(
                        TicketStatus? status,
                        string? search,
                        bool delayed);

        Task<bool> MarkItemPreparedAsync(int id);


        Task<bool> CompleteOrderAsync(int id);

        Task<List<OrderItem>> GetViewItemsAsync(int orderId);



    }
}
