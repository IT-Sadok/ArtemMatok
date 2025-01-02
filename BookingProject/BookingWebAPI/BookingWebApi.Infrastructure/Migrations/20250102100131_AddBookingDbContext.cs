using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Apartaments_ApartamentId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

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

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_UserId",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ApartamentId",
                table: "Bookings",
                newName: "IX_Bookings_ApartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9fba5673-0e21-43f5-9a29-40cd4d28bbff", null, "Admin", "ADMIN" },
                    { "da8f5457-83c0-49ff-85f6-fe77c7bfa293", null, "User", "USER" },
                    { "ec72228e-fe09-4602-b998-322ec7935cf3", null, "Host", "HOST" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Apartaments_ApartamentId",
                table: "Bookings",
                column: "ApartamentId",
                principalTable: "Apartaments",
                principalColumn: "ApartamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Apartaments_ApartamentId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

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

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "Booking",
                newName: "IX_Booking_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ApartamentId",
                table: "Booking",
                newName: "IX_Booking_ApartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2596bdbf-78a1-4183-a620-923a07407f0b", null, "Host", "HOST" },
                    { "7bc27fdd-3d44-4a14-b218-587f723973c9", null, "Admin", "ADMIN" },
                    { "891451e9-5234-4dd9-9702-ccdde23535d4", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Apartaments_ApartamentId",
                table: "Booking",
                column: "ApartamentId",
                principalTable: "Apartaments",
                principalColumn: "ApartamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
