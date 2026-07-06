using Microsoft.EntityFrameworkCore;
using GymAkhada.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.Configure<GymAkhada.Models.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<GymAkhada.Models.TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));

builder.Services.AddTransient<GymAkhada.Services.IEmailService, GymAkhada.Services.EmailService>();
builder.Services.AddTransient<GymAkhada.Services.IWhatsAppService, GymAkhada.Services.WhatsAppService>();

// Only one database connection: SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("DefaultConnection is missing in appsettings.json");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.AppUsers.Any(u => u.Role == "Admin"))
    {
        dbContext.AppUsers.Add(new GymAkhada.Models.AppUser
        {
            Username = "admin",
            PasswordHash = GymAkhada.Controllers.AccountController.HashPassword("admin123"),
            Role = "Admin"
        });
        dbContext.SaveChanges();
    }

    var defaultCategories = new[] { "Age Wise", "Weight Wise", "Open for All" };
    foreach (var cat in defaultCategories)
    {
        if (!dbContext.GymCategories.Any(c => c.Gym_categoryName == cat))
        {
            dbContext.GymCategories.Add(new GymAkhada.Models.GymCategory
            {
                Gym_categoryName = cat,
                Gym_Remarks = "System Default"
            });
        }
    }
    dbContext.SaveChanges();
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();