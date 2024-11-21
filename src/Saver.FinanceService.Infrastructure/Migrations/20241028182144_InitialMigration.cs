using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.CreateTable(
                name: "IntegrationEventLog",
                schema: "finance",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventTypeName = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    TimesSent = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLog", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "accountHolders",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultAccountId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountHolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bankAccounts",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AccountHolderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bankAccounts_accountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalSchema: "finance",
                        principalTable: "accountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AccountHolderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_categories_accountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalSchema: "finance",
                        principalTable: "accountHolders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "recurringTransactionDefinitions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cron = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TransactionData_Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TransactionData_Value = table.Column<decimal>(type: "numeric(2)", precision: 2, nullable: false),
                    TransactionData_CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManualBankAccountId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurringTransactionDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recurringTransactionDefinitions_bankAccounts_ManualBankAcco~",
                        column: x => x.ManualBankAccountId,
                        principalSchema: "finance",
                        principalTable: "bankAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_recurringTransactionDefinitions_categories_TransactionData_~",
                        column: x => x.TransactionData_CategoryId,
                        principalSchema: "finance",
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionData_Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TransactionData_Value = table.Column<decimal>(type: "numeric(2)", precision: 2, nullable: false),
                    TransactionData_CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_bankAccounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "finance",
                        principalTable: "bankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_categories_TransactionData_CategoryId",
                        column: x => x.TransactionData_CategoryId,
                        principalSchema: "finance",
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accountHolders_DefaultAccountId",
                schema: "finance",
                table: "accountHolders",
                column: "DefaultAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bankAccounts_AccountHolderId",
                schema: "finance",
                table: "bankAccounts",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_bankAccounts_Name",
                schema: "finance",
                table: "bankAccounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_AccountHolderId",
                schema: "finance",
                table: "categories",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                schema: "finance",
                table: "categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recurringTransactionDefinitions_ManualBankAccountId",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                column: "ManualBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_recurringTransactionDefinitions_TransactionData_CategoryId",
                schema: "finance",
                table: "recurringTransactionDefinitions",
                column: "TransactionData_CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_AccountId",
                schema: "finance",
                table: "transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_TransactionData_CategoryId",
                schema: "finance",
                table: "transactions",
                column: "TransactionData_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_accountHolders_bankAccounts_DefaultAccountId",
                schema: "finance",
                table: "accountHolders",
                column: "DefaultAccountId",
                principalSchema: "finance",
                principalTable: "bankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accountHolders_bankAccounts_DefaultAccountId",
                schema: "finance",
                table: "accountHolders");

            migrationBuilder.DropTable(
                name: "IntegrationEventLog",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "recurringTransactionDefinitions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "bankAccounts",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "accountHolders",
                schema: "finance");
        }
    }
}
