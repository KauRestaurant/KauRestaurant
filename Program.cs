using KauRestaurant.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KauRestaurant.Areas.Identity.Data;
using KauRestaurant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<KauRestaurantUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddErrorDescriber<ArabicIdentityErrorDescriber>()    // ← Arabic messages
    .AddRoles<IdentityRole>() // line to enable roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddScoped<TicketQrService>();

builder.Services.AddScoped<TicketPriceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await IdentityDataInitializer.SeedRolesAndAdminUser(app.Services);
}

app.MapControllerRoute(
    name: "userControllers",
    pattern: "User/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "User" }
);

app.Run();
