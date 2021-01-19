using Microsoft.EntityFrameworkCore.Migrations;

namespace YarnsAndMobileRCOnlineBookStore.Migrations
{
    public partial class ModifyAddressPhonesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaCode",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Phones");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Addresses",
                newName: "Line2");

            migrationBuilder.AddColumn<string>(
                name: "Phone1",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone2",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone3",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Line1",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "632f91f4-af7d-4eb3-adae-f374967f3740", "7869c1cf-7066-4fc6-90fb-ff800269c230", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "632f91f4-af7d-4eb3-adae-f374967f3740");

            migrationBuilder.DropColumn(
                name: "Phone1",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "Phone2",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "Phone3",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "Line1",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Line2",
                table: "Addresses",
                newName: "Country");

            migrationBuilder.AddColumn<int>(
                name: "AreaCode",
                table: "Phones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Extension",
                table: "Phones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "Phones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
