using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelNestBackend.Migrations
{
    /// <inheritdoc />
    public partial class TotalReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalReplies",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalReplies",
                table: "Comments");
        }
    }
}
