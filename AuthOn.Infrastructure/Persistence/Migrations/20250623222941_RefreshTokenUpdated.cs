using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthOn.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ExpirationTimeInHours",
                table: "TokenTypes",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)1,
                column: "ExpirationTimeInHours",
                value: 0.25);

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)2,
                column: "ExpirationTimeInHours",
                value: 48.0);

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)3,
                column: "ExpirationTimeInHours",
                value: 720.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExpirationTimeInHours",
                table: "TokenTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)1,
                column: "ExpirationTimeInHours",
                value: 15);

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)2,
                column: "ExpirationTimeInHours",
                value: 48);

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)3,
                column: "ExpirationTimeInHours",
                value: 720);
        }
    }
}
