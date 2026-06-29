using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace Restaurant_Management_System.Data
{
    public class rmsDbContext: DbContext
    {
        public rmsDbContext(DbContextOptions<rmsDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Data Seeding for the User table using explicit primary keys (Id)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Shaik", UserId = "AD01", password = "AD01@123", Role = "Admin" }, // Admin
                new User { Id = 2, Name = "Sai", UserId = "CH01", password = "CH01@123", Role = "Chef" }, // Chef
                new User { Id = 3, Name = "Satya", UserId = "MN01", password = "MN01@123", Role = "Manager" }, // Manager
                new User { Id = 4, Name = "Anusha", UserId = "WA01", password = "WA01@123", Role = "Waiter" }, // Waiter
                new User { Id = 5, Name = "Om Prakash", UserId = "CS01", password = "CS01@123", Role = "Cashier" }, // Cashier
                new User { Id = 6, Name = "Rishab", UserId = "IC01", password = "IC01@123", Role = "Inventory Clerk" }  // Inventory Clerk
            );
        }
    }
}   

      