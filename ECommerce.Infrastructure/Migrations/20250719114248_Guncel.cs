using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    public partial class Guncel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_QuestId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_QuestId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "AspNetUsers");
        }
    }
}
