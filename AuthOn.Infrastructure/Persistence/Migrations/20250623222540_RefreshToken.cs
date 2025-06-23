using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthOn.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)1,
                columns: new[] { "ExpirationTimeInHours", "Name" },
                values: new object[] { 15, "ACCESS_TOKEN" });

            migrationBuilder.InsertData(
                table: "TokenTypes",
                columns: new[] { "Id", "ExpirationTimeInHours", "Name" },
                values: new object[,]
                {
                    { (byte)2, 48, "ACTIVATION_TOKEN" },
                    { (byte)3, 720, "REFRESH_TOKEN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)3);

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)1,
                columns: new[] { "ExpirationTimeInHours", "Name" },
                values: new object[] { 48, "ACTIVATION_TOKEN" });
        }
    }
}
