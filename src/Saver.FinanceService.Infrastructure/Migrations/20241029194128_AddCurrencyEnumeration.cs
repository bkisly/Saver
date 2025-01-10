using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Saver.FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyEnumeration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "finance",
                table: "bankAccounts");

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "transactions",
                type: "numeric(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                type: "numeric(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldPrecision: 2);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                schema: "finance",
                table: "bankAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencies", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "finance",
                table: "currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "AED" },
                    { 2, "ARS" },
                    { 3, "AUD" },
                    { 4, "BGN" },
                    { 5, "BRL" },
                    { 6, "BSD" },
                    { 7, "CAD" },
                    { 8, "CHF" },
                    { 9, "CLP" },
                    { 10, "CNY" },
                    { 11, "COP" },
                    { 12, "CZK" },
                    { 13, "DKK" },
                    { 14, "DOP" },
                    { 15, "EGP" },
                    { 16, "EUR" },
                    { 17, "FJD" },
                    { 18, "GBP" },
                    { 19, "GTQ" },
                    { 20, "HKD" },
                    { 21, "HRK" },
                    { 22, "HUF" },
                    { 23, "IDR" },
                    { 24, "ILS" },
                    { 25, "INR" },
                    { 26, "ISK" },
                    { 27, "JPY" },
                    { 28, "KRW" },
                    { 29, "KZT" },
                    { 30, "MXN" },
                    { 31, "MYR" },
                    { 32, "NOK" },
                    { 33, "NZD" },
                    { 34, "PAB" },
                    { 35, "PEN" },
                    { 36, "PHP" },
                    { 37, "PKR" },
                    { 38, "PLN" },
                    { 39, "PYG" },
                    { 40, "RON" },
                    { 41, "RUB" },
                    { 42, "SAR" },
                    { 43, "SEK" },
                    { 44, "SGD" },
                    { 45, "THB" },
                    { 46, "TRY" },
                    { 47, "TWD" },
                    { 48, "UAH" },
                    { 49, "USD" },
                    { 50, "UYU" },
                    { 51, "ZAR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bankAccounts_CurrencyId",
                schema: "finance",
                table: "bankAccounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_currencies_Name",
                schema: "finance",
                table: "currencies",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bankAccounts_currencies_CurrencyId",
                schema: "finance",
                table: "bankAccounts",
                column: "CurrencyId",
                principalSchema: "finance",
                principalTable: "currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bankAccounts_currencies_CurrencyId",
                schema: "finance",
                table: "bankAccounts");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "IX_bankAccounts_CurrencyId",
                schema: "finance",
                table: "bankAccounts");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                schema: "finance",
                table: "bankAccounts");

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "transactions",
                type: "numeric(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                type: "numeric(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2)",
                oldPrecision: 2);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "finance",
                table: "bankAccounts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
