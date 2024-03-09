using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 9, 11, 52, 34, 787, DateTimeKind.Local).AddTicks(2237),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 9, 11, 51, 15, 416, DateTimeKind.Local).AddTicks(3544));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 9, 11, 52, 34, 786, DateTimeKind.Local).AddTicks(187),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 9, 11, 51, 15, 415, DateTimeKind.Local).AddTicks(1391));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 9, 11, 51, 15, 416, DateTimeKind.Local).AddTicks(3544),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 9, 11, 52, 34, 787, DateTimeKind.Local).AddTicks(2237));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PayTime",
                table: "AuctionReceiptPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 9, 11, 51, 15, 415, DateTimeKind.Local).AddTicks(1391),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 9, 11, 52, 34, 786, DateTimeKind.Local).AddTicks(187));
        }
    }
}
