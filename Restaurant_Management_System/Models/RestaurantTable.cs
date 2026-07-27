using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class RestaurantTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(50)]
        public string TableNumber { get; set; } = string.Empty;

        public bool IsOccupied { get; set; }

        public ICollection<CustomerOrder> CustomerOrders { get; set; }
            = new List<CustomerOrder>();
    }
}