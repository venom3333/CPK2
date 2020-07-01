using Microsoft.EntityFrameworkCore.Migrations;

namespace CPK.Api.Migrations
{
    public partial class LinesRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketLineDto_Baskets_BasketId",
                table: "BasketLineDto");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketLineDto_Products_ProductId",
                table: "BasketLineDto");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineDto_Orders_OrderId",
                table: "OrderLineDto");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineDto_Products_ProductId",
                table: "OrderLineDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderLineDto",
                table: "OrderLineDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketLineDto",
                table: "BasketLineDto");

            migrationBuilder.RenameTable(
                name: "OrderLineDto",
                newName: "OrderLines");

            migrationBuilder.RenameTable(
                name: "BasketLineDto",
                newName: "BasketLines");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLineDto_OrderId",
                table: "OrderLines",
                newName: "IX_OrderLines_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketLineDto_BasketId",
                table: "BasketLines",
                newName: "IX_BasketLines_BasketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderLines",
                table: "OrderLines",
                columns: new[] { "ProductId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketLines",
                table: "BasketLines",
                columns: new[] { "ProductId", "BasketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BasketLines_Baskets_BasketId",
                table: "BasketLines",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketLines_Products_ProductId",
                table: "BasketLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Orders_OrderId",
                table: "OrderLines",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Products_ProductId",
                table: "OrderLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketLines_Baskets_BasketId",
                table: "BasketLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketLines_Products_ProductId",
                table: "BasketLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Orders_OrderId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Products_ProductId",
                table: "OrderLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderLines",
                table: "OrderLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketLines",
                table: "BasketLines");

            migrationBuilder.RenameTable(
                name: "OrderLines",
                newName: "OrderLineDto");

            migrationBuilder.RenameTable(
                name: "BasketLines",
                newName: "BasketLineDto");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLineDto",
                newName: "IX_OrderLineDto_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketLines_BasketId",
                table: "BasketLineDto",
                newName: "IX_BasketLineDto_BasketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderLineDto",
                table: "OrderLineDto",
                columns: new[] { "ProductId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketLineDto",
                table: "BasketLineDto",
                columns: new[] { "ProductId", "BasketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BasketLineDto_Baskets_BasketId",
                table: "BasketLineDto",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketLineDto_Products_ProductId",
                table: "BasketLineDto",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineDto_Orders_OrderId",
                table: "OrderLineDto",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineDto_Products_ProductId",
                table: "OrderLineDto",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
