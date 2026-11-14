using DishDrop.Data;
using DishDrop.Data.Entities;
using DishDrop.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is missing or empty.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DatabaseSeeder>();

var app = builder.Build();

// Seeding is opt-in and never applies migrations. No HTTP server is started in this mode.
if (args.Contains("--seed-reference") || args.Contains("--seed-demo"))
{
    var demo = args.Contains("--seed-demo");
    if (demo && !(app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing")))
        throw new InvalidOperationException("Demo seeding is allowed only in Development or Testing.");
    await using var scope = app.Services.CreateAsyncScope();
    var password = builder.Configuration["Seed:DemoPassword"];
    await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync(demo, password);
    Console.WriteLine(demo && string.IsNullOrWhiteSpace(password)
        ? "Reference seed complete. Demo users skipped: Seed:DemoPassword is not configured."
        : "Requested seed complete.");
    return;
}

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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();

public partial class Program { }
