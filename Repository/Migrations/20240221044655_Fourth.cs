using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Estates_AuctionId",
                table: "Auctions");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "Auctions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_EstateId",
                table: "Auctions",
                column: "EstateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Estates_EstateId",
                table: "Auctions",
                column: "EstateId",
                principalTable: "Estates",
                principalColumn: "EstateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Estates_EstateId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_EstateId",
                table: "Auctions");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "Auctions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Estates_AuctionId",
                table: "Auctions",
                column: "AuctionId",
                principalTable: "Estates",
                principalColumn: "EstateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
