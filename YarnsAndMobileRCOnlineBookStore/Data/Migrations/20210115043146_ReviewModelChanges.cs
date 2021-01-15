using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class ReviewModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Reviews_ReviewId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ReviewId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "SaleDate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BooksBookId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BooksBookId",
                table: "Reviews",
                column: "BooksBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BooksBookId",
                table: "Reviews",
                column: "BooksBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BooksBookId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BooksBookId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BooksBookId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleDate",
                table: "Reviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Reviews",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReviewId",
                table: "Books",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Reviews_ReviewId",
                table: "Books",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
