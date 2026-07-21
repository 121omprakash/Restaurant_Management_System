using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly rmsDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(rmsDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

      

        // Employee Management Dashboard
       

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            ViewBag.TotalProfiles = await _context.Employees.CountAsync();
            ViewBag.ActiveProfiles = await _context.Employees.CountAsync(e => e.IsActive);
            ViewBag.InactiveProfiles = await _context.Employees.CountAsync(e => !e.IsActive);
            ViewBag.ManagementStaff = await _context.Employees.CountAsync(e => e.IsActive && (e.Role == "Admin" || e.Role == "Manager"));
            ViewBag.OperationalStaff = await _context.Employees
                .CountAsync(e => e.IsActive && (e.Role == "Chef" || e.Role == "Waiter" || e.Role == "Cashier" || e.Role == "Inventory Clerk"));

            var employees = await _context.Employees.ToListAsync();

            return View(employees);
        }

        // =====================================================
        // Create Employee
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please enter valid employee details.";
                return RedirectToAction(nameof(Employees));
            }

            try
            {
                // 1. Only one administrator account is allowed
                if (employee.Role == "Admin" &&
                    await _context.Employees.AnyAsync(e => e.Role == "Admin"))
                {
                    TempData["Message"] = "Error: An Administrator account already exists.";
                    return RedirectToAction(nameof(Employees));
                }

                // 2. Exact prefix mapping matching your rmsDbContext seed data
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

                // 3. Get all existing IDs starting with this prefix
                var existingEmpIds = await _context.Employees
                    .Where(e => e.EmpId.StartsWith(prefix))
                    .Select(e => e.EmpId)
                    .ToListAsync();

                // 4. Find the highest numeric value (e.g., from CH01, CH02, CH05 -> max is 5)
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

                // 5. Generate next unique ID (e.g., max 5 + 1 = 6 -> "CH06")
                string empId = $"{prefix}{(maxNumber + 1):D2}";

                var newEmployee = new Employee
                {
                    Name = employee.Name,
                    password = employee.Password, // Ensure property name casing matches your Employee model
                    Role = employee.Role,
                    EmpId = empId,
                    IsActive = true
                };

                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Employee created successfully. Employee ID: {newEmployee.EmpId}";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating employee.");
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx &&
                    (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    TempData["Message"] = "Error: An employee with this ID already exists.";
                }
                else
                {
                    TempData["Message"] = "Database error occurred while saving employee.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating employee.");
                TempData["Message"] = "Something went wrong.";
            }

            return RedirectToAction(nameof(Employees));
        }

        // Update Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel updatedEmployee)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(updatedEmployee.Id);

                if (employee == null)
                {
                    TempData["Message"] = "Employee not found.";
                    return RedirectToAction(nameof(Employees));
                }

                if (!employee.IsActive)
                {
                    TempData["Message"] = "Action denied. Reactivate the employee before editing.";
                    return RedirectToAction(nameof(Employees));
                }

                if (employee.Role == "Admin")
                {
                    TempData["Message"] = "Administrator account cannot be modified here.";
                    return RedirectToAction(nameof(Employees));
                }

                // Update core details
                employee.Name = updatedEmployee.Name;
                employee.Role = updatedEmployee.Role;

                
                if (!string.IsNullOrWhiteSpace(updatedEmployee.Password))
                {
                    employee.password = updatedEmployee.Password; 
                }

                await _context.SaveChangesAsync();

                TempData["Message"] = "Employee updated successfully.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating employee.");
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx &&
                    (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    TempData["Message"] = "Error: An employee with this ID or unique detail already exists.";
                }
                else
                {
                    TempData["Message"] = "Database error occurred while saving employee.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating employee.");
                TempData["Message"] = "Something went wrong.";
            }

            return RedirectToAction(nameof(Employees));
        }

        // Activate / Deactivate Employee

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    TempData["Message"] = "Employee not found.";
                    return RedirectToAction(nameof(Employees));
                }

                if (employee.Role == "Admin")
                {
                    TempData["Message"] = "Administrator account cannot be deactivated.";
                    return RedirectToAction(nameof(Employees));
                }

                employee.IsActive = !employee.IsActive;

                await _context.SaveChangesAsync();

                TempData["Message"] = employee.IsActive
                    ? "Employee activated successfully."
                    : "Employee deactivated successfully.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while changing employee status.");
                TempData["Message"] = "Unable to update employee status.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while changing employee status.");
                TempData["Message"] = "Something went wrong.";
            }

            return RedirectToAction(nameof(Employees));
        }

      
        // Restaurant Profile

        [HttpGet]
        public async Task<IActionResult> RestaurantProfile()
        {
            var details = await _context.RestaurantProfiles
                .FirstOrDefaultAsync(r => r.Id == 1);

            return View(details ?? new RestaurantProfile { Id = 1 });
        }

       
        // Update Restaurant Profile

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRestaurantProfile(RestaurantProfile updatedProfile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please enter valid details.";
                return RedirectToAction(nameof(RestaurantProfile));
            }

            try
            {
                var profile = await _context.RestaurantProfiles
                    .FirstOrDefaultAsync(r => r.Id == 1);

                if (profile == null)
                {
                    TempData["Message"] = "Restaurant profile not found.";
                    return RedirectToAction(nameof(RestaurantProfile));
                }

                profile.RestaurantName = updatedProfile.RestaurantName;
                profile.PrimaryPhone = updatedProfile.PrimaryPhone;
                profile.CorporateEmail = updatedProfile.CorporateEmail;
                profile.PhysicalAddress = updatedProfile.PhysicalAddress;
                profile.TaxIdentifier = updatedProfile.TaxIdentifier;
                profile.BaseCgstPercentage = updatedProfile.BaseCgstPercentage;
                profile.BaseSgstPercentage = updatedProfile.BaseSgstPercentage;

                await _context.SaveChangesAsync();

                TempData["Message"] = "Restaurant profile updated successfully.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating restaurant profile.");
                TempData["Message"] = "Unable to update restaurant profile.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating restaurant profile.");
                TempData["Message"] = "Something went wrong.";
            }

            return RedirectToAction(nameof(RestaurantProfile));
        }
    }
}