// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StrzonBinanceTradingBot.Data;

#nullable disable

namespace StrzonBinanceTradingBot.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220219193520_FirstDBSchema")]
    partial class FirstDBSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "admin",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fd267d65-703d-4923-b897-8efd9ea77cbb",
                            Email = "admin@admin.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "admin@admin.com",
                            NormalizedUserName = "admin@admin.com",
                            PasswordHash = "AQAAAAEAACcQAAAAEMR4M8A5DOomyB1eaSAQOcXMPOF5wvjgtbn31UUEAZKvu0edBCwAXxzHsaeUOs2u0A==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "74d34d14-23fe-4725-bafb-2754d297ee93",
                            TwoFactorEnabled = false,
                            UserName = "admin@admin.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.Property<int?>("DemoWallet_ID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("LockAmount")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LockDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("LockUSDTRate")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .HasColumnType("varchar(20)");

                    b.Property<decimal>("USDTRate")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.HasIndex("DemoWallet_ID", "Symbol", "IdentityUserName");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceHistoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DemoWallet_ID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Event")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserName")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("USDTBalance")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DemoWallet_ID");

                    b.HasIndex("IdentityUserId");

                    b.ToTable("BalancesHistory");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceHistoryRow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.Property<int?>("Parent_ID")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("USDTRate")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Parent_ID");

                    b.ToTable("BalanceHistoryRows");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BinanceCredentialEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApiSecret")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.HasIndex("IdentityUserName")
                        .IsUnique();

                    b.ToTable("BinanceCredentials");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.DemoWalletEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserName")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("USDTBalance")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.ToTable("DemoWallets");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.TradeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DemoWallet_ID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("USDTRate")
                        .HasPrecision(18, 18)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DemoWallet_ID");

                    b.HasIndex("IdentityUserId");

                    b.HasIndex("Date", "Symbol", "DemoWallet_ID", "IdentityUserName");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceEntity", b =>
                {
                    b.HasOne("StrzonBinanceTradingBot.Data.DemoWalletEntity", "DemoWallet")
                        .WithMany("Balances")
                        .HasForeignKey("DemoWallet_ID");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("DemoWallet");

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceHistoryEntity", b =>
                {
                    b.HasOne("StrzonBinanceTradingBot.Data.DemoWalletEntity", "DemoWallet")
                        .WithMany("BalanceHistory")
                        .HasForeignKey("DemoWallet_ID");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("DemoWallet");

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceHistoryRow", b =>
                {
                    b.HasOne("StrzonBinanceTradingBot.Data.BalanceHistoryEntity", "Parent")
                        .WithMany("Rows")
                        .HasForeignKey("Parent_ID");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BinanceCredentialEntity", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.DemoWalletEntity", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.TradeEntity", b =>
                {
                    b.HasOne("StrzonBinanceTradingBot.Data.DemoWalletEntity", "DemoWallet")
                        .WithMany("Trades")
                        .HasForeignKey("DemoWallet_ID");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("DemoWallet");

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.BalanceHistoryEntity", b =>
                {
                    b.Navigation("Rows");
                });

            modelBuilder.Entity("StrzonBinanceTradingBot.Data.DemoWalletEntity", b =>
                {
                    b.Navigation("BalanceHistory");

                    b.Navigation("Balances");

                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
