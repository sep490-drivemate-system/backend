using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Add_Poliline_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "polyline_sesion_log",
                table: "DrivingSession",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "polyline_sesion_route",
                table: "DrivingSession",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "polyline_sesion_log",
                table: "DrivingSession");

            migrationBuilder.DropColumn(
                name: "polyline_sesion_route",
                table: "DrivingSession");
        }
    }
}
