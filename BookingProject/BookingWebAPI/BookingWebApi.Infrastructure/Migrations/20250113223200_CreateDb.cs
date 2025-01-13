using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65896f02-07d4-415d-9ad1-d0d379d4e44a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72621007-cc68-49f8-87c7-528be7df7ca8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c10d70f1-7c72-4f36-bb0d-411fbe5e8006");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "225149a6-e4f2-4492-8a3d-1d534f943ac9", null, "User", "USER" },
                    { "7df3b5ba-184f-42ae-9c98-438ddcd89e8f", null, "Admin", "ADMIN" },
                    { "8921fabf-1d56-4c41-be0b-c0e7033c31d7", null, "Host", "HOST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "225149a6-e4f2-4492-8a3d-1d534f943ac9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7df3b5ba-184f-42ae-9c98-438ddcd89e8f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8921fabf-1d56-4c41-be0b-c0e7033c31d7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "65896f02-07d4-415d-9ad1-d0d379d4e44a", null, "User", "USER" },
                    { "72621007-cc68-49f8-87c7-528be7df7ca8", null, "Admin", "ADMIN" },
                    { "c10d70f1-7c72-4f36-bb0d-411fbe5e8006", null, "Host", "HOST" }
                });
        }
    }
}
