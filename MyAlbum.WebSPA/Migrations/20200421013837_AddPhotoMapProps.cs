using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAlbum.Migrations
{
    public partial class AddPhotoMapProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CenterLat",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CenterLng",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LocLat",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LocLng",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapFilePath",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapZoom",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenterLat",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "CenterLng",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "LocLat",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "LocLng",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "MapFilePath",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "MapZoom",
                table: "Photos");
        }
    }
}
