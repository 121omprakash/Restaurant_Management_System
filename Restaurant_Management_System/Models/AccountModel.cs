using System.ComponentModel.DataAnnotations;
namespace Restaurant_Management_System.Models
{
    public class AccountModel
    {
        [Required]
        public string AccountId { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}
