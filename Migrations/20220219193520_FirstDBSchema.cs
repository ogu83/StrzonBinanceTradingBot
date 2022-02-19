using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StrzonBinanceTradingBot.Migrations
{
    public partial class FirstDBSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd267d65-703d-4923-b897-8efd9ea77cbb", "AQAAAAEAACcQAAAAEMR4M8A5DOomyB1eaSAQOcXMPOF5wvjgtbn31UUEAZKvu0edBCwAXxzHsaeUOs2u0A==", "74d34d14-23fe-4725-bafb-2754d297ee93" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78a4772b-23a3-4aa9-9ae5-0915eecb32b6", "AQAAAAEAACcQAAAAECnjbjTwkpLgUGTLWgAs4rRKcuMuzvdbz141yYvNLF8n4PiVbRh3PAdpAmF4RaN5iw==", "7fce102e-055b-4b4b-b5f3-36faf29524ea" });
        }
    }
}
