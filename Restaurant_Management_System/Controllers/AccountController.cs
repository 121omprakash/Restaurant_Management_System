using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Models;
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    //[HttpPost]
    //public IActionResult Login(string user, string name)
    //{
    //    // Mock authentication for now
    //    Dictionary<string, string> role = new Dictionary<string, string>()
    //    {
    //        {"AD01","Admin" },
    //        {"CH01","Chef" },
    //        {"MN01","Manager" },
    //        {"WA01","Waiter" },
    //        {"CS01","Cashier" },
    //        {"IC01","Inventory Cleck" }
    //    };
    //    if(role.Keys.Contains(a.AccountId))
    //    {
    //        if(a.password == "123")
    //        {
    //            string ret = role[a.AccountId];
    //            return RedirectToAction("Index", "Home");
    //        }
    //    }
    //    ViewBag.Error = "Invalid User ID or Password";
    //    return View();
    //}
}