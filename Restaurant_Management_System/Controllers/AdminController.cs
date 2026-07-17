using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using System.Linq;

namespace Restaurant_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly rmsDbContext _context;

        public AdminController(rmsDbContext context)
        {
            _context = context;
        }

        public IActionResult Employees()
        {
            ViewBag.TotalProfiles = _context.Employees.Count();
            ViewBag.ActiveProfiles = _context.Employees.Count(e => e.IsActive);
            ViewBag.InactiveProfiles = _context.Employees.Count(e => !e.IsActive);
            ViewBag.SecurityAdmins = _context.Employees.Count(e => e.Role == "Admin" && e.IsActive);
            ViewBag.OperationalStaff = _context.Employees.Count(e => e.Role != "Admin" && e.IsActive);

            return View(_context.Employees.ToList());
        }

        [HttpPost]
        public IActionResult CreateUser(string name, string password, string role)
        {
            if (role == "Admin" && _context.Employees.Any(e => e.Role == "Admin"))
            {
                TempData["Message"] = "Error: An Administrator account already exists.";
                return RedirectToAction("Employees");
            }

            // Generate Unique ID
            string prefix = role.Length >= 2 ? role.Substring(0, 2).ToUpper() : "ST";
            int count = _context.Employees.Count(e => e.Role == role) + 1;
            string generatedId = $"{prefix}{count:D2}";

            var newEmp = new Employee
            {
                Name = name,
                EmpId = generatedId,
                password = password,
                Role = role,
                IsActive = true
            };

            _context.Employees.Add(newEmp);
            _context.SaveChanges();

            TempData["Message"] = $"Employee Added! ID: {generatedId}";
            return RedirectToAction("Employees");
        }

        [HttpPost]
        public IActionResult EditUser(int id, string name, string role, bool isActive)
        {
            var emp = _context.Employees.Find(id);

            if (emp == null) return RedirectToAction("Employees");
            if (!emp.IsActive)
            {
                TempData["Message"] = "Action Denied: You must reactivate the employee before editing their details.";
                return RedirectToAction("Employees");
            }

            // 1. Prevent deactivating the Admin
            if (emp.Role == "Admin" && !isActive)
            {
                TempData["Message"] = "Action Denied: The Admin account cannot be deactivated.";
                return RedirectToAction("Employees");
            }

            // 2. Update properties
            emp.Name = name;
            emp.Role = role;
            emp.IsActive = isActive;

            _context.SaveChanges();
            TempData["Message"] = "Employee details updated.";
            return RedirectToAction("Employees");
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp == null) return RedirectToAction("Employees");

            if (emp.Role == "Admin")
            {
                TempData["Message"] = "Action Denied: The Administrator account cannot be deactivated.";
            }
            else
            {
                emp.IsActive = !emp.IsActive;
                _context.SaveChanges();
                TempData["Message"] = emp.IsActive ? "Employee reactivated successfully." : "Employee deactivated successfully.";
            }
            return RedirectToAction("Employees");
        }

        public IActionResult Settings()
        {
            var settings = _context.SystemSettings.FirstOrDefault(s => s.Id == 1) ?? new SystemSetting { Id = 1 };
            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSettings(SystemSetting updatedData)
        {
            var existing = _context.SystemSettings.FirstOrDefault(s => s.Id == 1);
            if (existing != null)
            {
                existing.RestaurantName = updatedData.RestaurantName;
                existing.PrimaryPhone = updatedData.PrimaryPhone;
                existing.CorporateEmail = updatedData.CorporateEmail;
                existing.PhysicalAddress = updatedData.PhysicalAddress;
                existing.TaxIdentifier = updatedData.TaxIdentifier;
                existing.BaseCgstPercentage = updatedData.BaseCgstPercentage;
                existing.BaseSgstPercentage = updatedData.BaseSgstPercentage;
                _context.SaveChanges();
                TempData["Message"] = "Settings updated successfully!";
            }
            return RedirectToAction("Settings");
        }
    }
}