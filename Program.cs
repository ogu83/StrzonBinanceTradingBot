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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBinanceClient, BinanceClient>();

var aesKey = builder.Configuration.GetSection("AESEncryptionKey").Value.ToString();
builder.Services.AddSingleton<IAES>(x => ActivatorUtilities.CreateInstance<AES>(x, aesKey));

builder.Services.AddTransient<IBinanceCredentialService, BinanceCredentialService>();
builder.Services.AddTransient<ITradeService, TradeService>();
builder.Services.AddTransient<IBalanceService, BalanceService>();
builder.Services.AddTransient<IDemoWalletService, DemoWalletService>();

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
