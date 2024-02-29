using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinedAuctions_Transactions_TransactionId",
                table: "JoinedAuctions");

            migrationBuilder.DropIndex(
                name: "IX_JoinedAuctions_TransactionId",
                table: "JoinedAuctions");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReceipts_AuctionId",
                table: "AuctionReceipts");

            migrationBuilder.DropColumn(
                name: "RegiisterFee",
                table: "JoinedAuctions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "JoinedAuctions");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Estates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "Auctions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "AuctionReceipts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainAmount",
                table: "AuctionReceipts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AuctionReceiptPayment",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ReceiptId = table.Column<int>(type: "int", nullable: false),
                    PayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 2, 29, 9, 8, 3, 868, DateTimeKind.Local).AddTicks(5244))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionReceiptPayment", x => new { x.AccountId, x.ReceiptId });
                    table.ForeignKey(
                        name: "FK_AuctionReceiptPayment_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionReceiptPayment_AuctionReceipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "AuctionReceipts",
                        principalColumn: "ReceiptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReceipts_AuctionId",
                table: "AuctionReceipts",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReceiptPayment_ReceiptId",
                table: "AuctionReceiptPayment",
                column: "ReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionReceiptPayment");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReceipts_AuctionId",
                table: "AuctionReceipts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Estates");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "RemainAmount",
                table: "AuctionReceipts");

            migrationBuilder.AddColumn<decimal>(
                name: "RegiisterFee",
                table: "JoinedAuctions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "JoinedAuctions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "AuctionReceipts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedAuctions_TransactionId",
                table: "JoinedAuctions",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReceipts_AuctionId",
                table: "AuctionReceipts",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinedAuctions_Transactions_TransactionId",
                table: "JoinedAuctions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "TransactionId");
        }
    }
}
