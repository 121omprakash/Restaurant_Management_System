using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Employee name is required.")]
        public string Name { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
