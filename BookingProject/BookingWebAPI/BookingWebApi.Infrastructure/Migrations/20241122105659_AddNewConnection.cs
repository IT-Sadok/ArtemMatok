using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "278fcabd-2ef5-4abb-a9e4-5fcdf7fb2165", null, "Admin", "ADMIN" },
                    { "44ac3b7a-1ff2-4203-bbf1-bbe35d7a023c", null, "Host", "HOST" },
                    { "abd16a70-2fce-4680-b0f7-cf02fd7c3666", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "278fcabd-2ef5-4abb-a9e4-5fcdf7fb2165");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44ac3b7a-1ff2-4203-bbf1-bbe35d7a023c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abd16a70-2fce-4680-b0f7-cf02fd7c3666");

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
    }
}
