using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Services;
using Restaurant_Management_System.ViewModel;

namespace Restaurant_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Employee Management Dashboard
        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            var dashboardData = await _adminService.GetEmployeeDashboardDataAsync();
            return View(dashboardData);
        }

        // Create Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please enter valid employee details.";
                return RedirectToAction(nameof(Employees));
            }

            var (_, message) = await _adminService.CreateEmployeeAsync(employee);
            TempData["Message"] = message;

            return RedirectToAction(nameof(Employees));
        }

        // Update Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel updatedEmployee)
        {
            var (_, message) = await _adminService.UpdateEmployeeAsync(updatedEmployee);
            TempData["Message"] = message;

            return RedirectToAction(nameof(Employees));
        }

        // Activate / Deactivate Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var (_, message) = await _adminService.ToggleEmployeeStatusAsync(id);
            TempData["Message"] = message;

            return RedirectToAction(nameof(Employees));
        }

        // Restaurant Profile
        [HttpGet]
        public async Task<IActionResult> RestaurantProfile()
        {
            var profile = await _adminService.GetRestaurantProfileAsync();
            return View(profile);
        }

        // Update Restaurant Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRestaurantProfile(RestaurantProfile updatedProfile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please enter valid details.";
                return RedirectToAction(nameof(RestaurantProfile));
            }

            var (_, message) = await _adminService.UpdateRestaurantProfileAsync(updatedProfile);
            TempData["Message"] = message;

            return RedirectToAction(nameof(RestaurantProfile));
        }
    }
}