using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class ChnagePhoneAddressTOICollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressesAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Phones_PhoneNumbersPhoneId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AddressesAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhoneNumbersPhoneId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37d86d92-c102-4e2e-a5f6-4c5fb8b0bbec");

            migrationBuilder.DropColumn(
                name: "AddressesAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumbersPhoneId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Phones",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberImport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Line2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneType1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneType2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneType3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneType4 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberImport", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ffdca76f-2770-4b46-a338-81f67ab417ab", "7e2e1a2c-1390-45d6-973a-e05e65942104", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_Phones_MemberId",
                table: "Phones",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_MemberId",
                table: "Addresses",
                column: "MemberId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_MemberId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_AspNetUsers_MemberId",
                table: "Phones");

            migrationBuilder.DropTable(
                name: "MemberImport");

            migrationBuilder.DropIndex(
                name: "IX_Phones_MemberId",
                table: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_MemberId",
                table: "Addresses");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffdca76f-2770-4b46-a338-81f67ab417ab");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "AddressesAddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumbersPhoneId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "37d86d92-c102-4e2e-a5f6-4c5fb8b0bbec", "142217b8-079f-454f-9e96-795a346e403b", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressesAddressId",
                table: "AspNetUsers",
                column: "AddressesAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumbersPhoneId",
                table: "AspNetUsers",
                column: "PhoneNumbersPhoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressesAddressId",
                table: "AspNetUsers",
                column: "AddressesAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Phones_PhoneNumbersPhoneId",
                table: "AspNetUsers",
                column: "PhoneNumbersPhoneId",
                principalTable: "Phones",
                principalColumn: "PhoneId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
