using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6431b27d-6c73-466e-9e6b-f6981f9e2b0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3f120f7-6200-4cb0-84dc-d286259f7ea7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e65c14d9-2629-44ab-85b3-4c94e5f71c5d");

            migrationBuilder.AddColumn<string>(
                name: "CustomData",
                table: "Apartaments",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "066ce4db-3232-4e18-8551-f984c443c88f", null, "User", "USER" },
                    { "185fdac6-c2eb-49c6-907d-8dc1edbea92f", null, "Host", "HOST" },
                    { "9aebbcc7-3a58-447c-af55-b8494f0f5d30", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "066ce4db-3232-4e18-8551-f984c443c88f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "185fdac6-c2eb-49c6-907d-8dc1edbea92f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aebbcc7-3a58-447c-af55-b8494f0f5d30");

            migrationBuilder.DropColumn(
                name: "CustomData",
                table: "Apartaments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6431b27d-6c73-466e-9e6b-f6981f9e2b0c", null, "Host", "HOST" },
                    { "b3f120f7-6200-4cb0-84dc-d286259f7ea7", null, "Admin", "ADMIN" },
                    { "e65c14d9-2629-44ab-85b3-4c94e5f71c5d", null, "User", "USER" }
                });
        }
    }
}
