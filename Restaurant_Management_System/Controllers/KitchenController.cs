using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Restaurant_Management_System.Controllers
{
    //[Authorize(Roles = "Chef")]
    public class kitchenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult PendingOrders()
        {
            return View();
        }

        public IActionResult PreparingOrders()
        {
            return View();
        }

        public IActionResult DelayedOrders()
        {
            return View();
        }

        public IActionResult CompletedOrders()
        {
            return View();
        }


    }
}
