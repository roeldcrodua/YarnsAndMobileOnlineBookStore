using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class ChangeAddressIntoNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fc97849-c0ab-4ccb-ade9-3c6546c5d27f");

            migrationBuilder.AlterColumn<int>(
                name: "Zip",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "10243169-ef31-418e-9cbe-89c6f37bc5d5", "c085e458-1950-446e-8efe-97fbc470e2d8", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10243169-ef31-418e-9cbe-89c6f37bc5d5");

            migrationBuilder.AlterColumn<int>(
                name: "Zip",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3fc97849-c0ab-4ccb-ade9-3c6546c5d27f", "fbeb528a-bfc1-4810-a693-eca1dc0e0e25", "Admin", "ADMIN" });
        }
    }
}
