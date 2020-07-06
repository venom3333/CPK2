using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CPK.Api.Migrations
{
    public partial class News : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "ProductCategories",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConcurrencyToken = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    ImageId = table.Column<Guid>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    Updated = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Files_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_ImageId",
                table: "News",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_News_Title",
                table: "News",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "ProductCategories");
        }
    }
}
