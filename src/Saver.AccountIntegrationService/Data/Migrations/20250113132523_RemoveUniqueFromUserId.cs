using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.AccountIntegrationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueFromUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountIntegrations_UserId",
                table: "AccountIntegrations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountIntegrations_UserId",
                table: "AccountIntegrations",
                column: "UserId",
                unique: true);
        }
    }
}
