using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class SaleModelChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Sales_SaleOrderId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_SaleOrderId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleOrderId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BooksBookId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BooksBookId",
                table: "Sales",
                column: "BooksBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Books_BooksBookId",
                table: "Sales",
                column: "BooksBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Books_BooksBookId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_BooksBookId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BooksBookId",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SaleOrderId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SaleOrderId",
                table: "Books",
                column: "SaleOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Sales_SaleOrderId",
                table: "Books",
                column: "SaleOrderId",
                principalTable: "Sales",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
