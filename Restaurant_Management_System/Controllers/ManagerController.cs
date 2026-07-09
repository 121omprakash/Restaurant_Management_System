using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Management_System.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult MenuManagement()
        {
            return View();
        }

        public IActionResult MenuAdd()
        {
            return View();
        }

        public IActionResult MenuView()
        {
            return View();
        }

        public IActionResult MenuEdit()
        {
            return View();
        }


        public IActionResult MenuDelete()
        {
            return View();
        }

        public IActionResult OrderMonitoring()
        {
            return View();
        }

        public IActionResult TableManagement()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }
    }
}