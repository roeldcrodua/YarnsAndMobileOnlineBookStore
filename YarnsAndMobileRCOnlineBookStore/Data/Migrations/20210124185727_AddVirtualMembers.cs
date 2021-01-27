using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class AddVirtualMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_MemberId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_AspNetUsers_MemberId",
                table: "Phones");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffdca76f-2770-4b46-a338-81f67ab417ab");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Phones",
                newName: "MembersId");

            migrationBuilder.RenameIndex(
                name: "IX_Phones_MemberId",
                table: "Phones",
                newName: "IX_Phones_MembersId");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Addresses",
                newName: "MembersId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_MemberId",
                table: "Addresses",
                newName: "IX_Addresses_MembersId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "91c5c09a-7d15-43df-a6f4-e0ac53941bc8", "bd913e36-7681-41ce-a3b1-fcf3f32159b8", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_MembersId",
                table: "Addresses",
                column: "MembersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_AspNetUsers_MembersId",
                table: "Phones",
                column: "MembersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_MembersId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_AspNetUsers_MembersId",
                table: "Phones");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91c5c09a-7d15-43df-a6f4-e0ac53941bc8");

            migrationBuilder.RenameColumn(
                name: "MembersId",
                table: "Phones",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Phones_MembersId",
                table: "Phones",
                newName: "IX_Phones_MemberId");

            migrationBuilder.RenameColumn(
                name: "MembersId",
                table: "Addresses",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_MembersId",
                table: "Addresses",
                newName: "IX_Addresses_MemberId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ffdca76f-2770-4b46-a338-81f67ab417ab", "7e2e1a2c-1390-45d6-973a-e05e65942104", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_MemberId",
                table: "Addresses",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_AspNetUsers_MemberId",
                table: "Phones",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
