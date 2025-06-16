using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebApplication1.Data;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// Add Razor view support
builder.Services.AddControllersWithViews();

// SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));

// Redis singleton
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect($"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}")
);
builder.Services.AddScoped<RedisService>();

// HTTP client for FastAPI
builder.Services.AddHttpClient<FastApiClient>();

var app = builder.Build();

// Serve static files (wwwroot)
app.UseStaticFiles();

// Enable routing and MVC
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
