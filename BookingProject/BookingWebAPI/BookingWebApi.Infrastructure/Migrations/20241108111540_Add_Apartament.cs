using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Apartament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f7edfa1-1117-412d-90e2-ab32ba8a1843");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1401d4e0-b488-4a28-b123-53d7bfe39895");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddb01eba-900b-40ff-b729-16477b58029e");

            migrationBuilder.CreateTable(
                name: "Apartaments",
                columns: table => new
                {
                    ApartamentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Area = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Bedrooms = table.Column<int>(type: "integer", nullable: false),
                    HostId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartaments", x => x.ApartamentId);
                    table.ForeignKey(
                        name: "FK_Apartaments_AspNetUsers_HostId",
                        column: x => x.HostId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b805db9-3237-4c44-a140-56fd546eae31", null, "Admin", "ADMIN" },
                    { "467b389d-0e2c-4d07-8ce9-680ac98a9464", null, "Host", "HOST" },
                    { "8a8867fb-69ce-4b1f-9e19-4c5930173d53", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartaments_HostId",
                table: "Apartaments",
                column: "HostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartaments");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f7edfa1-1117-412d-90e2-ab32ba8a1843", null, "User", "USER" },
                    { "1401d4e0-b488-4a28-b123-53d7bfe39895", null, "Admin", "ADMIN" },
                    { "ddb01eba-900b-40ff-b729-16477b58029e", null, "Host", "HOST" }
                });
        }
    }
}
