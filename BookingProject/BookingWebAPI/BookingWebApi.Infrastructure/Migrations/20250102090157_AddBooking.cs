using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApartamentId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Apartaments_ApartamentId",
                        column: x => x.ApartamentId,
                        principalTable: "Apartaments",
                        principalColumn: "ApartamentId");
                    table.ForeignKey(
                        name: "FK_Booking_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2596bdbf-78a1-4183-a620-923a07407f0b", null, "Host", "HOST" },
                    { "7bc27fdd-3d44-4a14-b218-587f723973c9", null, "Admin", "ADMIN" },
                    { "891451e9-5234-4dd9-9702-ccdde23535d4", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ApartamentId",
                table: "Booking",
                column: "ApartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2596bdbf-78a1-4183-a620-923a07407f0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bc27fdd-3d44-4a14-b218-587f723973c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "891451e9-5234-4dd9-9702-ccdde23535d4");

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
    }
}
