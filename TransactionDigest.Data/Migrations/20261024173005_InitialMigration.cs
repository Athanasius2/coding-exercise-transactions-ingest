using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionDigest.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    SourceSystem = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardNumber = table.Column<string>(type: "TEXT", maxLength: 4, nullable: true),
                    LocationCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TransactionTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => new { x.SourceSystem, x.TransactionId });
                });

            migrationBuilder.CreateTable(
                name: "TransactionAuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceSystem = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CurrentValue = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    UpdatedValue = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionAuditLogs_Transactions_SourceSystem_TransactionId",
                        columns: x => new { x.SourceSystem, x.TransactionId },
                        principalTable: "Transactions",
                        principalColumns: new[] { "SourceSystem", "TransactionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAuditLogs_SourceSystem_TransactionId",
                table: "TransactionAuditLogs",
                columns: new[] { "SourceSystem", "TransactionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionAuditLogs");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
