using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASNClub.Data.Migrations
{
    public partial class ChangeOrderStatusIdToIntFIX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrdersStatuses",
                columns: new[] { "Id", "Status" },
                values: new object[] { 3, "The order is packaged" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrdersStatuses",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
