using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingRemoveCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "CreatedAt",
                table: "Bookings");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
