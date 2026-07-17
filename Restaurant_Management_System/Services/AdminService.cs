//using Restaurant_Management_System.Data;
//using Restaurant_Management_System.Models;

//namespace Restaurant_Management_System.Services
//{
//    public class AdminService : IAdminService
//    {
//        private readonly rmsDbContext _context;
//        public AdminService(rmsDbContext context) => _context = context;

//        public IEnumerable<Employee> GetAllEmployees() => _context.Employees.ToList();

//        public (bool success, string message) CreateEmployee(string name, string password, string role)
//        {
//            if (role == "Admin" && _context.Employees.Any(e => e.Role == "Admin"))
//                return (false, "An Administrator account already exists.");

//            // Custom ID Logic: Role(2 chars) + random 2 digits
//            string prefix = role.Length >= 2 ? role.Substring(0, 2).ToUpper() : "ST";
//            string empId = $"{prefix}{new Random().Next(10, 99)}";

//            var newEmp = new Employee { Name = name, EmpId = empId, password = password, Role = role, IsActive = true };
//            _context.Employees.Add(newEmp);
//            _context.SaveChanges();
//            return (true, $"Employee Added! ID: {empId}");
//        }

//        public (bool success, string message) EditEmployee(int id, string name, string role, bool isActive, string loggedInEmpId)
//        {
//            var emp = _context.Employees.Find(id);
//            var admin = _context.Employees.FirstOrDefault(e => e.EmpId == loggedInEmpId);

//            if (emp == null) return (false, "Employee not found.");
//            if (emp.Role == "Admin" && admin?.Role != "Admin") return (false, "Only Admin can modify Admin accounts.");
//            if (emp.Role == "Admin" && !isActive) return (false, "Admin accounts cannot be deactivated.");

//            emp.Name = name;
//            emp.Role = role;
//            if (emp.Role != "Admin") emp.IsActive = isActive;

//            _context.SaveChanges();
//            return (true, "Employee Updated Successfully!");
//        }

//        public (bool success, string message) ToggleStatus(int id)
//        {
//            var emp = _context.Employees.Find(id);
//            if (emp == null) return (false, "Employee not found.");
//            if (emp.Role == "Admin") return (false, "Admin accounts cannot be deactivated.");

//            emp.IsActive = !emp.IsActive;
//            _context.SaveChanges();
//            return (true, emp.IsActive ? "Account Reactivated!" : "Account Deactivated.");
//        }
//    }
//}
