using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Models.ViewModels;
using Restaurant_Management_System.ViewModel;
using System.Linq;

namespace Restaurant_Management_System.Services
{
    public class KitchenService : IKitchenService

    {
        private readonly rmsDbContext _context;

        public KitchenService(rmsDbContext context)
        {
            _context = context;
        }
        public async Task<AddRecipeViewModel?> GetRecipeAsync(int menuItemId)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.ItemRecipes)
                    .ThenInclude(ir => ir.Ingredient)
                .FirstOrDefaultAsync(m => m.MenuItemId == menuItemId);

            if (menuItem == null)
            {
                return null;
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

            return model;
        }

        public async Task<AddRecipeViewModel?> GetEditRecipeAsync(int menuItemId)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.ItemRecipes)
                .ThenInclude(r => r.Ingredient)
                .FirstOrDefaultAsync(m => m.MenuItemId == menuItemId);

            if (menuItem == null)
            {
                return null;
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

                IsReadOnly = false,
                IsEdit = true
            };

            return model;
        }

        public async Task<bool> AddRecipeAsync(AddRecipeViewModel model)
        {
            var menuItem = await _context.MenuItems.FindAsync(model.MenuItemId);

            if (menuItem == null)
            {
                return false;
            }

            // Save recipe steps
            menuItem.RecipeSteps = model.RecipeSteps;
            menuItem.PreparationTime = model.PreparationTime;

            // Remove old ingredients
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
                    Quantity = model.Quantities[i],
                    UnitOfMeasure = model.Units[i]
                };

                _context.ItemRecipes.Add(recipe);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RecipeManagementViewModel> GetRecipeManagementAsync(string tab)
        {
            var vm = new RecipeManagementViewModel();

            vm.ExistingRecipes = await _context.MenuItems
                .Where(m => _context.ItemRecipes.Any(r => r.MenuItemId == m.MenuItemId))
                .ToListAsync();

            vm.PendingRecipes = await _context.MenuItems
                .Where(m => !_context.ItemRecipes.Any(r => r.MenuItemId == m.MenuItemId))
                .ToListAsync();

            vm.ActiveTab = tab;

            return vm;
        }

        public async Task<AddRecipeViewModel?> GetAddRecipeAsync(int id)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (menuItem == null)
            {
                return null;
            }

            var model = new AddRecipeViewModel
            {
                MenuItemId = menuItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Category = menuItem.Category,
                PreparationTime = menuItem.PreparationTime,
                RecipeSteps = menuItem.RecipeSteps
            };

            return model;
        }


        public async Task<OrderManagementViewModel> GetOrderManagementAsync(
    TicketStatus? status,
    string? search,
    bool delayed)
        {
            var vm = new OrderManagementViewModel();

            // Dashboard Card Counts
            vm.PendingCount = await _context.KitchenTickets
                .CountAsync(x => x.TicketStatus == TicketStatus.QUEUED);

            vm.PreparingCount = await _context.KitchenTickets
                .Include(k => k.CustomerOrder)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                .CountAsync(k =>
                    k.TicketStatus == TicketStatus.IN_PROGRESS &&
                    (
                        !k.StartTime.HasValue ||
                        DateTime.Now <=
                        k.StartTime.Value.AddMinutes(
                            k.CustomerOrder.OrderItems.Max(oi => oi.MenuItem.PreparationTime) + 3
                        )
                    ));

            vm.CompletedCount = await _context.KitchenTickets
                .CountAsync(x => x.TicketStatus == TicketStatus.READY);

            vm.DelayedCount = await _context.KitchenTickets
                .Include(k => k.CustomerOrder)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                .CountAsync(k =>
                    k.TicketStatus == TicketStatus.IN_PROGRESS &&
                    k.StartTime.HasValue &&
                    DateTime.Now >
                    k.StartTime.Value.AddMinutes(
                        k.CustomerOrder.OrderItems.Max(oi => oi.MenuItem.PreparationTime) + 3));

            vm.Search = search;
            vm.SelectedStatus = status;

            var tickets = _context.KitchenTickets
                .Include(k => k.CustomerOrder)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                .AsQueryable();

            if (delayed)
            {
                tickets = tickets.Where(k =>
                    k.TicketStatus == TicketStatus.IN_PROGRESS &&
                    k.StartTime.HasValue &&
                    DateTime.Now >
                    k.StartTime.Value.AddMinutes(
                        k.CustomerOrder.OrderItems.Max(oi => oi.MenuItem.PreparationTime) + 3));
            }
            else if (status.HasValue)
            {
                if (status == TicketStatus.IN_PROGRESS)
                {
                    tickets = tickets.Where(k =>
                        k.TicketStatus == TicketStatus.IN_PROGRESS &&
                        (
                            !k.StartTime.HasValue ||
                            DateTime.Now <=
                            k.StartTime.Value.AddMinutes(
                                k.CustomerOrder.OrderItems.Max(oi => oi.MenuItem.PreparationTime) + 3
                            )
                        ));
                }
                else
                {
                    tickets = tickets.Where(k => k.TicketStatus == status.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                tickets = tickets.Where(k =>
                    k.OrderId.ToString().Contains(search) ||
                    k.CustomerOrder.CustomerName.Contains(search));
            }

            vm.Tickets = await tickets.ToListAsync();

            return vm;
        }

        public async Task<bool> MarkItemPreparedAsync(int id)
        {
            var ticket = await _context.KitchenTickets
                .FirstOrDefaultAsync(k => k.TicketId == id);

            if (ticket == null)
            {
                return false;
            }

            ticket.TicketStatus = TicketStatus.IN_PROGRESS;
            ticket.StartTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteOrderAsync(int id)
        {
            var ticket = await _context.KitchenTickets
                .FirstOrDefaultAsync(k => k.TicketId == id);

            if (ticket == null)
            {
                return false;
            }

            ticket.TicketStatus = TicketStatus.READY;
            ticket.CompletionTime = DateTime.Now;

            var orderItems = await _context.OrderItems
                .Where(o => o.OrderId == ticket.OrderId)
                .ToListAsync();

            foreach (var orderItem in orderItems)
            {
                var recipes = await _context.ItemRecipes
                    .Where(r => r.MenuItemId == orderItem.MenuItemId)
                    .ToListAsync();

                foreach (var recipe in recipes)
                {
                    var ingredient = await _context.Ingredients
                        .FirstOrDefaultAsync(i => i.IngredientId == recipe.IngredientId);

                    if (ingredient == null)
                        continue;

                    decimal convertedQuantity = ConvertToIngredientUnit(
                        recipe.Quantity,
                        recipe.UnitOfMeasure,
                        ingredient.UnitOfMeasure);

                    decimal quantityToReduce = convertedQuantity * orderItem.Quantity;

                    ingredient.CurrentStock -= quantityToReduce;

                    if (ingredient.CurrentStock < 0)
                        ingredient.CurrentStock = 0;

                    if (ingredient.CurrentStock == 0)
                        ingredient.StockStatus = StockStatus.OUT_OF_STOCK;
                    else if (ingredient.CurrentStock <= ingredient.ReorderLevel)
                        ingredient.StockStatus = StockStatus.LOW;
                    else
                        ingredient.StockStatus = StockStatus.AVAILABLE;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }


        private decimal ConvertToIngredientUnit(decimal quantity, UnitOfMeasure recipeUnit, UnitOfMeasure ingredientUnit)
        {
            if (recipeUnit == ingredientUnit)
                return quantity;

            // Weight
            if (recipeUnit == UnitOfMeasure.KG && ingredientUnit == UnitOfMeasure.G)
                return quantity * 1000;

            if (recipeUnit == UnitOfMeasure.G && ingredientUnit == UnitOfMeasure.KG)
                return quantity / 1000;

            // Liquid
            if (recipeUnit == UnitOfMeasure.L && ingredientUnit == UnitOfMeasure.ML)
                return quantity * 1000;

            if (recipeUnit == UnitOfMeasure.ML && ingredientUnit == UnitOfMeasure.L)
                return quantity / 1000;

            // Pieces
            if (recipeUnit == UnitOfMeasure.P && ingredientUnit == UnitOfMeasure.P)
                return quantity;

            throw new Exception("Unsupported unit conversion.");
        }

        public async Task<List<OrderItem>> GetViewItemsAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(o => o.OrderId == orderId)
                .Include(o => o.MenuItem)
                .ToListAsync();
        }
    }
}
