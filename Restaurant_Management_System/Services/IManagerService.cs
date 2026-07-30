using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Services
{
    public interface IManagerService
    {
        Task<DashboardVM> GetDashboardDataAsync();
        Task<List<MenuItem>> GetAllMenuItemsAsync();
        Task<MenuItem?> GetMenuItemWithRecipeAsync(int id);
        Task<MenuItemCreateViewModel?> GetMenuItemForEditAsync(int id);
        Task<(bool Success, string Message)> AddMenuItemAsync(MenuItemCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateMenuItemAsync(MenuItemCreateViewModel vm);
        Task<(bool Success, string Message)> DeleteMenuItemAsync(int id);
        Task<List<OrderMonitoringViewModel>> GetMonitoredOrdersAsync(OrderStatus? status);
        Task<TableManagementVM> GetTableManagementDataAsync();
        Task<InventoryListViewModel> GetInventoryDataAsync(string? search);
        Task<ReportsViewModel> GetReportsDataAsync();
    }
}