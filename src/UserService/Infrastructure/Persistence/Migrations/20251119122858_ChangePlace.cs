using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Users_user_id",
                table: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "EmergencyContact");

            migrationBuilder.RenameIndex(
                name: "IX_Address_user_id",
                table: "EmergencyContact",
                newName: "IX_EmergencyContact_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmergencyContact",
                table: "EmergencyContact",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyContact_Users_user_id",
                table: "EmergencyContact",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmergencyContact_Users_user_id",
                table: "EmergencyContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmergencyContact",
                table: "EmergencyContact");

            migrationBuilder.RenameTable(
                name: "EmergencyContact",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_EmergencyContact_user_id",
                table: "Address",
                newName: "IX_Address_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Users_user_id",
                table: "Address",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
