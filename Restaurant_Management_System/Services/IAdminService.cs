using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Services
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetEmployeeDashboardDataAsync();
        Task<(bool Success, string Message)> CreateEmployeeAsync(EmployeeViewModel employee);
        Task<(bool Success, string Message)> UpdateEmployeeAsync(EmployeeViewModel updatedEmployee);
        Task<(bool Success, string Message)> ToggleEmployeeStatusAsync(int id);
        Task<RestaurantProfile> GetRestaurantProfileAsync();
        Task<(bool Success, string Message)> UpdateRestaurantProfileAsync(RestaurantProfile updatedProfile);
    }
}