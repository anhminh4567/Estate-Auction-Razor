using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class nineth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReceiptPayment_Account_AccountId",
                table: "AuctionReceiptPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReceiptPayment_AuctionReceipts_ReceiptId",
                table: "AuctionReceiptPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionReceiptPayment",
                table: "AuctionReceiptPayment");

            migrationBuilder.RenameTable(
                name: "AuctionReceiptPayment",
                newName: "AuctionReceiptPayments");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionReceiptPayment_ReceiptId",
                table: "AuctionReceiptPayments",
                newName: "IX_AuctionReceiptPayments_ReceiptId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 3, 17, 10, 43, 524, DateTimeKind.Local).AddTicks(5897),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 3, 12, 50, 32, 824, DateTimeKind.Local).AddTicks(1953));

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "AuctionReceiptPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AuctionReceiptPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AuctionReceiptPayments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionReceiptPayments",
                table: "AuctionReceiptPayments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReceiptPayments_AccountId",
                table: "AuctionReceiptPayments",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReceiptPayments_Account_AccountId",
                table: "AuctionReceiptPayments",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReceiptPayments_AuctionReceipts_ReceiptId",
                table: "AuctionReceiptPayments",
                column: "ReceiptId",
                principalTable: "AuctionReceipts",
                principalColumn: "ReceiptId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReceiptPayments_Account_AccountId",
                table: "AuctionReceiptPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReceiptPayments_AuctionReceipts_ReceiptId",
                table: "AuctionReceiptPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionReceiptPayments",
                table: "AuctionReceiptPayments");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReceiptPayments_AccountId",
                table: "AuctionReceiptPayments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AuctionReceiptPayments");

            migrationBuilder.RenameTable(
                name: "AuctionReceiptPayments",
                newName: "AuctionReceiptPayment");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionReceiptPayments_ReceiptId",
                table: "AuctionReceiptPayment",
                newName: "IX_AuctionReceiptPayment_ReceiptId");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "AuctionReceiptPayment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 3, 12, 50, 32, 824, DateTimeKind.Local).AddTicks(1953),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 3, 17, 10, 43, 524, DateTimeKind.Local).AddTicks(5897));

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AuctionReceiptPayment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionReceiptPayment",
                table: "AuctionReceiptPayment",
                columns: new[] { "AccountId", "ReceiptId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReceiptPayment_Account_AccountId",
                table: "AuctionReceiptPayment",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReceiptPayment_AuctionReceipts_ReceiptId",
                table: "AuctionReceiptPayment",
                column: "ReceiptId",
                principalTable: "AuctionReceipts",
                principalColumn: "ReceiptId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
