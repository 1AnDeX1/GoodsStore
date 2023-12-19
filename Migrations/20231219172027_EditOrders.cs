using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsStore.Migrations
{
    /// <inheritdoc />
    public partial class EditOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AppUsersId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AppUsersId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AppUsersId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AppUserID",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserID",
                table: "Orders",
                column: "AppUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AppUserID",
                table: "Orders",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AppUserID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AppUserID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AppUserID",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppUsersId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUsersId",
                table: "Orders",
                column: "AppUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AppUsersId",
                table: "Orders",
                column: "AppUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
