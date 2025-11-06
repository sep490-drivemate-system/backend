using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "instructor_note",
                table: "DrivingSession",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "novice_driver_note",
                table: "DrivingSession",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instructor_note",
                table: "DrivingSession");

            migrationBuilder.DropColumn(
                name: "novice_driver_note",
                table: "DrivingSession");
        }
    }
}
