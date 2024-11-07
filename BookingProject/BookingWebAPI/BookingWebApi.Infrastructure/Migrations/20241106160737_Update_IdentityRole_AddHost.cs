using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_IdentityRole_AddHost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e9a2761-9fa9-4e16-8451-c952299a2111");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "589a8fe3-3cc3-43ee-aeb1-0b438bc04132");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "0e9a2761-9fa9-4e16-8451-c952299a2111", null, "User", "USER" },
                    { "589a8fe3-3cc3-43ee-aeb1-0b438bc04132", null, "Admin", "ADMIN" }
                });
        }
    }
}
