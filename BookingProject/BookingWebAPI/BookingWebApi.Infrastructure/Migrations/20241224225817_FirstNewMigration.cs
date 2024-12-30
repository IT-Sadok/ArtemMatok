using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstNewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14d44814-ec61-43b5-bc0c-e8577ecfbb8d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2ff719f-2a82-479a-af9f-d86ed77c9918");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2ef3649-2e56-4118-b4da-82a39bf09f1b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "890e2bc9-9060-47ae-9b48-251904185f85", null, "Host", "HOST" },
                    { "d56bedda-7cb4-4c32-9c31-9b30e4a12714", null, "Admin", "ADMIN" },
                    { "e5288cef-3998-4c5b-aef6-126151048715", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "890e2bc9-9060-47ae-9b48-251904185f85");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d56bedda-7cb4-4c32-9c31-9b30e4a12714");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5288cef-3998-4c5b-aef6-126151048715");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14d44814-ec61-43b5-bc0c-e8577ecfbb8d", null, "User", "USER" },
                    { "b2ff719f-2a82-479a-af9f-d86ed77c9918", null, "Host", "HOST" },
                    { "e2ef3649-2e56-4118-b4da-82a39bf09f1b", null, "Admin", "ADMIN" }
                });
        }
    }
}
