using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class Add_SystemConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberDate",
                table: "SystemConfiguration",
                newName: "number_date");

            migrationBuilder.AlterColumn<int>(
                name: "number_date",
                table: "SystemConfiguration",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "number_date",
                table: "SystemConfiguration",
                newName: "NumberDate");

            migrationBuilder.AlterColumn<int>(
                name: "NumberDate",
                table: "SystemConfiguration",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
