using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Restaurant_Management_System.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
