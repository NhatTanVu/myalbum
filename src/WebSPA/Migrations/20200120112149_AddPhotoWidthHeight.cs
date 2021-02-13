using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAlbum.Migrations
{
    public partial class AddPhotoWidthHeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Photos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Photos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Photos");
        }
    }
}
