using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Services
{
    public class AdminService : IAdminService
    {
        private readonly rmsDbContext _context;
        private readonly ILogger<AdminService> _logger;
        private readonly IPasswordHasher<Employee> _passwordHasher;

        public AdminService(rmsDbContext context, ILogger<AdminService> logger, IPasswordHasher<Employee> passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<AdminDashboardViewModel> GetEmployeeDashboardDataAsync()
        {
            var dashboard = new AdminDashboardViewModel
            {
                TotalProfiles = await _context.Employees.CountAsync(),
                ActiveProfiles = await _context.Employees.CountAsync(e => e.IsActive),
                InactiveProfiles = await _context.Employees.CountAsync(e => !e.IsActive),
                ManagementStaff = await _context.Employees.CountAsync(e => e.IsActive && (e.Role == "Admin" || e.Role == "Manager")),
                OperationalStaff = await _context.Employees.CountAsync(e => e.IsActive && (e.Role == "Chef" || e.Role == "Waiter" || e.Role == "Cashier" || e.Role == "Inventory Clerk")),
                Employees = await _context.Employees.ToListAsync()
            };

            return dashboard;
        }

        public async Task<(bool Success, string Message)> CreateEmployeeAsync(EmployeeViewModel employee)
        {
            try
            {
                // 1. Auto-create Admin if no Admin exists in the system yet
                if (!await _context.Employees.AnyAsync(e => e.Role == "Admin" || e.EmpId == "AD01"))
                {
                    var defaultAdmin = new Employee
                    {
                        Name = "Shaik",
                        EmpId = "AD01",
                        Role = "Admin",
                        IsActive = true
                    };
                    defaultAdmin.password = _passwordHasher.HashPassword(defaultAdmin, "AD01@123");

                    _context.Employees.Add(defaultAdmin);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Default Admin (AD01) created automatically.");
                }

                // 2. Prevent creating a SECOND Admin account
                if (employee.Role == "Admin" && await _context.Employees.AnyAsync(e => e.Role == "Admin"))
                {
                    return (false, "Error: An Administrator account already exists.");
                }

                // 3. Exact prefix mapping matching seed data
                string prefix = employee.Role switch
                {
                    "Admin" => "AD",
                    "Chef" => "CH",
                    "Manager" => "MN",
                    "Waiter" => "WA",
                    "Cashier" => "CS",
                    "Inventory Clerk" => "IC",
                    _ => "ST"
                };

                // 4. Get all existing IDs starting with this prefix
                var existingEmpIds = await _context.Employees
                    .Where(e => e.EmpId.StartsWith(prefix))
                    .Select(e => e.EmpId)
                    .ToListAsync();

                // 5. Find highest numeric value
                int maxNumber = 0;
                foreach (var id in existingEmpIds)
                {
                    if (id.Length > prefix.Length)
                    {
                        string numericPart = id.Substring(prefix.Length);
                        if (int.TryParse(numericPart, out int parsedNum))
                        {
                            if (parsedNum > maxNumber)
                            {
                                maxNumber = parsedNum;
                            }
                        }
                    }
                }

                // 6. Generate next unique ID
                string empId = $"{prefix}{(maxNumber + 1):D2}";

                var newEmployee = new Employee
                {
                    Name = employee.Name,
                    Role = employee.Role,
                    EmpId = empId,
                    IsActive = true
                };
                newEmployee.password = _passwordHasher.HashPassword(newEmployee, employee.Password);

                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                return (true, $"Employee created successfully. Employee ID: {newEmployee.EmpId}");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating employee.");
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx &&
                    (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    return (false, "Error: An employee with this ID already exists.");
                }
                return (false, "Database error occurred while saving employee.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating employee.");
                return (false, "Something went wrong.");
            }
        }

        public async Task<(bool Success, string Message)> UpdateEmployeeAsync(EmployeeViewModel updatedEmployee)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(updatedEmployee.Id);

                if (employee == null)
                {
                    return (false, "Employee not found.");
                }

                if (!employee.IsActive)
                {
                    return (false, "Action denied. Reactivate the employee before editing.");
                }

                if (employee.Role == "Admin")
                {
                    return (false, "Administrator account cannot be modified here.");
                }

                // Update core details
                employee.Name = updatedEmployee.Name;
                employee.Role = updatedEmployee.Role;

                if (!string.IsNullOrWhiteSpace(updatedEmployee.Password))
                {
                    employee.password = _passwordHasher.HashPassword(employee, updatedEmployee.Password);
                }
            

                await _context.SaveChangesAsync();
                return (true, "Employee updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating employee.");
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx &&
                    (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    return (false, "Error: An employee with this ID or unique detail already exists.");
                }
                return (false, "Database error occurred while saving employee.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating employee.");
                return (false, "Something went wrong.");
            }
        }

        public async Task<(bool Success, string Message)> ToggleEmployeeStatusAsync(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return (false, "Employee not found.");
                }

                if (employee.Role == "Admin")
                {
                    return (false, "Administrator account cannot be deactivated.");
                }

                employee.IsActive = !employee.IsActive;
                await _context.SaveChangesAsync();

                string msg = employee.IsActive
                    ? "Employee activated successfully."
                    : "Employee deactivated successfully.";

                return (true, msg);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while changing employee status.");
                return (false, "Unable to update employee status.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while changing employee status.");
                return (false, "Something went wrong.");
            }
        }

        public async Task<RestaurantProfile> GetRestaurantProfileAsync()
        {
            var details = await _context.RestaurantProfiles
                .FirstOrDefaultAsync(r => r.Id == 1);

            return details ?? new RestaurantProfile { Id = 1 };
        }

        public async Task<(bool Success, string Message)> UpdateRestaurantProfileAsync(RestaurantProfile updatedProfile)
        {
            try
            {
                var profile = await _context.RestaurantProfiles
                    .FirstOrDefaultAsync(r => r.Id == 1);

                if (profile == null)
                {
                    return (false, "Restaurant profile not found.");
                }

                profile.RestaurantName = updatedProfile.RestaurantName;
                profile.PrimaryPhone = updatedProfile.PrimaryPhone;
                profile.CorporateEmail = updatedProfile.CorporateEmail;
                profile.PhysicalAddress = updatedProfile.PhysicalAddress;
                profile.TaxIdentifier = updatedProfile.TaxIdentifier;
                profile.BaseCgstPercentage = updatedProfile.BaseCgstPercentage;
                profile.BaseSgstPercentage = updatedProfile.BaseSgstPercentage;

                await _context.SaveChangesAsync();
                return (true, "Restaurant profile updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating restaurant profile.");
                return (false, "Unable to update restaurant profile.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating restaurant profile.");
                return (false, "Something went wrong.");
            }
        }
    }
}