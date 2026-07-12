using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Models;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Identity.Client;

namespace Restaurant_Management_System.Data
{
    public class rmsDbContext: DbContext
    {
        public rmsDbContext(DbContextOptions<rmsDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; } 
        public DbSet<ItemRecipe> ItemRecipes { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BillInvoice> BillInvoices { get; set; }
        public DbSet<TableStatus> TableStatuses { get; set; }
        public DbSet<KitchenTicket> KitchenTickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Data Seeding for the User table using explicit primary keys (Id)
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Shaik", EmpId = "AD01", password = "AD01@123", Role = "Admin", IsActive= true}, // Admin
                new Employee { Id = 2, Name = "Sai", EmpId = "CH01", password = "CH01@123", Role = "Chef", IsActive = true }, // Chef
                new Employee { Id = 3, Name = "Satya", EmpId = "MN01", password = "MN01@123", Role = "Manager", IsActive = true }, // Manager
                new Employee { Id = 4, Name = "Anusha", EmpId = "WA01", password = "WA01@123", Role = "Waiter", IsActive = true }, // Waiter
                new Employee { Id = 5, Name = "Om Prakash", EmpId = "CS01", password = "CS01@123", Role = "Cashier" , IsActive = true }, // Cashier
                new Employee { Id = 6, Name = "Rishab", EmpId = "IC01", password = "IC01@123", Role = "Inventory Clerk" , IsActive = true }  // Inventory Clerk
            );
        }
    }
}   

      