using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class FixMembersModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91c5c09a-7d15-43df-a6f4-e0ac53941bc8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9f1d3b02-a121-4f56-aa0d-03902533846d", "d770413a-830c-4411-86d2-fb8c6f1ea839", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f1d3b02-a121-4f56-aa0d-03902533846d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "91c5c09a-7d15-43df-a6f4-e0ac53941bc8", "bd913e36-7681-41ce-a3b1-fcf3f32159b8", "Admin", "ADMIN" });
        }
    }
}
