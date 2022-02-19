using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StrzonBinanceTradingBot.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinanceCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: true),
                    ApiSecret = table.Column<string>(type: "TEXT", nullable: true),
                    IdentityUserName = table.Column<string>(type: "TEXT", nullable: true),
                    IdentityUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinanceCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinanceCredentials_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DemoWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USDTBalance = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    IdentityUserName = table.Column<string>(type: "TEXT", nullable: true),
                    IdentityUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoWallets_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "varchar(20)", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    USDTRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    LockUSDTRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    LockDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdentityUserName = table.Column<string>(type: "TEXT", nullable: true),
                    IdentityUserId = table.Column<string>(type: "TEXT", nullable: true),
                    DemoWallet_ID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balances_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Balances_DemoWallets_DemoWallet_ID",
                        column: x => x.DemoWallet_ID,
                        principalTable: "DemoWallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BalancesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Event = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    USDTBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    IdentityUserName = table.Column<string>(type: "TEXT", nullable: true),
                    IdentityUserId = table.Column<string>(type: "TEXT", nullable: true),
                    DemoWallet_ID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalancesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalancesHistory_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BalancesHistory_DemoWallets_DemoWallet_ID",
                        column: x => x.DemoWallet_ID,
                        principalTable: "DemoWallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(20)", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    USDTRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    IdentityUserName = table.Column<string>(type: "TEXT", nullable: true),
                    IdentityUserId = table.Column<string>(type: "TEXT", nullable: true),
                    DemoWallet_ID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trades_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trades_DemoWallets_DemoWallet_ID",
                        column: x => x.DemoWallet_ID,
                        principalTable: "DemoWallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BalanceHistoryRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    USDTRate = table.Column<decimal>(type: "TEXT", precision: 18, scale: 18, nullable: false),
                    Parent_ID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceHistoryRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceHistoryRows_BalancesHistory_Parent_ID",
                        column: x => x.Parent_ID,
                        principalTable: "BalancesHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin", 0, "78a4772b-23a3-4aa9-9ae5-0915eecb32b6", "admin@admin.com", true, false, null, "admin@admin.com", "admin@admin.com", "AQAAAAEAACcQAAAAECnjbjTwkpLgUGTLWgAs4rRKcuMuzvdbz141yYvNLF8n4PiVbRh3PAdpAmF4RaN5iw==", null, false, "7fce102e-055b-4b4b-b5f3-36faf29524ea", false, "admin@admin.com" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceHistoryRows_Parent_ID",
                table: "BalanceHistoryRows",
                column: "Parent_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_DemoWallet_ID_Symbol_IdentityUserName",
                table: "Balances",
                columns: new[] { "DemoWallet_ID", "Symbol", "IdentityUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_Balances_IdentityUserId",
                table: "Balances",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BalancesHistory_DemoWallet_ID",
                table: "BalancesHistory",
                column: "DemoWallet_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BalancesHistory_IdentityUserId",
                table: "BalancesHistory",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BinanceCredentials_IdentityUserId",
                table: "BinanceCredentials",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BinanceCredentials_IdentityUserName",
                table: "BinanceCredentials",
                column: "IdentityUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemoWallets_IdentityUserId",
                table: "DemoWallets",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_Date_Symbol_DemoWallet_ID_IdentityUserName",
                table: "Trades",
                columns: new[] { "Date", "Symbol", "DemoWallet_ID", "IdentityUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_Trades_DemoWallet_ID",
                table: "Trades",
                column: "DemoWallet_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_IdentityUserId",
                table: "Trades",
                column: "IdentityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BalanceHistoryRows");

            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.DropTable(
                name: "BinanceCredentials");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "BalancesHistory");

            migrationBuilder.DropTable(
                name: "DemoWallets");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
