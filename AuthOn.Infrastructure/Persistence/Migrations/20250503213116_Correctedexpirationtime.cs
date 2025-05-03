using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthOn.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Correctedexpirationtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationTime",
                table: "TokenTypes");

            migrationBuilder.AddColumn<int>(
                name: "ExpirationTimeInHours",
                table: "TokenTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)1,
                column: "ExpirationTimeInHours",
                value: 48);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationTimeInHours",
                table: "TokenTypes");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpirationTime",
                table: "TokenTypes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "TokenTypes",
                keyColumn: "Id",
                keyValue: (byte)1,
                column: "ExpirationTime",
                value: new TimeSpan(2, 0, 0, 0, 0));
        }
    }
}
