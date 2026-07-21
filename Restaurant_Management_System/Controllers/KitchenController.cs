using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Models.ViewModels;
using Restaurant_Management_System.ViewModel;
using System.Linq;
using System.Net.NetworkInformation;


namespace Restaurant_Management_System.Controllers
{
    //[Authorize(Roles = "Chef")]
    public class kitchenController : Controller
    {
        private readonly rmsDbContext _context;

        public kitchenController(rmsDbContext context)
        {
            _context = context;
        } 
        public IActionResult Index()
        {
            return RedirectToAction("OrderManagement");
        }

        public IActionResult OrderManagement(TicketStatus? status, string? search)
        {
            // Dashboard Card Counts
            ViewBag.PendingCount = _context.KitchenTickets.Count(x => x.TicketStatus == TicketStatus.QUEUED);
            ViewBag.PreparingCount = _context.KitchenTickets.Count(x => x.TicketStatus == TicketStatus.IN_PROGRESS);
            ViewBag.CompletedCount = _context.KitchenTickets.Count(x => x.TicketStatus == TicketStatus.READY);
            ViewBag.DelayedCount = 0;

            ViewBag.Search = search;
            ViewBag.SelectedStatus = status;

            var tickets = _context.KitchenTickets
                .Include(k => k.CustomerOrder)
                .AsQueryable();

            // Filter by Status
            if (status.HasValue)
            {
                tickets = tickets.Where(k => k.TicketStatus == status.Value);
            }

            // Search by Order ID or Customer Name
            if (!string.IsNullOrWhiteSpace(search))
            {
                tickets = tickets.Where(k =>
                    k.OrderId.ToString().Contains(search) ||
                    k.CustomerOrder.CustomerName.Contains(search));
            }

            return View(tickets.ToList());
        }


        public IActionResult ViewItems(int orderId)
        {
            var items = _context.OrderItems
                .Where(o => o.OrderId == orderId)
                .Include(o => o.MenuItem)
                .ToList();
               ViewBag.OrderId = orderId;

            return View(items);
        }

        [HttpPost]
        public IActionResult markItemPrepared(int id)
        {
            var ticket = _context.KitchenTickets
                .FirstOrDefault(k => k.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.TicketStatus = TicketStatus.IN_PROGRESS;
            ticket.StartTime = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(OrderManagement));
        }

        [HttpPost]
        public IActionResult completeOrder(int id)
        {
            var ticket = _context.KitchenTickets
                .FirstOrDefault(k => k.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.TicketStatus = TicketStatus.READY;
            ticket.CompletionTime = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(OrderManagement));
        }

       
        public IActionResult RecipeManagement(string tab="existing")
        {
            var vm = new RecipeManagementViewModel();

            vm.ExistingRecipes = _context.MenuItems
                .Where(m => _context.ItemRecipes.Any(r => r.MenuItemId == m.MenuItemId))
                .ToList();

            vm.PendingRecipes = _context.MenuItems
                .Where(m => !_context.ItemRecipes.Any(r => r.MenuItemId == m.MenuItemId))
                .ToList();

            vm.ActiveTab = tab;

            return View(vm);

        }

        public IActionResult AddRecipe(int id)
        {
            var menuItem = _context.MenuItems
                .FirstOrDefault(m => m.MenuItemId == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            var model = new AddRecipeViewModel
            {
                MenuItemId = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Category = menuItem.Category,
                PreparationTime = menuItem.PreparationTime,
                RecipeSteps = menuItem.RecipeSteps
            };

            ViewBag.Ingredients = _context.Ingredients.ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddRecipe(AddRecipeViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                ViewBag.Ingredients = _context.Ingredients.ToList();
                return View(model);
            }

            // Find the menu item
            var menuItem = await _context.MenuItems.FindAsync(model.MenuItemId);

            if (menuItem == null)
            {
                return NotFound();
            }

            // Save recipe steps
            menuItem.RecipeSteps = model.RecipeSteps;

            // Remove old ingredients (if any)
            var oldRecipes = _context.ItemRecipes
                .Where(r => r.MenuItemId == model.MenuItemId);

            _context.ItemRecipes.RemoveRange(oldRecipes);

            // Add new ingredients
            for (int i = 0; i < model.IngredientIds.Count; i++)
            {
                var recipe = new ItemRecipe
                {
                    MenuItemId = model.MenuItemId,
                    IngredientId = model.IngredientIds[i],
                    Quantity = model.Quantities[i]
                };

                _context.ItemRecipes.Add(recipe);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("RecipeManagement");
        }

        public IActionResult ViewRecipe(int menuItemid)
        {
            var menuItem = _context.MenuItems
                .Include(m => m.ItemRecipes)
                .ThenInclude(ir => ir.Ingredient)
                .FirstOrDefault(m => m.MenuItemId == menuItemid);

            if (menuItem == null)
            {
                return NotFound();
            }

            var model = new AddRecipeViewModel
            {
                MenuItemId = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Category = menuItem.Category,
                PreparationTime = menuItem.PreparationTime,
                RecipeSteps = menuItem.RecipeSteps,

                IngredientIds = menuItem.ItemRecipes
                    .Select(r => r.IngredientId)
                    .ToList(),

                Quantities = menuItem.ItemRecipes
                    .Select(r => r.Quantity)
                    .ToList(),
                IngredientNames = menuItem.ItemRecipes
                    .Select(r => r.Ingredient.IngredientName)
                    .ToList(),

                Units = menuItem.ItemRecipes
                    .Select(r => r.Ingredient.UnitOfMeasure)
                    .ToList(),
                IsReadOnly = true
            };

            ViewBag.Ingredients = _context.Ingredients.ToList();

            return View("AddRecipe", model);
        }


        public IActionResult EditRecipe(int menuItemId)
        {
            var menuItem = _context.MenuItems
                .Include(m => m.ItemRecipes)
                    .ThenInclude(r => r.Ingredient)
                .FirstOrDefault(m => m.MenuItemId == menuItemId);

            if (menuItem == null)
            {
                return NotFound();
            }

            var model = new AddRecipeViewModel
            {
                MenuItemId = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Category = menuItem.Category,
                PreparationTime = menuItem.PreparationTime,
                RecipeSteps = menuItem.RecipeSteps,

                IngredientIds = menuItem.ItemRecipes
                    .Select(r => r.IngredientId)
                    .ToList(),

                Quantities = menuItem.ItemRecipes
                    .Select(r => r.Quantity)
                    .ToList(),

                IngredientNames = menuItem.ItemRecipes
                    .Select(r => r.Ingredient.IngredientName)
                    .ToList(),

                Units = menuItem.ItemRecipes
                    .Select(r => r.Ingredient.UnitOfMeasure)
                    .ToList(),

                IsEdit = true,
                IsReadOnly = false
            };

            ViewBag.Ingredients = _context.Ingredients.ToList();

            return View("AddRecipe", model);
        }
    }
}
