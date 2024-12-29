using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EUR",
                table: "UserBalances");

            migrationBuilder.DropColumn(
                name: "UAH",
                table: "UserBalances");

            migrationBuilder.DropColumn(
                name: "USD",
                table: "UserBalances");

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrencyName = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    UserBalanceId = table.Column<string>(type: "text", nullable: false),
                    UserBalanceId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                    table.ForeignKey(
                        name: "FK_Currency_UserBalances_UserBalanceId1",
                        column: x => x.UserBalanceId1,
                        principalTable: "UserBalances",
                        principalColumn: "UserBalanceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currency_UserBalanceId1",
                table: "Currency",
                column: "UserBalanceId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.AddColumn<decimal>(
                name: "EUR",
                table: "UserBalances",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UAH",
                table: "UserBalances",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "USD",
                table: "UserBalances",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
