using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTT_Platform.Migrations
{
    public partial class Secondcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_WatchList_WatchListWid",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_WatchListWid",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WatchList");

            migrationBuilder.DropColumn(
                name: "WatchListWid",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WatchList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WatchListWid",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_WatchListWid",
                table: "Movies",
                column: "WatchListWid");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_WatchList_WatchListWid",
                table: "Movies",
                column: "WatchListWid",
                principalTable: "WatchList",
                principalColumn: "Wid");
        }
    }
}
