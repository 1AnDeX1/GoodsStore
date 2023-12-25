using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsStore.Migrations
{
    /// <inheritdoc />
    public partial class DeliveryQueueEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "DeliveryQueues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryQueues_OrderID",
                table: "DeliveryQueues",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryQueues_Orders_OrderID",
                table: "DeliveryQueues",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryQueues_Orders_OrderID",
                table: "DeliveryQueues");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryQueues_OrderID",
                table: "DeliveryQueues");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "DeliveryQueues");
        }
    }
}
