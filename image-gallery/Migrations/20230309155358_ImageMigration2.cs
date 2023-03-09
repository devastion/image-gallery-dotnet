using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace imagegallery.Migrations
{
    /// <inheritdoc />
    public partial class ImageMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_User_IdId",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "User_IdId",
                table: "Images",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_User_IdId",
                table: "Images",
                newName: "IX_Images_UserId");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Images",
                newName: "User_IdId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_UserId",
                table: "Images",
                newName: "IX_Images_User_IdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_User_IdId",
                table: "Images",
                column: "User_IdId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
