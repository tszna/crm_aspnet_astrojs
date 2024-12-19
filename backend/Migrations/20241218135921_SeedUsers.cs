using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backendCRM.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmailVerifiedAt", "Name", "Password", "RememberToken", "UpdatedAt" },
                values: new object[,]
                {
                    { 1ul, new DateTime(2024, 12, 18, 13, 59, 20, 320, DateTimeKind.Utc).AddTicks(6733), "john@example.com", null, "John Doe", "$2a$11$p6YA6hbX52AnQB1uEYpCiOEM2jW/RWB5RZVy9XlSH7T5WC5uWcq4.", null, new DateTime(2024, 12, 18, 13, 59, 20, 320, DateTimeKind.Utc).AddTicks(6733) },
                    { 2ul, new DateTime(2024, 12, 18, 13, 59, 20, 320, DateTimeKind.Utc).AddTicks(6733), "john1@example.com", null, "John Doe1", "$2a$11$roQ4IO66aWQwAmVyOydIG.z1CyU3fPpv/4gK0SS8OluXfeApfdBda", null, new DateTime(2024, 12, 18, 13, 59, 20, 320, DateTimeKind.Utc).AddTicks(6733) },
                    { 3ul, new DateTime(2024, 12, 18, 13, 59, 20, 320, DateTimeKind.Utc).AddTicks(6733), "john2@example.com", null, "John Doe2", "$2a$11$LluIxGJg3orvCSiKirNFz.cSnHiqVLErp4i6AgKmHDciXMc1HPRUO", null, new DateTime(2024, 12, 18, 13, 59, 20, 320, DateTimeKind.Utc).AddTicks(6733) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1ul);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 2ul);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 3ul);
        }
    }
}
