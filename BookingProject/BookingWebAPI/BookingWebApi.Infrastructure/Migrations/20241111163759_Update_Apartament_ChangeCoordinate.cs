using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Apartament_ChangeCoordinate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b805db9-3237-4c44-a140-56fd546eae31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "467b389d-0e2c-4d07-8ce9-680ac98a9464");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a8867fb-69ce-4b1f-9e19-4c5930173d53");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Apartaments",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Apartaments",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f91b028-3852-4193-9f26-509b8f80cd3b", null, "User", "USER" },
                    { "43cf71ad-6cd9-4ae9-9d92-fbeb1ddfc54f", null, "Admin", "ADMIN" },
                    { "e8ed8c18-a71e-44d1-ba5c-146e443aad2c", null, "Host", "HOST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f91b028-3852-4193-9f26-509b8f80cd3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43cf71ad-6cd9-4ae9-9d92-fbeb1ddfc54f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8ed8c18-a71e-44d1-ba5c-146e443aad2c");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Apartaments",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Apartaments",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b805db9-3237-4c44-a140-56fd546eae31", null, "Admin", "ADMIN" },
                    { "467b389d-0e2c-4d07-8ce9-680ac98a9464", null, "Host", "HOST" },
                    { "8a8867fb-69ce-4b1f-9e19-4c5930173d53", null, "User", "USER" }
                });
        }
    }
}
