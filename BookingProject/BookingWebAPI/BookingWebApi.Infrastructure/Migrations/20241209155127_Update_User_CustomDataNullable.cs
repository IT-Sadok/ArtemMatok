using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_User_CustomDataNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "CustomUserData",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "CustomUserData",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
