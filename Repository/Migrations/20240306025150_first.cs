using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Account");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 6, 9, 51, 50, 610, DateTimeKind.Local).AddTicks(5455),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 3, 17, 10, 43, 524, DateTimeKind.Local).AddTicks(5897));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 3, 17, 10, 43, 524, DateTimeKind.Local).AddTicks(5897),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 6, 9, 51, 50, 610, DateTimeKind.Local).AddTicks(5455));

            migrationBuilder.AddColumn<byte>(
                name: "IsVerified",
                table: "Account",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
