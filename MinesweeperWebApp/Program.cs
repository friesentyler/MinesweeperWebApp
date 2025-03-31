using MinesweeperWebApp.Data;
using MinesweeperWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// COMMENT AND UNCOMMENT AS NEEDED 
// TO TOGGLE BETWEEN USER DAO AND USER COLLECTION FOR NON-DB TESTING

//builder.Services.AddSingleton<IUserManager, UserCollection>();
builder.Services.AddSingleton<IUserManager, UserDAO>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddDistributedMemoryCache(); // For storing session data in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Configure the HTTP request pipeline.
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enable HSTS for production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session management
app.UseSession(); // Added to enable sessions and keep login state

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
