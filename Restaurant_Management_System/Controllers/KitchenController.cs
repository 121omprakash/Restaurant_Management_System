using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.ENUM;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Models.ViewModels;
using Restaurant_Management_System.Services;
using Restaurant_Management_System.ViewModel;
using System.Linq;
using System.Net.NetworkInformation;


namespace Restaurant_Management_System.Controllers
{
    //[Authorize(Roles = "Chef")]
  
  
    public class kitchenController : Controller
    {
        private readonly IKitchenService _kitchenService;
        private readonly rmsDbContext _context;

        public kitchenController(rmsDbContext context,IKitchenService kitchenService)
        {
            _context = context;
            _kitchenService = kitchenService;
        } 
        public IActionResult Index()
        {
            return RedirectToAction("OrderManagement");
        }

        public async Task<IActionResult> OrderManagement(
                 TicketStatus? status,
                 string? search,
                 bool delayed = false)
                    {
                        var vm = await _kitchenService.GetOrderManagementAsync(status, search, delayed);
                        return View(vm);
                    }

        public async Task<IActionResult> ViewItems(int orderId)
        {
            var items = await _kitchenService.GetViewItemsAsync(orderId);

            ViewBag.OrderId = orderId;

            return View(items);
        }


        [HttpPost]
        public async Task<IActionResult> MarkItemPrepared(int id)
        {
            var success = await _kitchenService.MarkItemPreparedAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(OrderManagement));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var success = await _kitchenService.CompleteOrderAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(OrderManagement));
        }


        public async Task<IActionResult> RecipeManagement(string tab = "existing")
        {
            var vm = await _kitchenService.GetRecipeManagementAsync(tab);
            return View(vm);
        }

        public async Task<IActionResult> AddRecipe(int id)
        {
            var model = await _kitchenService.GetAddRecipeAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewBag.Ingredients = _context.Ingredients.ToList();
            ViewBag.Units = Enum.GetValues(typeof(UnitOfMeasure))
                                .Cast<UnitOfMeasure>()
                                .ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddRecipe(AddRecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Ingredients = _context.Ingredients.ToList();
                ViewBag.Units = Enum.GetValues(typeof(UnitOfMeasure))
                                    .Cast<UnitOfMeasure>()
                                    .ToList();

                return View(model);
            }

            var success = await _kitchenService.AddRecipeAsync(model);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("RecipeManagement");
        }



        public async Task<IActionResult> ViewRecipe(int menuItemId, string? returnTo)
        {
            var model = await _kitchenService.GetRecipeAsync(menuItemId);
           

            if (model == null)
            {
                return NotFound();
            }
            model.ReturnTo = returnTo;

            ViewBag.Ingredients = _context.Ingredients.ToList();
            ViewBag.Units = Enum.GetValues(typeof(UnitOfMeasure))
                                .Cast<UnitOfMeasure>()
                                .ToList();

            return View("AddRecipe", model);
        }


        public async Task<IActionResult> EditRecipe(int menuItemId)
        {
            var model = await _kitchenService.GetEditRecipeAsync(menuItemId);

            if (model == null)
            {
                return NotFound();
            }

            ViewBag.Ingredients = _context.Ingredients.ToList();
            ViewBag.Units = Enum.GetValues(typeof(UnitOfMeasure))
                                .Cast<UnitOfMeasure>()
                                .ToList();

            return View("AddRecipe", model);
        }


       
    }

}
