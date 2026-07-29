using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.DTO;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Services;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers.Api
{
    [ApiController]
    [Route("admin")]
   
    public class AdminApiController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly rmsDbContext _context;

        public AdminApiController(IAdminService adminService, rmsDbContext context)
        {
            _adminService = adminService;
            _context = context;
        }

        /// <summary>
        /// Retrieves dashboard summary statistics and active employees.
        /// </summary>

        // GET: api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _adminService.GetEmployeeDashboardDataAsync();

            var response = new AdminDashboardResponseDto
            {
                TotalProfiles = data.TotalProfiles,
                ActiveProfiles = data.ActiveProfiles,
                InactiveProfiles = data.InactiveProfiles,
                ManagementStaff = data.ManagementStaff,
                OperationalStaff = data.OperationalStaff,
                Employees = data.Employees.Select(e => new EmployeeResponseDto
                {
                    EmpId = e.EmpId,
                    Name = e.Name,
                    Role = e.Role,
                    IsActive = e.IsActive
                }).ToList()
            };

            return Ok(response);
        }

        /// <summary>
        /// Creates a new employee (Manager, Waiter, Chef, Cashier, or Inventory Clerk).
        ///</summary>

        // POST: api/admin/employees
        [HttpPost("employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            // Catches invalid roles (e.g., random role inputs) before service call
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new EmployeeViewModel
            {
                Name = dto.Name,
                Role = dto.Role,
                Password = dto.Password
            };

            var (success, message) = await _adminService.CreateEmployeeAsync(model);
            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message });
        }


        /// <summary>
        /// Updates existing employee information using their EmpId.
        /// </summary>
        // PUT: api/admin/employees
        [HttpPut("employees")]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Look up by EmpId 
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == dto.EmpId);
            if (employee == null)
            {
                return NotFound(new { message = $"Employee with EmpId '{dto.EmpId}' was not found." });
            }

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = dto.Name,
                Role = dto.Role,
                Password = dto.Password
            };

            var (success, message) = await _adminService.UpdateEmployeeAsync(model);
            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message });
        }

        /// <summary>
        /// Toggles an employee's status between active and inactive.
        /// </summary>
        // PATCH: api/admin/employees/CH01/toggle-status
        [HttpPatch("employees/{empId}/toggle-status")]
        public async Task<IActionResult> ToggleEmployeeStatus(string empId)
        {
            // Look up by EmpId 
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
            if (employee == null)
            {
                return NotFound(new { message = $"Employee with EmpId '{empId}' was not found." });
            }

            var (success, message) = await _adminService.ToggleEmployeeStatusAsync(employee.Id);
            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message });
        }

        /// <summary>
        /// Retrieves the restaurant's profile information.
        /// </summary>

        // GET: api/admin/restaurant-profile
        [HttpGet("restaurant-profile")]
        public async Task<IActionResult> GetRestaurantProfile()
        {
            var profile = await _adminService.GetRestaurantProfileAsync();
            return Ok(profile);
        }


        /// <summary>
        /// Updates the restaurant's profile information.
        /// </summary>
        // PUT: api/admin/restaurant-profile
        [HttpPut("restaurant-profile")]
        public async Task<IActionResult> UpdateRestaurantProfile([FromBody] RestaurantProfile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message) = await _adminService.UpdateRestaurantProfileAsync(profile);
            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message });
        }
    }
}