using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceService.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Update_PostComment_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_comments_Posts_post_id",
                table: "post_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_post_comments_post_comments_parent_comment_id",
                table: "post_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_comments",
                table: "post_comments");

            migrationBuilder.RenameTable(
                name: "post_comments",
                newName: "PostComments");

            migrationBuilder.RenameIndex(
                name: "IX_post_comments_post_id",
                table: "PostComments",
                newName: "IX_PostComments_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_post_comments_parent_comment_id",
                table: "PostComments",
                newName: "IX_PostComments_parent_comment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostComments",
                table: "PostComments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_PostComments_parent_comment_id",
                table: "PostComments",
                column: "parent_comment_id",
                principalTable: "PostComments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_post_id",
                table: "PostComments",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_PostComments_parent_comment_id",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_post_id",
                table: "PostComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostComments",
                table: "PostComments");

            migrationBuilder.RenameTable(
                name: "PostComments",
                newName: "post_comments");

            migrationBuilder.RenameIndex(
                name: "IX_PostComments_post_id",
                table: "post_comments",
                newName: "IX_post_comments_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostComments_parent_comment_id",
                table: "post_comments",
                newName: "IX_post_comments_parent_comment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_comments",
                table: "post_comments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_comments_Posts_post_id",
                table: "post_comments",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_comments_post_comments_parent_comment_id",
                table: "post_comments",
                column: "parent_comment_id",
                principalTable: "post_comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
