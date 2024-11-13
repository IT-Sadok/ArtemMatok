using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Appuser_Apartament_AddNewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceCompanyId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Apartaments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceCompanyId",
                table: "Apartaments",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6431b27d-6c73-466e-9e6b-f6981f9e2b0c", null, "Host", "HOST" },
                    { "b3f120f7-6200-4cb0-84dc-d286259f7ea7", null, "Admin", "ADMIN" },
                    { "e65c14d9-2629-44ab-85b3-4c94e5f71c5d", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ExternalId_SourceCompanyId",
                table: "AspNetUsers",
                columns: new[] { "ExternalId", "SourceCompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Apartaments_ExternalId_SourceCompanyId",
                table: "Apartaments",
                columns: new[] { "ExternalId", "SourceCompanyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ExternalId_SourceCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Apartaments_ExternalId_SourceCompanyId",
                table: "Apartaments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6431b27d-6c73-466e-9e6b-f6981f9e2b0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3f120f7-6200-4cb0-84dc-d286259f7ea7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e65c14d9-2629-44ab-85b3-4c94e5f71c5d");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SourceCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Apartaments");

            migrationBuilder.DropColumn(
                name: "SourceCompanyId",
                table: "Apartaments");

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
    }
}
