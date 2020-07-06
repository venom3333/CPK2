using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CPK.Api.Migrations
{
    public partial class ProductUnnecessaryFKRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductDtoId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProductDtoId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ProductDtoId",
                table: "ProductCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductDtoId",
                table: "ProductCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductDtoId",
                table: "ProductCategories",
                column: "ProductDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductDtoId",
                table: "ProductCategories",
                column: "ProductDtoId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
