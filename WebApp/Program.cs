using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'AppDBContextConnection' not found.");

// Add DbContext with SQL Server
//builder.Services.AddDbContext<AppDBContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDbContextFactory<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Add Identity services
builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDBContext>();

// Add MVC controllers and views
builder.Services.AddControllersWithViews();

// Optional: Add Razor Pages if you scaffold Identity pages
builder.Services.AddRazorPages();

builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(cfg => {                   
    cfg.Cookie.Name = "quocnhan";            
    cfg.IdleTimeout = new TimeSpan(0, 30, 0);    
});

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// **Add authentication and authorization middleware**
app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages for Identity UI
app.MapRazorPages();

app.Run();
