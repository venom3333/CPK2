using Microsoft.EntityFrameworkCore.Migrations;

namespace CPK.Api.Migrations
{
    public partial class CategoriesIdxInsteadAltKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProductCategories_Title",
                table: "ProductCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ProductCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Title",
                table: "ProductCategories",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_Title",
                table: "ProductCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ProductCategories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProductCategories_Title",
                table: "ProductCategories",
                column: "Title");
        }
    }
}
