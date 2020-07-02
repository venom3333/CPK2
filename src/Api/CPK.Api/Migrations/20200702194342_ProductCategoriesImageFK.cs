using Microsoft.EntityFrameworkCore.Migrations;

namespace CPK.Api.Migrations
{
    public partial class ProductCategoriesImageFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ImageId",
                table: "ProductCategories",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Files_ImageId",
                table: "ProductCategories",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Files_ImageId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ImageId",
                table: "ProductCategories");
        }
    }
}
