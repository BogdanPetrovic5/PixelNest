using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelNestBackend.Migrations
{
    /// <inheritdoc />
    public partial class FollowTabelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    UserFollowerID = table.Column<int>(type: "int", nullable: false),
                    UserFollowingID = table.Column<int>(type: "int", nullable: false),
                    FollowerUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FollowingUsername = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => new { x.UserFollowerID, x.UserFollowingID });
                    table.ForeignKey(
                        name: "FK_Follow_Users_UserFollowerID",
                        column: x => x.UserFollowerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follow_Users_UserFollowingID",
                        column: x => x.UserFollowingID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follow_UserFollowingID",
                table: "Follow",
                column: "UserFollowingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follow");
        }
    }
}
