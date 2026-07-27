using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using System.Linq;

public class EmployeeController : Controller
{
    private readonly rmsDbContext _context;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    public EmployeeController(rmsDbContext context, IPasswordHasher<Employee> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel l)
    {
        if (string.IsNullOrEmpty(l.UserId) || string.IsNullOrEmpty(l.password))
        {
            ViewBag.Error = "Please fill in all fields.";
            return View();
        }

        // Clean inputs to prevent trailing space errors
        string cleanUserId = l.UserId.Trim();
        string cleanPassword = l.password.Trim();

        // 1. Fetch the user directly from the database safely
        // SQL handles string matching perfectly. Passwords remain case-sensitive here.
        var user = _context.Employees
                    .FirstOrDefault(u => u.EmpId.ToLower() == cleanUserId.ToLower());

        // 2. If no matching user record is returned, throw the error banner
        if (user == null)
        {
            ViewBag.Error = "Invalid User ID or Password";
            return View();
        }

        // 3. Verify hashed password
        var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.password, cleanPassword);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            ViewBag.Error = "Invalid User ID or Password.";
            return View();
        }

        // 4. Check if employee profile is active
        if (!user.IsActive)
        {
            ViewBag.Error = "Your account is deactivated. Please contact your administrator.";
            return View();
        }

        // 5. Route to correct controller based on Role
        switch (user.Role)
        {
            case "Admin":
                return RedirectToAction("Employees", "Admin");

            case "Chef":
                return RedirectToAction("OrderManagement", "Kitchen");

            case "Manager":
                return RedirectToAction("Dashboard", "Manager");

            case "Waiter":
                return RedirectToAction("Menu", "Waiter");

            case "Cashier":
                return RedirectToAction("Index", "Billing");

            case "Inventory Clerk":
                return RedirectToAction("Dashboard", "Inventory");

            default:
                ViewBag.Error = "Role not authorized on this terminal.";
                return View();
        }



    }
}
