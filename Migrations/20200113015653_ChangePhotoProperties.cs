using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAlbum.Migrations
{
    public partial class ChangePhotoProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_UserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_AuthorId",
                table: "Photos",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_AuthorId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Photos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
