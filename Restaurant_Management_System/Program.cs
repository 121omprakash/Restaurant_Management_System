using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetSection("ConnectionStrings")["MyConn"];
builder.Services.AddDbContext<rmsDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IInventory, InventoryService>();
builder.Services.AddScoped<IKitchenService, KitchenService>();
builder.Services.AddScoped<IManagerService, ManagerService>();

// Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Restaurant Management System API",
        Description = "Admin API endpoints"
    });
    // Load XML documentation comments for Swagger UI
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    // FIX FOR 500 FETCH ERROR: Prevents Swagger from crashing on duplicate route actions
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CLI Command: dotnet run --seed-admin <EmpId> <Password> <Name>
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