using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Management_System.Controllers
{
    public class WaiterController : Controller
    {

        public IActionResult Dashboard()
        {
            return View();
        }

        
        public IActionResult TableReservation()
        {
            return View();
        }

       
        public IActionResult TakeOrder()
        {
            return View();
        }

       
        public IActionResult ManageOrders()
        {
            return View();
        }

      
        public IActionResult OrderStatus()
        {
            return View();
        }

       
        public IActionResult ServeOrders()
        {
            return View();
        }

        
        public IActionResult Billing()
        {
            return View();
        }
    }
}
