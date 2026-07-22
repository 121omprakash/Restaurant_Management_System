using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace Restaurant_Management_System.Models
{
    public class Employee
    {
       
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string EmpId { get; set; } = null!;
        [Required]
        public string password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
