using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.ENUM;
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
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<KitchenTicket> KitchenTickets { get; set; }

        public DbSet<RestaurantProfile> RestaurantProfiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Store enum values as strings in the database to match existing DB data
            modelBuilder.Entity<Models.Ingredient>()
                .Property(i => i.StockStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Models.Ingredient>()
                .Property(i => i.UnitOfMeasure)
                .HasConversion<string>();

            // Fluent API configuration for uniqueness
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmpId)
                .IsUnique();

            modelBuilder.Entity<Ingredient>()
               .HasIndex(i => i.IngredientName)
               .IsUnique();

            modelBuilder.Entity<MenuItem>()
                .HasIndex(m => m.ItemName)
                .IsUnique();



            // Data Seeding for the User table using explicit primary keys (Id)
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Shaik", EmpId = "AD01", password = "AD01@123", Role = "Admin", IsActive= true}, // Admin
                new Employee { Id = 2, Name = "Sai", EmpId = "CH01", password = "CH01@123", Role = "Chef", IsActive = true }, // Chef
                new Employee { Id = 3, Name = "Satya", EmpId = "MN01", password = "MN01@123", Role = "Manager", IsActive = true }, // Manager
                new Employee { Id = 4, Name = "Anusha", EmpId = "WA01", password = "WA01@123", Role = "Waiter", IsActive = true }, // Waiter
                new Employee { Id = 5, Name = "Om Prakash", EmpId = "CS01", password = "CS01@123", Role = "Cashier" , IsActive = true }, // Cashier
                new Employee { Id = 6, Name = "Rishab", EmpId = "IC01", password = "IC01@123", Role = "Inventory Clerk" , IsActive = true }  // Inventory Clerk
            );


            // 2. Data Seeding for the Global System Settings Table
            modelBuilder.Entity<RestaurantProfile>().HasData(
                new RestaurantProfile
                {
                    Id = 1,
                    RestaurantName = "Pizza Hub",
                    PrimaryPhone = "+91 987654321",
                    CorporateEmail = "operations@pizzahub.com",
                    PhysicalAddress = "1024, Banjara Hills, Hyderabad, Telangana",
                    TaxIdentifier = "GSTIN9283471029B1Z4",
                    BaseCgstPercentage = 9.00m,
                    BaseSgstPercentage = 9.00m
                }
            );

            // Seed 10 restaurant tables: 6 available (IsOccupied = false) and 4 occupied (IsOccupied = true)
            modelBuilder.Entity<Models.RestaurantTable>().HasData(
                new Models.RestaurantTable { TableNumber = "T01", IsOccupied = false },
                new Models.RestaurantTable { TableNumber = "T02", IsOccupied = false },
                new Models.RestaurantTable { TableNumber = "T03", IsOccupied = false },
                new Models.RestaurantTable { TableNumber = "T04", IsOccupied = false },
                new Models.RestaurantTable { TableNumber = "T05", IsOccupied = false },
                new Models.RestaurantTable { TableNumber = "T06", IsOccupied = false },
                new Models.RestaurantTable { TableNumber = "T07", IsOccupied = true },
                new Models.RestaurantTable { TableNumber = "T08", IsOccupied = true },
                new Models.RestaurantTable { TableNumber = "T09", IsOccupied = true },
                new Models.RestaurantTable { TableNumber = "T10", IsOccupied = true }
            );

            // Seed Menu Items
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { MenuItemId = 1, ItemName = "BBQ Chicken Pizza", Category = "Pizza", Price = 550.00m, PreparationTime = 20, ItemStatus = ItemStatus.AVAILABLE },
                new MenuItem { MenuItemId = 2, ItemName = "Farm House Pizza", Category = "Pizza", Price = 500.00m, PreparationTime = 18, ItemStatus = ItemStatus.AVAILABLE },
                new MenuItem { MenuItemId = 3, ItemName = "Margherita", Category = "Pizza", Price = 350.00m, PreparationTime = 15, ItemStatus = ItemStatus.AVAILABLE },
                new MenuItem { MenuItemId = 4, ItemName = "Fries", Category = "Sides", Price = 120.00m, PreparationTime = 8, ItemStatus = ItemStatus.AVAILABLE },
                new MenuItem { MenuItemId = 5, ItemName = "Coke", Category = "Beverage", Price = 50.00m, PreparationTime = 1, ItemStatus = ItemStatus.AVAILABLE },
                new MenuItem { MenuItemId = 6, ItemName = "Garlic Bread", Category = "Sides", Price = 90.00m, PreparationTime = 7, ItemStatus = ItemStatus.AVAILABLE }
            );

            // Seed Ingredients
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { IngredientId = 1, IngredientName = "Flour", UnitOfMeasure = UnitOfMeasure.KG, CurrentStock = 100.00m, ReorderLevel = 20.00m, StockStatus = StockStatus.AVAILABLE },
                new Ingredient { IngredientId = 2, IngredientName = "Chicken", UnitOfMeasure = UnitOfMeasure.KG, CurrentStock = 50.00m, ReorderLevel = 10.00m, StockStatus = StockStatus.AVAILABLE },
                new Ingredient { IngredientId = 3, IngredientName = "Cheese", UnitOfMeasure = UnitOfMeasure.KG, CurrentStock = 40.00m, ReorderLevel = 10.00m, StockStatus = StockStatus.AVAILABLE },
                new Ingredient { IngredientId = 4, IngredientName = "Potato", UnitOfMeasure = UnitOfMeasure.KG, CurrentStock = 60.00m, ReorderLevel = 15.00m, StockStatus = StockStatus.AVAILABLE },
                new Ingredient { IngredientId = 5, IngredientName = "Tomato", UnitOfMeasure = UnitOfMeasure.KG, CurrentStock = 30.00m, ReorderLevel = 5.00m, StockStatus = StockStatus.LOW },
                new Ingredient { IngredientId = 6, IngredientName = "Garlic", UnitOfMeasure = UnitOfMeasure.G, CurrentStock = 500.00m, ReorderLevel = 100.00m, StockStatus = StockStatus.AVAILABLE },
                new Ingredient { IngredientId = 7, IngredientName = "Oil", UnitOfMeasure = UnitOfMeasure.L, CurrentStock = 10.00m, ReorderLevel = 2.00m, StockStatus = StockStatus.AVAILABLE },
                new Ingredient { IngredientId = 8, IngredientName = "Sugar", UnitOfMeasure = UnitOfMeasure.KG, CurrentStock = 20.00m, ReorderLevel = 5.00m, StockStatus = StockStatus.AVAILABLE }
            );

            // Seed Item Recipes (map menu items to ingredients)
            modelBuilder.Entity<ItemRecipe>().HasData(
                new ItemRecipe { RecipeId = 1, MenuItemId = 1, IngredientId = 1, Quantity = 0.30m },
                new ItemRecipe { RecipeId = 2, MenuItemId = 1, IngredientId = 2, Quantity = 0.20m },
                new ItemRecipe { RecipeId = 3, MenuItemId = 1, IngredientId = 3, Quantity = 0.15m },
                new ItemRecipe { RecipeId = 4, MenuItemId = 2, IngredientId = 1, Quantity = 0.28m },
                new ItemRecipe { RecipeId = 5, MenuItemId = 2, IngredientId = 3, Quantity = 0.12m },
                new ItemRecipe { RecipeId = 6, MenuItemId = 4, IngredientId = 4, Quantity = 0.25m },
                new ItemRecipe { RecipeId = 7, MenuItemId = 5, IngredientId = 8, Quantity = 0.33m },
                new ItemRecipe { RecipeId = 8, MenuItemId = 6, IngredientId = 6, Quantity = 0.02m }
            );

            // Seed Customer Orders
            modelBuilder.Entity<CustomerOrder>().HasData(
                new CustomerOrder { OrderId = 1001, TableNumber = "T07", CustomerName = "Ravi", OrderType = OrderType.DINE_IN, OrderTime = new DateTime(2026,5,21,12,30,0), OrderStatus = OrderStatus.SERVED },
                new CustomerOrder { OrderId = 1002, TableNumber = "T08", CustomerName = "Anita", OrderType = OrderType.DINE_IN, OrderTime = new DateTime(2026,5,21,13,15,0), OrderStatus = OrderStatus.PREPARING },
                new CustomerOrder { OrderId = 1003, TableNumber = "T03", CustomerName = "Karan", OrderType = OrderType.TAKEAWAY, OrderTime = new DateTime(2026,5,21,13,45,0), OrderStatus = OrderStatus.NEW },
                new CustomerOrder { OrderId = 1004, TableNumber = "T02", CustomerName = "Sita", OrderType = OrderType.DELIVERY, OrderTime = new DateTime(2026,5,20,19,0,0), OrderStatus = OrderStatus.CANCEL }
            );

            // Seed Order Items
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId = 2001, OrderId = 1001, MenuItemId = 1, Quantity = 2, Price = 550.00m },
                new OrderItem { OrderItemId = 2002, OrderId = 1001, MenuItemId = 4, Quantity = 1, Price = 120.00m },
                new OrderItem { OrderItemId = 2003, OrderId = 1002, MenuItemId = 2, Quantity = 1, Price = 500.00m },
                new OrderItem { OrderItemId = 2004, OrderId = 1003, MenuItemId = 4, Quantity = 2, Price = 120.00m },
                new OrderItem { OrderItemId = 2005, OrderId = 1004, MenuItemId = 3, Quantity = 1, Price = 350.00m }
            );

            // Seed Bill Invoices
            modelBuilder.Entity<BillInvoice>().HasData(
                new BillInvoice { InvoiceId = 5001, OrderId = 1001, SubtotalAmount = 1220.00m, TaxAmount = 219.60m, TipAmount = 50.00m, TotalAmount = 1489.60m, PaymentStatus = PaymentStatus.PAID },
                new BillInvoice { InvoiceId = 5002, OrderId = 1002, SubtotalAmount = 500.00m, TaxAmount = 90.00m, TipAmount = 0.00m, TotalAmount = 590.00m, PaymentStatus = PaymentStatus.PENDING },
                new BillInvoice { InvoiceId = 5003, OrderId = 1003, SubtotalAmount = 240.00m, TaxAmount = 43.20m, TipAmount = 0.00m, TotalAmount = 283.20m, PaymentStatus = PaymentStatus.PAID },
                new BillInvoice { InvoiceId = 5004, OrderId = 1004, SubtotalAmount = 350.00m, TaxAmount = 63.00m, TipAmount = 0.00m, TotalAmount = 413.00m, PaymentStatus = PaymentStatus.REFUNDED }
            );

            // Seed Kitchen Tickets
            modelBuilder.Entity<KitchenTicket>().HasData(
                new KitchenTicket { TicketId = 3001, OrderId = 1002, Station = "Pizza Oven", AssignedChef = "Sai", StartTime = new DateTime(2026,5,21,13,20,0), CompletionTime = null, TicketStatus = TicketStatus.IN_PROGRESS },
                new KitchenTicket { TicketId = 3002, OrderId = 1001, Station = "Grill", AssignedChef = "Rishab", StartTime = new DateTime(2026,5,21,12,35,0), CompletionTime = new DateTime(2026,5,21,12,50,0), TicketStatus = TicketStatus.SERVED },
                new KitchenTicket { TicketId = 3003, OrderId = 1003, Station = "Fryer", AssignedChef = null, StartTime = null, CompletionTime = null, TicketStatus = TicketStatus.QUEUED },
                new KitchenTicket { TicketId = 3004, OrderId = 1004, Station = "Oven", AssignedChef = null, StartTime = null, CompletionTime = null, TicketStatus = TicketStatus.READY }
            );
        }
    }
}   

      