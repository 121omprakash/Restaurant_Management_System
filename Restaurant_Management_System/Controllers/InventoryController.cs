//using Microsoft.AspNetCore.Mvc;
//using Restaurant_Management_System.Models;
//using Restaurant_Management_System.ViewModel;
//using Restaurant_Management_System.ENUM;
//using System.Collections.Generic;
//using System.Linq;
//using Restaurant_Management_System.Data;
//using Microsoft.EntityFrameworkCore;


//namespace Restaurant_Management_System.Controllers
//{
//    public class InventoryController : Controller
//    {
//        // Temporary in-memory data
//        private static List<Ingredient> items = new List<Ingredient>
//        {
//            new Ingredient
//            {
//                IngredientId = 1,
//                IngredientName = "Rice",
//                UnitOfMeasure = "Kg",
//                CurrentStock = 50,
//                ReorderLevel = 20,
//                StockStatus = StockStatus.AVAILABLE
//            },
//            new Ingredient
//            {
//                IngredientId = 2,
//                IngredientName = "Paneer",
//                UnitOfMeasure = "Kg",
//                CurrentStock = 3,
//                ReorderLevel = 5,
//                StockStatus = StockStatus.LOW
//            },
//            new Ingredient
//            {
//                IngredientId = 3,
//                IngredientName = "Fish",
//                UnitOfMeasure = "Kg",
//                CurrentStock = 15,
//                ReorderLevel = 10,
//                StockStatus = StockStatus.AVAILABLE
//            },
//            new Ingredient
//            {
//                IngredientId = 4,
//                IngredientName = "Mutton",
//                UnitOfMeasure = "Kg",
//                CurrentStock = 0,
//                ReorderLevel = 10,
//                StockStatus = StockStatus.OUT_OF_STOCK
//            }
//        };

//        // Dashboard
//        public IActionResult Dashboard()
//        {
//            var lowItems = items
//                .Where(i => i.StockStatus == StockStatus.LOW ||
//                            i.StockStatus == StockStatus.OUT_OF_STOCK)
//                .ToList();

//            var vm = new InventoryDashboardViewModel
//            {
//                TotalItems = items.Count,
//                LowStockCount = lowItems.Count,
//                CriticalCount = items.Count(i => i.StockStatus == StockStatus.OUT_OF_STOCK),
//                PurchaseRequests = 1,
//                LowItems = lowItems,
//                AllItems = items
//            };

//            return View(vm);
//        }

//        // Inventory List
//        public IActionResult InventoryManagement()
//        {
//            return View(items);
//        }

//        // View Stock Levels
//        public IActionResult GetStockLevels()
//        {
//            return View("InventoryManagement", items);
//        }

//        // Create Page
//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // Add Ingredient
//        [HttpPost]
//        public IActionResult RecordStockReceipt(Ingredient item)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View("Create", item);
//            }

//            item.IngredientId = items.Any()
//                ? items.Max(i => i.IngredientId) + 1
//                : 1;

//            UpdateStockStatus(item);

//            items.Add(item);

//            return RedirectToAction(nameof(InventoryManagement));
//        }

//        // Edit Page
//        [HttpGet]
//        public IActionResult Edit(int id)
//        {
//            var item = items.FirstOrDefault(i => i.IngredientId == id);

//            if (item == null)
//            {
//                return NotFound();
//            }

//            return View(item);
//        }

//        // Update Ingredient
//        [HttpPost]
//        public IActionResult ConsumeIngredients(Ingredient updatedItem)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View("Edit", updatedItem);
//            }

//            var item = items.FirstOrDefault(i => i.IngredientId == updatedItem.IngredientId);

//            if (item == null)
//            {
//                return NotFound();
//            }

//            item.IngredientName = updatedItem.IngredientName;
//            item.UnitOfMeasure = updatedItem.UnitOfMeasure;
//            item.CurrentStock = updatedItem.CurrentStock;
//            item.ReorderLevel = updatedItem.ReorderLevel;

//            UpdateStockStatus(item);

//            return RedirectToAction(nameof(InventoryManagement));
//        }

//        // Delete Ingredient
//        [HttpPost]
//        public IActionResult Delete(int id)
//        {
//            var item = items.FirstOrDefault(i => i.IngredientId == id);

//            if (item != null)
//            {
//                items.Remove(item);
//            }

//            return RedirectToAction(nameof(InventoryManagement));
//        }

//        // Low Stock Page
//        public IActionResult LowStock()
//        {
//            var lowItems = items
//                .Where(i => i.StockStatus == StockStatus.LOW ||
//                            i.StockStatus == StockStatus.OUT_OF_STOCK)
//                .ToList();

//            return View(lowItems);
//        }

//        // Low Stock Alert
//        public IActionResult RaiseLowStockAlert()
//        {
//            var lowItems = items
//                .Where(i => i.StockStatus == StockStatus.LOW ||
//                            i.StockStatus == StockStatus.OUT_OF_STOCK)
//                .ToList();

//            return View("LowStock", lowItems);
//        }

//        // Helper Method
//        private void UpdateStockStatus(Ingredient item)
//        {
//            if (item.CurrentStock <= 0)
//            {
//                item.StockStatus = StockStatus.OUT_OF_STOCK;
//            }
//            else if (item.CurrentStock <= item.ReorderLevel)
//            {
//                item.StockStatus = StockStatus.LOW;
//            }
//            else
//            {
//                item.StockStatus = StockStatus.AVAILABLE;
//            }
//        }
//    }
//}





// NEW CODE

using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers
{
    public class InventoryController : Controller
    {
        private readonly rmsDbContext _context;

        public InventoryController(rmsDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            var items = _context.Ingredients.ToList();

            var lowItems = items
                .Where(i =>
                    i.StockStatus == StockStatus.LOW ||
                    i.StockStatus == StockStatus.OUT_OF_STOCK)
                .ToList();

            var vm = new InventoryDashboardViewModel
            {
                TotalItems = items.Count,
                LowStockCount = lowItems.Count,
                CriticalCount = items.Count(i =>
                    i.StockStatus == StockStatus.OUT_OF_STOCK),
                PurchaseRequests = 1,
                LowItems = lowItems,
                AllItems = items
            };

            return View(vm);
        }

        // Inventory List
        public IActionResult InventoryManagement()
        {
            var items = _context.Ingredients.ToList();
            return View(items);
        }

        // View Stock Levels
        public IActionResult GetStockLevels()
        {
            var items = _context.Ingredients.ToList();
            return View("InventoryManagement", items);
        }

        // Create Page
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Add Ingredient
        [HttpPost]
        public IActionResult RecordStockReceipt(Ingredient item)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", item);
            }

            UpdateStockStatus(item);

            _context.Ingredients.Add(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(InventoryManagement));
        }

        // Edit Page
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.Ingredients
                .FirstOrDefault(i => i.IngredientId == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // Update Ingredient
        [HttpPost]
        public IActionResult ConsumeIngredients(Ingredient updatedItem)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", updatedItem);
            }

            var item = _context.Ingredients
                .FirstOrDefault(i =>
                    i.IngredientId == updatedItem.IngredientId);

            if (item == null)
            {
                return NotFound();
            }

            item.IngredientName = updatedItem.IngredientName;
            item.UnitOfMeasure = updatedItem.UnitOfMeasure;
            item.CurrentStock = updatedItem.CurrentStock;
            item.ReorderLevel = updatedItem.ReorderLevel;

            UpdateStockStatus(item);

            _context.SaveChanges();

            return RedirectToAction(nameof(InventoryManagement));
        }

        // Delete Ingredient
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Ingredients
                .FirstOrDefault(i => i.IngredientId == id);

            if (item != null)
            {
                try
                {
                    _context.Ingredients.Remove(item);
                    _context.SaveChanges();
                }
                catch
                {
                    TempData["Error"] =
                        "This ingredient is being used in recipes and cannot be deleted.";
                }
            }

            return RedirectToAction(nameof(InventoryManagement));
        }

        // Low Stock Page
        public IActionResult LowStock()
        {
            var lowItems = _context.Ingredients
                .Where(i =>
                    i.StockStatus == StockStatus.LOW ||
                    i.StockStatus == StockStatus.OUT_OF_STOCK)
                .ToList();

            return View(lowItems);
        }

        // Low Stock Alert
        public IActionResult RaiseLowStockAlert()
        {
            var lowItems = _context.Ingredients
                .Where(i =>
                    i.StockStatus == StockStatus.LOW ||
                    i.StockStatus == StockStatus.OUT_OF_STOCK)
                .ToList();

            return View("LowStock", lowItems);
        }

        // Helper Method
        private void UpdateStockStatus(Ingredient item)
        {
            if (item.CurrentStock <= 0)
            {
                item.StockStatus = StockStatus.OUT_OF_STOCK;
            }
            else if (item.CurrentStock <= item.ReorderLevel)
            {
                item.StockStatus = StockStatus.LOW;
            }
            else
            {
                item.StockStatus = StockStatus.AVAILABLE;
            }
        }
    }
}