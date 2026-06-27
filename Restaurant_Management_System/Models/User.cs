using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace Restaurant_Management_System.Models
{
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [NotNull]
        public string Name { get; set; }

        [Required]
        [NotNull]
        public string UserId { get; set; }
        [Required]
        [NotNull]
        public string password { get; set; }
        [Required]
        [NotNull]
        public string Role { get; set; }
     //   [Required]
        //public string Status { get; set; } = "Active";
    }
}
