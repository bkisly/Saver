using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderIdToExternalAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                schema: "finance",
                table: "bankAccounts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderId",
                schema: "finance",
                table: "bankAccounts");
        }
    }
}
