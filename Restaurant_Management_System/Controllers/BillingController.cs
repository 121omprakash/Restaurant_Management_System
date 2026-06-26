using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Restaurant_Management_System.Controllers
{
    //[Authorize(Roles = "Cashier")]
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
