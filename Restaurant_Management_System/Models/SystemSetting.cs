using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Models
{
    public class SystemSetting
    {

        [Key]
        public int Id { get; set; }

        // Restaurant Profile Configurations
        [Required]
        [StringLength(100)]
        public string RestaurantName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PrimaryPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CorporateEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PhysicalAddress { get; set; } = string.Empty;

        // Tax Rates & Metrics
        [Required]
        [StringLength(50)]
        public string TaxIdentifier { get; set; } = string.Empty; // GSTIN

        [Required]
        public decimal BaseCgstPercentage { get; set; } // Central Tax

        [Required]
        public decimal BaseSgstPercentage { get; set; } // State Tax

        // Inventory Settings
        [Required]
        public int LowStockThreshold { get; set; }
    }
}

