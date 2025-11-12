using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceService.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogImage");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "BlogContent",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "BlogContent");

            migrationBuilder.CreateTable(
                name: "BlogImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    no = table.Column<int>(type: "integer", maxLength: 500, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImage", x => x.id);
                    table.ForeignKey(
                        name: "FK_BlogImage_BlogContent_content_id",
                        column: x => x.content_id,
                        principalTable: "BlogContent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogImage_content_id",
                table: "BlogImage",
                column: "content_id");
        }
    }
}
