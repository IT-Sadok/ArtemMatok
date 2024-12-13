using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_User_CustomData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CustomUserData",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "914ef8d4-bc37-46ee-b9fd-65b866926e3f", null, "Admin", "ADMIN" },
                    { "a911b162-45d8-4e2b-905e-28b2fec3d2fa", null, "Host", "HOST" },
                    { "d3d25781-7d99-4219-8eee-597f97cee26c", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "914ef8d4-bc37-46ee-b9fd-65b866926e3f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a911b162-45d8-4e2b-905e-28b2fec3d2fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3d25781-7d99-4219-8eee-597f97cee26c");

            migrationBuilder.DropColumn(
                name: "CustomUserData",
                table: "AspNetUsers");

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
    }
}
