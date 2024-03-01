using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bids",
                table: "Bids");

            migrationBuilder.AddColumn<int>(
                name: "BidId",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 1, 16, 37, 33, 121, DateTimeKind.Local).AddTicks(2295),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 2, 29, 9, 8, 3, 868, DateTimeKind.Local).AddTicks(5244));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bids",
                table: "Bids",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BidderId",
                table: "Bids",
                column: "BidderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bids",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_BidderId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "BidId",
                table: "Bids");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 29, 9, 8, 3, 868, DateTimeKind.Local).AddTicks(5244),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 1, 16, 37, 33, 121, DateTimeKind.Local).AddTicks(2295));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bids",
                table: "Bids",
                columns: new[] { "BidderId", "AuctionId" });
        }
    }
}
