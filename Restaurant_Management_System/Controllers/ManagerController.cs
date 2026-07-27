using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Services;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var vm = await _managerService.GetDashboardDataAsync();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> MenuManagement()
        {
            var menuItems = await _managerService.GetAllMenuItemsAsync();
            return View(menuItems);
        }

        [HttpGet]
        public async Task<IActionResult> MenuView(int id)
        {
            var menuItem = await _managerService.GetMenuItemWithRecipeAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [HttpGet]
        public IActionResult MenuAdd()
        {
            var vm = new MenuItemCreateViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuAdd(MenuItemCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (_, message) = await _managerService.AddMenuItemAsync(vm);
            TempData["Message"] = message;

            return RedirectToAction(nameof(MenuManagement));
        }

        [HttpGet]
        public async Task<IActionResult> MenuEdit(int id)
        {
            var vm = await _managerService.GetMenuItemForEditAsync(id);
            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuEdit(MenuItemCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (success, message) = await _managerService.UpdateMenuItemAsync(vm);
            TempData["Message"] = message;

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(MenuManagement));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuDelete(int id)
        {
            var (success, message) = await _managerService.DeleteMenuItemAsync(id);
            TempData["Message"] = message;

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(MenuManagement));
        }

        [HttpGet]
        public async Task<IActionResult> OrderMonitoring(OrderStatus? status)
        {
            var orders = await _managerService.GetMonitoredOrdersAsync(status);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> TableManagement()
        {
            var vm = await _managerService.GetTableManagementDataAsync();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Inventory(string? search)
        {
            var vm = await _managerService.GetInventoryDataAsync(search);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var vm = await _managerService.GetReportsDataAsync();
            return View(vm);
        }
    }
}