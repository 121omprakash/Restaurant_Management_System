using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace Restaurant_Management_System.Models
{
    public class Employee
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmpId { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string Role { get; set; }
     //   [Required]
        //public string Status { get; set; } = "Active";
    }
}
