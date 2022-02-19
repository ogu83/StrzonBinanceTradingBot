// using EntityFrameworkCore.EncryptColumn;
// using EntityFrameworkCore.EncryptColumn.Extension;
// using EntityFrameworkCore.EncryptColumn.Interfaces;
// using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StrzonBinanceTradingBot.Data;

public class ApplicationDbContext : IdentityDbContext
{
    // private readonly IEncryptionProvider _provider;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        var b = new DbContextOptionsBuilder<ApplicationDbContext>(options);
        b.EnableSensitiveDataLogging(true);

        // Initialize.EncryptionKey = "StrzonBinanceEncKey";
        // _provider = new GenerateEncryptionProvider();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.UseEncryption(_provider);

        var hashHelper = new PasswordHasher<IdentityUser>();
        var identityUser = new IdentityUser("admin")
        {
            Id = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            UserName = "admin@admin.com",
            NormalizedUserName = "admin@admin.com",
            NormalizedEmail = "admin@admin.com",
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        var hashPassword = hashHelper.HashPassword(identityUser, "Admin12345678*");
        identityUser.PasswordHash = hashPassword;
        modelBuilder.Entity<IdentityUser>().HasData(identityUser);

        modelBuilder.Entity<DemoWalletEntity>()
                    .HasMany(x => x.Balances)
                    .WithOne(x => x.DemoWallet)
                    .HasForeignKey(x => x.DemoWallet_ID);

        modelBuilder.Entity<DemoWalletEntity>()
                    .HasMany(x => x.Trades)
                    .WithOne(x => x.DemoWallet)
                    .HasForeignKey(x => x.DemoWallet_ID);

        modelBuilder.Entity<DemoWalletEntity>()
                    .HasMany(x => x.BalanceHistory)
                    .WithOne(x => x.DemoWallet)
                    .HasForeignKey(x => x.DemoWallet_ID);

        modelBuilder.Entity<BalanceHistoryEntity>()
                    .HasMany(x => x.Rows)
                    .WithOne(x => x.Parent)
                    .HasForeignKey(x => x.Parent_ID);

        modelBuilder.Entity<TradeEntity>().HasIndex(x => new { x.Date, x.Symbol, x.DemoWallet_ID, x.IdentityUserName });
        modelBuilder.Entity<BalanceEntity>().HasIndex(x => new { x.DemoWallet_ID, x.Symbol, x.IdentityUserName });
        modelBuilder.Entity<BinanceCredentialEntity>().HasIndex(x => x.IdentityUserName).IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<BalanceEntity>? Balances { get; set; }

    public DbSet<BalanceHistoryEntity>? BalancesHistory { get; set; }

    public DbSet<BalanceHistoryRow>? BalanceHistoryRows { get; set; }

    public DbSet<DemoWalletEntity>? DemoWallets { get; set; }

    public DbSet<TradeEntity>? Trades { get; set; }

    public DbSet<BinanceCredentialEntity>? BinanceCredentials { get; set; }
}