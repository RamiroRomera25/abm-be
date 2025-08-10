using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace technical_tests_backend_ssr.Migrations
{
    public partial class Fix02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Auctions_AuctionId",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Purchases_PurchaseId",
                table: "Movements");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                table: "Movements",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuctionId",
                table: "Movements",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Auctions_AuctionId",
                table: "Movements",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Purchases_PurchaseId",
                table: "Movements",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Auctions_AuctionId",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Purchases_PurchaseId",
                table: "Movements");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                table: "Movements",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuctionId",
                table: "Movements",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Auctions_AuctionId",
                table: "Movements",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "AuctionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Purchases_PurchaseId",
                table: "Movements",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
