using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToAccountHolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "finance",
                table: "accountHolders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_accountHolders_UserId",
                schema: "finance",
                table: "accountHolders",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_accountHolders_UserId",
                schema: "finance",
                table: "accountHolders");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "finance",
                table: "accountHolders");

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
        }
    }
}
