using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceService.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Init_Case : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.id);
                    table.ForeignKey(
                        name: "FK_Blog_Category_category_id",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogContent",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    blog_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    no = table.Column<int>(type: "integer", maxLength: 500, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogContent", x => x.id);
                    table.ForeignKey(
                        name: "FK_BlogContent_Blog_blog_id",
                        column: x => x.blog_id,
                        principalTable: "Blog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    no = table.Column<int>(type: "integer", maxLength: 500, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
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
                name: "IX_Blog_category_id",
                table: "Blog",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogContent_blog_id",
                table: "BlogContent",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogImage_content_id",
                table: "BlogImage",
                column: "content_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogImage");

            migrationBuilder.DropTable(
                name: "BlogContent");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
