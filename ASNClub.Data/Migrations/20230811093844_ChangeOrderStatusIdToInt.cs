using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASNClub.Data.Migrations
{
    public partial class ChangeOrderStatusIdToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderStatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrdersStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrdersStatuses",
                columns: new[] { "Id", "Status" },
                values: new object[] { 1, "Awaiting approval" });

            migrationBuilder.InsertData(
                table: "OrdersStatuses",
                columns: new[] { "Id", "Status" },
                values: new object[] { 2, "Confirmed" });

            migrationBuilder.InsertData(
                table: "OrdersStatuses",
                columns: new[] { "Id", "Status" },
                values: new object[] { 4, "The order has been delivered to a courier" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrdersStatuses_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId",
                principalTable: "OrdersStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrdersStatuses_OrderStatusId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrdersStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStatusId",
                table: "Orders");
        }
    }
}
