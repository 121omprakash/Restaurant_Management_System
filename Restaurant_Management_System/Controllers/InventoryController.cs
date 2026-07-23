using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;
using Restaurant_Management_System.Services;

namespace Restaurant_Management_System.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventory _inventoryService;

        public InventoryController(IInventory inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            var vm = _inventoryService.GetDashboard();
            return View(vm);
        }

        // Inventory List
        public IActionResult InventoryManagement(string q)
        {
            var vm = _inventoryService.GetAll(q);
            vm.UserRole = User?.Identity?.Name ?? string.Empty;
            return View(vm);
        }

        // View Stock Levels
        public IActionResult GetStockLevels()
        {
            var vm = _inventoryService.GetAll();
            vm.UserRole = User?.Identity?.Name ?? string.Empty;
            return View("InventoryManagement", vm);
        }

        // Create Page
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new InventoryItemViewModel { UserRole = User?.Identity?.Name ?? string.Empty };
            return View(vm);
        }

        // Add Ingredient
        [HttpPost]
        public IActionResult RecordStockReceipt(InventoryItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", vm);
            }

            var item = vm.Ingredient;
            _inventoryService.Create(item);
            return RedirectToAction(nameof(InventoryManagement));
        }

        // Edit Page
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vm = _inventoryService.GetItem(id);
            vm.UserRole = User?.Identity?.Name ?? string.Empty;
            if (vm.Ingredient == null || vm.Ingredient.IngredientId == 0)
                return NotFound();
            return View(vm);
        }

        // Update Ingredient
        [HttpPost]
        public IActionResult ConsumeIngredients(InventoryItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            var updatedItem = vm.Ingredient;
            _inventoryService.Update(updatedItem);
            return RedirectToAction(nameof(InventoryManagement));
        }

        // Delete Ingredient
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _inventoryService.Delete(id);
            }
            catch
            {
                TempData["Error"] = "This ingredient is being used in recipes and cannot be deleted.";
            }

            return RedirectToAction(nameof(InventoryManagement));
        }

        // Low Stock Page
        public IActionResult LowStock()
        {
            var vm = _inventoryService.GetLowStock();
            vm.UserRole = User?.Identity?.Name ?? string.Empty;
            return View(vm);
        }

        // Low Stock Alert
        public IActionResult RaiseLowStockAlert()
        {
            var vm = _inventoryService.GetLowStock();
            vm.UserRole = User?.Identity?.Name ?? string.Empty;
            return View("LowStock", vm);
        }

        // NOTE: stock status handling is performed inside InventoryService.
    }
}