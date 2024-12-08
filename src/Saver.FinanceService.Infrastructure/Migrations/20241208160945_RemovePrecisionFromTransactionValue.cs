using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePrecisionFromTransactionValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "transactions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldPrecision: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "transactions",
                type: "numeric(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionData_Value",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                type: "numeric(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
