using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_DefaultConnection : Migration
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
                    { "06fc8852-cddc-479c-a38a-2350b4754e26", null, "Admin", "ADMIN" },
                    { "8c269b62-d430-41a5-a2a9-6a1fda74cf89", null, "Host", "HOST" },
                    { "d4fd6687-d2c2-441f-8f45-4ed3a987fa39", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06fc8852-cddc-479c-a38a-2350b4754e26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c269b62-d430-41a5-a2a9-6a1fda74cf89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4fd6687-d2c2-441f-8f45-4ed3a987fa39");

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
