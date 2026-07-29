using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.DTO
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(Manager|Waiter|Inventory Clerk|Chef|Cashier)$",
            ErrorMessage = "Invalid Role. Allowed roles: Manager, Waiter, Inventory Clerk, Chef, Cashier.")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateEmployeeDto
    {
        [Required(ErrorMessage = "EmpId is required.")]
        public string EmpId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(Manager|Waiter|Inventory Clerk|Chef|Cashier)$",
            ErrorMessage = "Invalid Role. Allowed roles: Manager, Waiter, Inventory Clerk, Chef, Cashier.")]
        public string Role { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    // --- RESPONSE DTOS (No Passwords, No Database Integer Id) ---

    public class EmployeeResponseDto
    {
        public string EmpId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AdminDashboardResponseDto
    {
        public int TotalProfiles { get; set; }
        public int ActiveProfiles { get; set; }
        public int InactiveProfiles { get; set; }
        public int ManagementStaff { get; set; }
        public int OperationalStaff { get; set; }
        public List<EmployeeResponseDto> Employees { get; set; } = new();
    }

}
