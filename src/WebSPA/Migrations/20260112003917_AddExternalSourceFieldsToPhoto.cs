using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAlbum.Migrations
{
    public partial class AddExternalSourceFieldsToPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalHash",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Photos",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalProvider",
                table: "Photos",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ExternalProvider_ExternalId",
                table: "Photos",
                columns: new[] { "ExternalProvider", "ExternalId" },
                unique: true,
                filter: "[ExternalProvider] IS NOT NULL AND [ExternalId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photos_ExternalProvider_ExternalId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ExternalHash",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ExternalProvider",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Photos");
        }
    }
}
