using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b919a10-f029-49d4-ba25-de6b65c6d63c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "657eb63e-a1b1-4ff3-ab13-c994406d4e92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e45e24a-cdc8-4f2a-8900-923270a98a49");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f7edfa1-1117-412d-90e2-ab32ba8a1843", null, "User", "USER" },
                    { "1401d4e0-b488-4a28-b123-53d7bfe39895", null, "Admin", "ADMIN" },
                    { "ddb01eba-900b-40ff-b729-16477b58029e", null, "Host", "HOST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f7edfa1-1117-412d-90e2-ab32ba8a1843");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1401d4e0-b488-4a28-b123-53d7bfe39895");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddb01eba-900b-40ff-b729-16477b58029e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b919a10-f029-49d4-ba25-de6b65c6d63c", null, "Host", "HOST" },
                    { "657eb63e-a1b1-4ff3-ab13-c994406d4e92", null, "Admin", "ADMIN" },
                    { "6e45e24a-cdc8-4f2a-8900-923270a98a49", null, "User", "USER" }
                });
        }
    }
}
