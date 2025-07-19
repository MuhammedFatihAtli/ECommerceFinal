using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    public partial class Guncel1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_QuestId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_QuestId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "CartItems",
                newName: "GuestId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_SessionId_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_GuestId_ProductId");

            migrationBuilder.RenameColumn(
                name: "QuestId",
                table: "AspNetUsers",
                newName: "GuestId");

            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GuestId",
                table: "Orders",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_GuestId",
                table: "Orders",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_GuestId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_GuestId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "CartItems",
                newName: "SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_GuestId_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_SessionId_ProductId");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "AspNetUsers",
                newName: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_QuestId",
                table: "Orders",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_QuestId",
                table: "Orders",
                column: "QuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
