using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConversionForCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accountHolders_bankAccounts_DefaultAccountId",
                schema: "finance",
                table: "accountHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_bankAccounts_currencies_CurrencyId",
                schema: "finance",
                table: "bankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_bankAccounts_CurrencyId",
                schema: "finance",
                table: "bankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_accountHolders_DefaultAccountId",
                schema: "finance",
                table: "accountHolders");

            migrationBuilder.DropColumn(
                name: "DefaultAccountId",
                schema: "finance",
                table: "accountHolders");

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

            migrationBuilder.CreateTable(
                name: "defaultBankAccounts",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountHolderId = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_defaultBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_defaultBankAccounts_accountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalSchema: "finance",
                        principalTable: "accountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_defaultBankAccounts_bankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "finance",
                        principalTable: "bankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_defaultBankAccounts_AccountHolderId",
                schema: "finance",
                table: "defaultBankAccounts",
                column: "AccountHolderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_defaultBankAccounts_BankAccountId",
                schema: "finance",
                table: "defaultBankAccounts",
                column: "BankAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "defaultBankAccounts",
                schema: "finance");

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

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultAccountId",
                schema: "finance",
                table: "accountHolders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bankAccounts_CurrencyId",
                schema: "finance",
                table: "bankAccounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_accountHolders_DefaultAccountId",
                schema: "finance",
                table: "accountHolders",
                column: "DefaultAccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_accountHolders_bankAccounts_DefaultAccountId",
                schema: "finance",
                table: "accountHolders",
                column: "DefaultAccountId",
                principalSchema: "finance",
                principalTable: "bankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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
    }
}
