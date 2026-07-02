    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
namespace Restaurant_Management_System.Controllers
    {

        public class AdminController : Controller
        {
            // 1. Declare  Database Context variable
            private readonly rmsDbContext _context;

            // 2. Inject the context through the constructor
            public AdminController(rmsDbContext context)
            {
                _context = context;
            }

        public IActionResult Dashboard()
            {
                return View();
            }

            public IActionResult Users()
            {
                // Fetch all users from your "Users" SQL table
                var usersList = _context.Users.ToList();

                // Pass the populated list straight into the View
                return View(usersList);
        }

            public IActionResult CreateUser()
            {
                return View();
            }

            public IActionResult EditUser(int id)
            {
                return View();
            }

            public IActionResult Details(int id)
            {
                return View();
            }

            public IActionResult Settings()
            {
                return View();
            }
        }
    }
