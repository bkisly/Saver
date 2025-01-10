using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.AccountIntegrationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBankServiceTypeConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Provider",
                table: "AccountIntegrations",
                newName: "BankServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountIntegrations_AccountId",
                table: "AccountIntegrations",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountIntegrations_UserId",
                table: "AccountIntegrations",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountIntegrations_AccountId",
                table: "AccountIntegrations");

            migrationBuilder.DropIndex(
                name: "IX_AccountIntegrations_UserId",
                table: "AccountIntegrations");

            migrationBuilder.RenameColumn(
                name: "BankServiceTypeId",
                table: "AccountIntegrations",
                newName: "Provider");
        }
    }
}
