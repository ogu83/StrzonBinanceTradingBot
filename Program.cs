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
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Scoped);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var settings = new Settings
{
    aesKey = builder.Configuration.GetSection("AESEncryptionKey").Value.ToString(),
    scheduledTaskInterval = short.Parse(builder.Configuration.GetSection("ScheduledTaskInterval").Value),
    maxAllowedSlipage = decimal.Parse(builder.Configuration.GetSection("MaxAllowedSlipage").Value)
};
builder.Services.AddSingleton<Settings>(settings);

builder.Services.AddScoped<IBinanceClient, BinanceClient>();
builder.Services.AddScoped<IAES>(x => ActivatorUtilities.CreateInstance<AES>(x, settings.aesKey));
builder.Services.AddScoped<IBinanceCredentialService, BinanceCredentialService>();
builder.Services.AddScoped<IDemoWalletService, DemoWalletService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<ITradeService, TradeService>();

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
