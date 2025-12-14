using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
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
                    table.ForeignKey(
                        name: "FK_InstructorRoutes_Instructor_instructor_id",
                        column: x => x.instructor_id,
                        principalTable: "Instructor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorRoutes_instructor_id",
                table: "InstructorRoutes",
                column: "instructor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorRoutes");
        }
    }
}
