using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceService.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Huy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "BlogContent");

            migrationBuilder.DropColumn(
                name: "no",
                table: "BlogContent");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "BlogContent",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "image_list",
                table: "Blog",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_list",
                table: "Blog");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "BlogContent",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "BlogContent",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "no",
                table: "BlogContent",
                type: "integer",
                maxLength: 500,
                nullable: false,
                defaultValue: 0);
        }
    }
}
