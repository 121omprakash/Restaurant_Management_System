using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ViewModel;
using System.Linq;

public class UserController : Controller
{
    private readonly rmsDbContext _context;

    public UserController(rmsDbContext context)
    {
        _context = context;
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
        var user = _context.Users
            .FirstOrDefault(u => u.UserId.ToLower() == cleanUserId.ToLower() && u.password == cleanPassword);

        // 2. If no matching user record is returned, throw the error banner
        if (user == null)
        {
            ViewBag.Error = "Invalid User ID or Password";
            return View();
        }

        // 3. Match against the User's Role property and route to the correct role controller
        string role = user.Role;

        if (role == "Admin")
        {
            return RedirectToAction("Dashboard", "Admin");
        }
        else if (role == "Chef")
        {
            // Fixed: Routes to ChefController matching your role-based folder structure
            return RedirectToAction("Dashboard", "Kitchen");
        }
        else if (role == "Manager")
        {
            return RedirectToAction("Dashboard", "Manager");
        }
        else if (role == "Waiter")
        {
            return RedirectToAction("Dashboard", "Waiter");
        }
        else if (role == "Cashier")
        {
            return RedirectToAction("Index", "Billing");
        }
        else if (role == "Inventory Clerk")
        {
            return RedirectToAction("Dashboard", "Inventory");
        }

        // Fallback safety route if a user role isn't recognized
        ViewBag.Error = "Role not authorized on this terminal.";
        return View();
    }
}
