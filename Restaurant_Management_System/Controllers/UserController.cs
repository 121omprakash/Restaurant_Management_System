//using Microsoft.AspNetCore.Mvc;
//using Restaurant_Management_System.Data;
//using Microsoft.IdentityModel.Tokens;

//public class UserController : Controller
//{
//    private readonly rmsDbContext _context;
//    public UserController(rmsDbContext context)
//    {
//        _context = context;
//    }
//    [HttpGet]
//    public IActionResult Login()
//    {
//        return View();
//    }

//    [HttpPost]
//    public IActionResult Login(string UserId, string password)
//    {
//        // Added StringComparer to allow case-insensitive User ID comparisons
//        var res = from u in _context.Users
//                  where string.Equals(u.UserId, UserId, StringComparison.OrdinalIgnoreCase) &&
//                        string.Equals(u.password, password, StringComparison.OrdinalIgnoreCase)
//                  select u.Role;
//        if(res.IsNullOrEmpty()) 
//        {
//            ViewBag.Error = "Invalid User ID or Password";
//        }
//        else { 
//            string role = res.FirstOrDefault();
//            if (role == "Admin")
//            {

//                return RedirectToAction("Index", "Admin"); //
//            }
//            else if (role == "Chef")
//            {
//                return RedirectToAction("Index", "Kitchen");//
//            }
//            else if (role == "Manager")
//            {

//                return RedirectToAction("Index", "Reports");
//            }
//            else if (role == "Waiter")
//            {
//                return RedirectToAction("Index", "Order");//
//            }
//            else if (role == "Cashier")
//            {
//                return RedirectToAction("Index", "Billing");//
//            }
//            else if (role == "Inventory Clerk")
//            {
//                return RedirectToAction("Index", "Inventory");//
//            }
//        }

//        return View();
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Data;
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
    public IActionResult Login(string UserId, string password)
    {
        if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Please fill in all fields.";
            return View();
        }

        // Clean inputs to prevent trailing space errors
        string cleanUserId = UserId.Trim();
        string cleanPassword = password.Trim();

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
            return RedirectToAction("Index", "Admin");    
        }
        else if (role == "Chef")
        {
            // Fixed: Routes to ChefController matching your role-based folder structure
            return RedirectToAction("Dashboard", "Kitchen");
        }
        else if (role == "Manager")
        {
            return RedirectToAction("Index", "Manager");
        }
        else if (role == "Waiter")
        {
            return RedirectToAction("Index", "Waiter");
        }
        else if (role == "Cashier")
        {
            return RedirectToAction("Index", "Cashier");
        }
        else if (role == "Inventory Clerk")
        {
            return RedirectToAction("Index", "Inventory Clerk");
        }

        // Fallback safety route if a user role isn't recognized
        ViewBag.Error = "Role not authorized on this terminal.";
        return View();
    }
}