using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Account_Id",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_EstateCategories_EstateCategoryDetail_CategoryId",
                table: "EstateCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstateCategoryDetail",
                table: "EstateCategoryDetail");

            migrationBuilder.RenameTable(
                name: "EstateCategoryDetail",
                newName: "EstateCategoryDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstateCategoryDetails",
                table: "EstateCategoryDetails",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Account_Id",
                table: "Company",
                column: "Id",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EstateCategories_EstateCategoryDetails_CategoryId",
                table: "EstateCategories",
                column: "CategoryId",
                principalTable: "EstateCategoryDetails",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Account_Id",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_EstateCategories_EstateCategoryDetails_CategoryId",
                table: "EstateCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstateCategoryDetails",
                table: "EstateCategoryDetails");

            migrationBuilder.RenameTable(
                name: "EstateCategoryDetails",
                newName: "EstateCategoryDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstateCategoryDetail",
                table: "EstateCategoryDetail",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Account_Id",
                table: "Company",
                column: "Id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstateCategories_EstateCategoryDetail_CategoryId",
                table: "EstateCategories",
                column: "CategoryId",
                principalTable: "EstateCategoryDetail",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
