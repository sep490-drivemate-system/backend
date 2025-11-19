using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmergencyContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_NoviceDriver_novice_driver_id",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "longtitude",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "novice_driver_id",
                table: "Address",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "location_string",
                table: "Address",
                newName: "contact_name");

            migrationBuilder.RenameIndex(
                name: "IX_Address_novice_driver_id",
                table: "Address",
                newName: "IX_Address_user_id");

            migrationBuilder.AddColumn<string>(
                name: "contact_number",
                table: "Address",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SavedLocation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    location_string = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    latitude = table.Column<float>(type: "real", nullable: false),
                    longtitude = table.Column<float>(type: "real", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    NoviceDriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedLocation", x => x.id);
                    table.ForeignKey(
                        name: "FK_SavedLocation_NoviceDriver_NoviceDriverId",
                        column: x => x.NoviceDriverId,
                        principalTable: "NoviceDriver",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_SavedLocation_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedLocation_NoviceDriverId",
                table: "SavedLocation",
                column: "NoviceDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedLocation_user_id",
                table: "SavedLocation",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Users_user_id",
                table: "Address",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Users_user_id",
                table: "Address");

            migrationBuilder.DropTable(
                name: "SavedLocation");

            migrationBuilder.DropColumn(
                name: "contact_number",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Address",
                newName: "novice_driver_id");

            migrationBuilder.RenameColumn(
                name: "contact_name",
                table: "Address",
                newName: "location_string");

            migrationBuilder.RenameIndex(
                name: "IX_Address_user_id",
                table: "Address",
                newName: "IX_Address_novice_driver_id");

            migrationBuilder.AddColumn<float>(
                name: "latitude",
                table: "Address",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "longtitude",
                table: "Address",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_NoviceDriver_novice_driver_id",
                table: "Address",
                column: "novice_driver_id",
                principalTable: "NoviceDriver",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
