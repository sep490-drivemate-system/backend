using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Add_InstructorRoutes_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstructorRoutes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    polyline = table.Column<string>(type: "text", nullable: false),
                    starting_latitude = table.Column<decimal>(type: "numeric(10,6)", nullable: false),
                    starting_longitude = table.Column<decimal>(type: "numeric(10,6)", nullable: false),
                    display_end_location_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    display_start_location_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ending_latitude = table.Column<decimal>(type: "numeric(10,6)", nullable: false),
                    ending_longitude = table.Column<decimal>(type: "numeric(10,6)", nullable: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorRoutes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InstructorRoutesPackage",
                columns: table => new
                {
                    InstructorRoutesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackagesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorRoutesPackage", x => new { x.InstructorRoutesId, x.PackagesId });
                    table.ForeignKey(
                        name: "FK_InstructorRoutesPackage_InstructorRoutes_InstructorRoutesId",
                        column: x => x.InstructorRoutesId,
                        principalTable: "InstructorRoutes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorRoutesPackage_Package_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorRoutesPackage_PackagesId",
                table: "InstructorRoutesPackage",
                column: "PackagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorRoutesPackage");

            migrationBuilder.DropTable(
                name: "InstructorRoutes");
        }
    }
}
