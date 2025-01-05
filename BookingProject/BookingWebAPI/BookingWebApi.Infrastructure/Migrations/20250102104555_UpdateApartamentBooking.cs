using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApartamentBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fba5673-0e21-43f5-9a29-40cd4d28bbff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da8f5457-83c0-49ff-85f6-fe77c7bfa293");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec72228e-fe09-4602-b998-322ec7935cf3");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "Bookings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "Apartaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerDay",
                table: "Apartaments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1af888ac-d159-4bb9-8b36-52aeee4241e9", null, "Admin", "ADMIN" },
                    { "8b818d8b-3df4-4a4a-95f2-2a0dd94a6366", null, "User", "USER" },
                    { "cd17a05e-c75c-4d55-b267-eb30f888d360", null, "Host", "HOST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1af888ac-d159-4bb9-8b36-52aeee4241e9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b818d8b-3df4-4a4a-95f2-2a0dd94a6366");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd17a05e-c75c-4d55-b267-eb30f888d360");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "Apartaments");

            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "Apartaments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9fba5673-0e21-43f5-9a29-40cd4d28bbff", null, "Admin", "ADMIN" },
                    { "da8f5457-83c0-49ff-85f6-fe77c7bfa293", null, "User", "USER" },
                    { "ec72228e-fe09-4602-b998-322ec7935cf3", null, "Host", "HOST" }
                });
        }
    }
}
