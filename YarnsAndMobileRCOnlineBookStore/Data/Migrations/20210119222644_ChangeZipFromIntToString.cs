using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class ChangeZipFromIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10243169-ef31-418e-9cbe-89c6f37bc5d5");

            migrationBuilder.AddColumn<string>(
                name: "Phone4",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Zip",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b266c332-9f69-433a-a233-b05c6daa1870", "7d03451a-dc36-4bf1-bf6a-b8b06784b932", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b266c332-9f69-433a-a233-b05c6daa1870");

            migrationBuilder.DropColumn(
                name: "Phone4",
                table: "Phones");

            migrationBuilder.AlterColumn<int>(
                name: "Zip",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "10243169-ef31-418e-9cbe-89c6f37bc5d5", "c085e458-1950-446e-8efe-97fbc470e2d8", "Admin", "ADMIN" });
        }
    }
}
