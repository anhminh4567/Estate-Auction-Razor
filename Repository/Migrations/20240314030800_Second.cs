using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Account_AccountId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AccountId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Notifications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 14, 10, 8, 0, 442, DateTimeKind.Local).AddTicks(9481),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 14, 10, 2, 58, 13, DateTimeKind.Local).AddTicks(3725));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 14, 10, 8, 0, 437, DateTimeKind.Local).AddTicks(935),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 14, 10, 2, 58, 5, DateTimeKind.Local).AddTicks(199));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 14, 10, 2, 58, 13, DateTimeKind.Local).AddTicks(3725),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 14, 10, 8, 0, 442, DateTimeKind.Local).AddTicks(9481));

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 14, 10, 2, 58, 5, DateTimeKind.Local).AddTicks(199),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 14, 10, 8, 0, 437, DateTimeKind.Local).AddTicks(935));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AccountId",
                table: "Notifications",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Account_AccountId",
                table: "Notifications",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
