using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbOperationsWithEFCore.Migrations
{
    /// <inheritdoc />
    public partial class addednewTables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrice_Currencies_CurrenciesId",
                table: "BookPrice");

            migrationBuilder.DropIndex(
                name: "IX_BookPrice_CurrenciesId",
                table: "BookPrice");

            migrationBuilder.DropColumn(
                name: "CurrenciesId",
                table: "BookPrice");

            migrationBuilder.CreateIndex(
                name: "IX_BookPrice_CurrencyId",
                table: "BookPrice",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrice_Currencies_CurrencyId",
                table: "BookPrice",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrice_Currencies_CurrencyId",
                table: "BookPrice");

            migrationBuilder.DropIndex(
                name: "IX_BookPrice_CurrencyId",
                table: "BookPrice");

            migrationBuilder.AddColumn<int>(
                name: "CurrenciesId",
                table: "BookPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookPrice_CurrenciesId",
                table: "BookPrice",
                column: "CurrenciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrice_Currencies_CurrenciesId",
                table: "BookPrice",
                column: "CurrenciesId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
