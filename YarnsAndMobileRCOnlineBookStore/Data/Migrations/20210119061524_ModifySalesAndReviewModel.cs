using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class ModifySalesAndReviewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "632f91f4-af7d-4eb3-adae-f374967f3740");

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleDate",
                table: "Reviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Reviews",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BooksBookId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3fc97849-c0ab-4ccb-ade9-3c6546c5d27f", "fbeb528a-bfc1-4810-a693-eca1dc0e0e25", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BooksBookId",
                table: "Books",
                column: "BooksBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_MemberId",
                table: "Books",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_MemberId",
                table: "Books",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_BooksBookId",
                table: "Books",
                column: "BooksBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_MemberId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_BooksBookId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BooksBookId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_MemberId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fc97849-c0ab-4ccb-ade9-3c6546c5d27f");

            migrationBuilder.DropColumn(
                name: "SaleDate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BooksBookId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Books");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "632f91f4-af7d-4eb3-adae-f374967f3740", "7869c1cf-7066-4fc6-90fb-ff800269c230", "Admin", "ADMIN" });
        }
    }
}
