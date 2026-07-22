using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.ViewModel
{
    public class AdminDashboardViewModel
    {
        public int TotalProfiles { get; set; }
        public int ActiveProfiles { get; set; }
        public int InactiveProfiles { get; set; }
        public int ManagementStaff { get; set; }
        public int OperationalStaff { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
