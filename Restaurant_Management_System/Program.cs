using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetSection("ConnectionStrings")["MyConn"];
builder.Services.AddDbContext<rmsDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

//added for authentication and authorization

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/User/Login"; // Redirect to login page if not authenticated
//        options.AccessDeniedPath = "/User/AccessDenied"; // Redirect to access denied page if not authorized
//    });
//builder.Services.AddAuthorization();

builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IInventory, InventoryService>();
builder.Services.AddScoped<IKitchenService, KitchenService>();
builder.Services.AddScoped<IManagerService, ManagerService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())       
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// CLI Command: dotnet run --seed-admin <EmpId> <Password> <Name>
// Example:     dotnet run --seed-admin AD01 AD01@123 Shaik
if (args.Length >= 4 && args[0] == "--seed-admin")
{
    string empId = args[1];
    string plainPassword = args[2];
    string name = args[3];

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<rmsDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Employee>>();

        if (db.Employees.Any(e => e.EmpId == empId || e.Role == "Admin"))
        {
            Console.WriteLine($"[WARNING] Admin or employee with ID '{empId}' already exists.");
            return;
        }

        var admin = new Employee
        {
            Name = name,
            EmpId = empId,
            Role = "Admin",
            IsActive = true
        };
        admin.password = hasher.HashPassword(admin, plainPassword);

        db.Employees.Add(admin);
        db.SaveChanges();

        Console.WriteLine($"[SUCCESS] Admin '{name}' ({empId}) created successfully!");
    }
    return; // Stop app execution after creating user
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Login}/{id?}");

app.Run();
