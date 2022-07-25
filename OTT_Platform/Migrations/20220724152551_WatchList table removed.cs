using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTT_Platform.Migrations
{
    public partial class WatchListtableremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchList");

            migrationBuilder.AddColumn<string>(
                name: "WatchList",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WatchList",
                table: "Profiles");

            migrationBuilder.CreateTable(
                name: "WatchList",
                columns: table => new
                {
                    Wid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchList", x => x.Wid);
                });
        }
    }
}
