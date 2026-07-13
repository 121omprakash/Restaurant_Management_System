    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
namespace Restaurant_Management_System.Controllers
    {

        public class AdminController : Controller
        {
            // 1. Declare  Database Context variable
            private readonly rmsDbContext _context;

            // 2. Inject the context through the constructor
            public AdminController(rmsDbContext context)
            {
                _context = context;
            }

        
        public IActionResult Employees()
        {
            // Calculate profile totals
            ViewBag.TotalProfiles = _context.Employees.Count();
            ViewBag.ActiveProfiles = _context.Employees.Count(e => e.IsActive); // Calculates active count here safely!
            ViewBag.InactiveProfiles = _context.Employees.Count(e => !e.IsActive);

            // Filter role statistics to ONLY count currently Active employees
            ViewBag.SecurityAdmins = _context.Employees.Count(e => e.Role == "Admin" && e.IsActive);
            ViewBag.OperationalStaff = _context.Employees.Count(e => e.Role != "Admin" && e.IsActive);

            // Fetch the entire collection list for the data table grid
            var usersList = _context.Employees.ToList();

            return View(usersList);
        }

        // ADD EMPLOYEE: Receives the POST data from #addUserModal
        [HttpPost]
        public IActionResult CreateUser(string name, string empId, string password, string role)
        {
            if (ModelState.IsValid)
            {
                var newEmp = new Employee { Name = name, EmpId = empId, password = password, Role = role, IsActive = true };
                _context.Employees.Add(newEmp);
                _context.SaveChanges();
            }
            return RedirectToAction("Employees");
        }

        // EDIT EMPLOYEE: Receives the POST data from #editUserModal
        [HttpPost]
        public IActionResult EditUser(int id, string name, string role, bool isActive)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                emp.Name = name;
                emp.Role = role;
                emp.IsActive = isActive;
                _context.SaveChanges();
            }
            return RedirectToAction("Employees");
        }

        // DEACTIVATE/TOGGLE STATUS: Receives the POST data from #deleteConfirmModal
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                emp.IsActive = !emp.IsActive; // Inverts status flag cleanly
                _context.SaveChanges();
            }
            return RedirectToAction("Employees");
        }

        public IActionResult Details(int id)
            {
                return View();
            }

        public IActionResult Settings()
        {
            return View();
        }
        }
    }
