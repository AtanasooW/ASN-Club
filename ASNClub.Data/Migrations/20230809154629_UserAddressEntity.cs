using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASNClub.Data.Migrations
{
    public partial class UserAddressEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_Addresses_AddressId",
                table: "UserAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_AspNetUsers_UserId",
                table: "UserAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress");

            migrationBuilder.RenameTable(
                name: "UserAddress",
                newName: "UsersAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddress_UserId",
                table: "UsersAddresses",
                newName: "IX_UsersAddresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAddresses",
                table: "UsersAddresses",
                columns: new[] { "AddressId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAddresses_Addresses_AddressId",
                table: "UsersAddresses",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAddresses_AspNetUsers_UserId",
                table: "UsersAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAddresses_Addresses_AddressId",
                table: "UsersAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAddresses_AspNetUsers_UserId",
                table: "UsersAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAddresses",
                table: "UsersAddresses");

            migrationBuilder.RenameTable(
                name: "UsersAddresses",
                newName: "UserAddress");

            migrationBuilder.RenameIndex(
                name: "IX_UsersAddresses_UserId",
                table: "UserAddress",
                newName: "IX_UserAddress_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress",
                columns: new[] { "AddressId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_Addresses_AddressId",
                table: "UserAddress",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_AspNetUsers_UserId",
                table: "UserAddress",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
