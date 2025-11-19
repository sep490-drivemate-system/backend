using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Update_Session_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "display_name",
                table: "DrivingSession",
                newName: "display_start_location_name");

            migrationBuilder.AddColumn<string>(
                name: "display_end_location_name",
                table: "DrivingSession",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "display_end_location_name",
                table: "DrivingSession");

            migrationBuilder.RenameColumn(
                name: "display_start_location_name",
                table: "DrivingSession",
                newName: "display_name");
        }
    }
}
