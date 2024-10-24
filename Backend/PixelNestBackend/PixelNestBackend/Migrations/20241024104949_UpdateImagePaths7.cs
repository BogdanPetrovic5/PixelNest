using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelNestBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagePaths7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoryID",
                table: "ImagePaths",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_ImagePaths_StoryID",
                table: "ImagePaths",
                column: "StoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagePaths_Stories_StoryID",
                table: "ImagePaths",
                column: "StoryID",
                principalTable: "Stories",
                principalColumn: "StoryID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagePaths_Stories_StoryID",
                table: "ImagePaths");

            migrationBuilder.DropIndex(
                name: "IX_ImagePaths_StoryID",
                table: "ImagePaths");

            migrationBuilder.DropColumn(
                name: "StoryID",
                table: "ImagePaths");
        }
    }
}
