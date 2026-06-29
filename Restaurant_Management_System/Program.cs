using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetSection("ConnectionStrings")["MyConn"];
builder.Services.AddDbContext<rmsDbContext>(options => options.UseSqlServer(connectionString));

//added for authentication and authorization

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/User/Login"; // Redirect to login page if not authenticated
//        options.AccessDeniedPath = "/User/AccessDenied"; // Redirect to access denied page if not authorized
//    });
//builder.Services.AddAuthorization();

////added for authentication and authorization

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
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
