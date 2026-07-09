using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_Error.Models;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant_Management_Error.Controllers
{
    public class InventoryController : Controller
    {
        //Temporary in-memory database
        private static List<Ingredient> items = new List<Ingredient>
        {
            new Ingredient { Id = 1, Name = "Rice", Category = "Grains", Stock = 50, ReorderLevel = 20 },
            new Ingredient { Id = 2, Name = "Paneer", Category = "Dairy", Stock = 3, ReorderLevel = 5 },
            new Ingredient { Id = 3, Name = "Fish", Category = "Meat", Stock = 15, ReorderLevel = 10 },
            new Ingredient { Id = 4, Name = "Mutton", Category = "Meat", Stock = 10, ReorderLevel = 10 },
            new Ingredient { Id = 5, Name = "Wheat", Category = "Grains", Stock = 8, ReorderLevel = 10 }
        };

        //Dashboard (dynamic)
        public IActionResult Dashboard()
        {
            var total = items.Count;
            var lowItems = items.Where(i => i.Stock < i.ReorderLevel).ToList();
            var lowCount = lowItems.Count;
            var criticalCount = items.Count(i => i.Stock < (i.ReorderLevel / 2.0));
            var purchaseRequests = 1; 

            var vm = new InventoryDashboardViewModel
            {
                TotalItems = total,
                LowStockCount = lowCount,
                CriticalCount = criticalCount,
                PurchaseRequests = purchaseRequests,
                LowItems = lowItems,
                AllItems = items
            };

            return View(vm);
        }

        //Show inventory
        public IActionResult InventoryManagement()
        {
            return View(items);
        }

        //Get stock (same page)
        public IActionResult GetStockLevels()
        {
            return View("InventoryManagement", items);
        }

        //CREATE PAGE
        public IActionResult Create()
        {
            return View();
        }

        //ADD ITEM
        [HttpPost]
        public IActionResult RecordStockReceipt(Ingredient item)
        {
            item.Id = items.Count + 1;
     
            return RedirectToAction("AddItem");

               }

        //EDIT PAGE
        public IActionResult Edit(int id)
        {
           var item = items.FirstOrDefault(i => i.Id == id);
            return View(item);
        }

        //UPDATE ITEM
        [HttpPost]
        public IActionResult ConsumeIngredients(Ingredient updatedItem)
        {
            var item = items.FirstOrDefault(i => i.Id == updatedItem.Id);

            if (item != null)
            {
                item.Name = updatedItem.Name;
                item.Category = updatedItem.Category;
                item.Stock = updatedItem.Stock;
                item.ReorderLevel = updatedItem.ReorderLevel;
            }

            return RedirectToAction("InventoryManagement");
        }

        //DELETE
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);

            if (item != null)
            {
                items.Remove(item);
            }

            return RedirectToAction("InventoryManagement");
        }

        //LOW STOCK - page action
        public IActionResult LowStock()
        {
            var lowItems = items.Where(i => i.Stock < i.ReorderLevel).ToList();
            return View(lowItems);
        }

        //Backwards-compatible API/action name (keeps existing behavior)
        public IActionResult RaiseLowStockAlert()
        {
            //Reuse LowStock logic and return the same view explicitly
            var lowItems = items.Where(i => i.Stock < i.ReorderLevel).ToList();
            return View("LowStock", lowItems);
        }
    }
}