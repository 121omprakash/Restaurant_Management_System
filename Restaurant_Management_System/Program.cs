using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Restaurant_Management_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetSection("ConnectionStrings")["MyConn"];
builder.Services.AddDbContext<rmsDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddScoped<IAdminService, AdminService>();

//added for authentication and authorization

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/User/Login"; // Redirect to login page if not authenticated
//        options.AccessDeniedPath = "/User/AccessDenied"; // Redirect to access denied page if not authorized
//    });
//builder.Services.AddAuthorization();

////added for authentication and authorization
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IInventory, InventoryService>();
builder.Services.AddScoped<IKitchenService, KitchenService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())       
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Login}/{id?}");

app.Run();
