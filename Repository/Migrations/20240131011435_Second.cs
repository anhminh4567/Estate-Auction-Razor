using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BidLogs_Account_BidderId",
                table: "BidLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BidLogs_Auctions_AuctionId",
                table: "BidLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BidLogs",
                table: "BidLogs");

            migrationBuilder.RenameTable(
                name: "BidLogs",
                newName: "Bids");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Account",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_BidLogs_AuctionId",
                table: "Bids",
                newName: "IX_Bids_AuctionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bids",
                table: "Bids",
                columns: new[] { "BidderId", "AuctionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Account_BidderId",
                table: "Bids",
                column: "BidderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Account_BidderId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bids",
                table: "Bids");

            migrationBuilder.RenameTable(
                name: "Bids",
                newName: "BidLogs");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Account",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_AuctionId",
                table: "BidLogs",
                newName: "IX_BidLogs_AuctionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BidLogs",
                table: "BidLogs",
                columns: new[] { "BidderId", "AuctionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BidLogs_Account_BidderId",
                table: "BidLogs",
                column: "BidderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BidLogs_Auctions_AuctionId",
                table: "BidLogs",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
