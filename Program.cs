using Binance.Net;
using Binance.Net.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StrzonBinanceTradingBot.Data;
using StrzonBinanceTradingBot.Helpers;
using StrzonBinanceTradingBot.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
                {
                    // Use the default property (Pascal) casing.
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

var settings = new Settings
{
    aesKey = builder.Configuration.GetSection("AESEncryptionKey").Value.ToString(),
    scheduledTaskInterval = short.Parse(builder.Configuration.GetSection("ScheduledTaskInterval").Value),
    maxAllowedSlipage = decimal.Parse(builder.Configuration.GetSection("MaxAllowedSlipage").Value),
    demoWalletInitialBalance = decimal.Parse(builder.Configuration.GetSection("DemoWalletInitialBalance").Value)
};
builder.Services.AddSingleton<Settings>(settings);

builder.Services.AddTransient<IBinanceClient, BinanceClient>();
builder.Services.AddTransient<IAES>(x => ActivatorUtilities.CreateInstance<AES>(x, settings.aesKey));
builder.Services.AddTransient<IBinanceCredentialService, BinanceCredentialService>();
builder.Services.AddTransient<IDemoWalletService, DemoWalletService>();
builder.Services.AddTransient<IBalanceService, BalanceService>();
builder.Services.AddTransient<ITradeService, TradeService>();

builder.Services.AddHostedService<ConsumeScopedServiceHostedService>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
