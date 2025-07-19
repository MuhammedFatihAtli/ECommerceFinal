using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    public partial class Guncel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_GuestId",
                table: "Products",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_GuestId",
                table: "Products",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_GuestId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GuestId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "Orders",
                type: "int",
                nullable: true);
        }
    }
}
