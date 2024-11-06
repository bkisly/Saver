using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToRecurringTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recurringTransactionDefinitions_bankAccounts_ManualBankAcco~",
                schema: "finance",
                table: "recurringTransactionDefinitions");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "ManualBankAccountId",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_recurringTransactionDefinitions_bankAccounts_ManualBankAcco~",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                column: "ManualBankAccountId",
                principalSchema: "finance",
                principalTable: "bankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recurringTransactionDefinitions_bankAccounts_ManualBankAcco~",
                schema: "finance",
                table: "recurringTransactionDefinitions");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "ManualBankAccountId",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_recurringTransactionDefinitions_bankAccounts_ManualBankAcco~",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                column: "ManualBankAccountId",
                principalSchema: "finance",
                principalTable: "bankAccounts",
                principalColumn: "Id");
        }
    }
}
