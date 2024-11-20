using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDbConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "421f2a99-2885-4fac-9917-1493cfaaf2a4", null, "Admin", "ADMIN" },
                    { "c3b44c35-12cd-46c0-a8ff-e37a2dfc2869", null, "Host", "HOST" },
                    { "ddf22c91-f47a-43a2-b3e4-d1b9553c844f", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "421f2a99-2885-4fac-9917-1493cfaaf2a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3b44c35-12cd-46c0-a8ff-e37a2dfc2869");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddf22c91-f47a-43a2-b3e4-d1b9553c844f");

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
    }
}
