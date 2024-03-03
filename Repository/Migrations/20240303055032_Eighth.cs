using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Eighth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Area",
                table: "Estates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Coordinate",
                table: "Estates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Estates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Estates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndPayDate",
                table: "Auctions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 3, 12, 50, 32, 824, DateTimeKind.Local).AddTicks(1953),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 1, 16, 37, 33, 121, DateTimeKind.Local).AddTicks(2295));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Estates");

            migrationBuilder.DropColumn(
                name: "Coordinate",
                table: "Estates");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Estates");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Estates");

            migrationBuilder.DropColumn(
                name: "EndPayDate",
                table: "Auctions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 1, 16, 37, 33, 121, DateTimeKind.Local).AddTicks(2295),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 3, 12, 50, 32, 824, DateTimeKind.Local).AddTicks(1953));
        }
    }
}
