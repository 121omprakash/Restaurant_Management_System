using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class RestaurantTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(4)]
        public string TableNumber { get; set; } = null!;

        public bool IsOccupied { get; set; }

        public ICollection<CustomerOrder> CustomerOrders { get; set; }
            = new List<CustomerOrder>();
    }
}