using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Init_Case : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrivingSkill",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingSkill", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PackageType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    car_id = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageType", x => x.id);
                    table.ForeignKey(
                        name: "FK_PackageType_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.id);
                    table.ForeignKey(
                        name: "FK_Booking_PackageType_package_id",
                        column: x => x.package_id,
                        principalTable: "PackageType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingDrivingSkill",
                columns: table => new
                {
                    BookingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    DrivingSkillsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDrivingSkill", x => new { x.BookingsId, x.DrivingSkillsId });
                    table.ForeignKey(
                        name: "FK_BookingDrivingSkill_Booking_BookingsId",
                        column: x => x.BookingsId,
                        principalTable: "Booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDrivingSkill_DrivingSkill_DrivingSkillsId",
                        column: x => x.DrivingSkillsId,
                        principalTable: "DrivingSkill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingTimeRange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTimeRange", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookingTimeRange_Booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrivingSession",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    actual_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actual_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    distance = table.Column<decimal>(type: "numeric", nullable: false),
                    speed = table.Column<decimal>(type: "numeric", nullable: false),
                    starting_lat = table.Column<decimal>(type: "numeric", nullable: false),
                    starting_long = table.Column<decimal>(type: "numeric", nullable: false),
                    end_lat = table.Column<decimal>(type: "numeric", nullable: false),
                    end_long = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingSession", x => x.id);
                    table.ForeignKey(
                        name: "FK_DrivingSession_Booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating_instructor = table.Column<int>(type: "integer", nullable: false),
                    description_instructor = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    rating_car = table.Column<int>(type: "integer", nullable: false),
                    description_car = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.id);
                    table.ForeignKey(
                        name: "FK_Feedback_Booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoadType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadType", x => x.id);
                    table.ForeignKey(
                        name: "FK_RoadType_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RouteLog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    street_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    longtitude = table.Column<decimal>(type: "numeric", nullable: false),
                    heading = table.Column<string>(type: "text", nullable: false),
                    speed = table.Column<decimal>(type: "numeric", maxLength: 500, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteLog", x => x.id);
                    table.ForeignKey(
                        name: "FK_RouteLog_DrivingSession_session_id",
                        column: x => x.session_id,
                        principalTable: "DrivingSession",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionRoute",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text_instruction = table.Column<string>(type: "text", nullable: false),
                    street_name = table.Column<string>(type: "text", nullable: false),
                    latitude_start = table.Column<decimal>(type: "numeric", nullable: false),
                    longtitude_start = table.Column<decimal>(type: "numeric", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRoute", x => x.id);
                    table.ForeignKey(
                        name: "FK_SessionRoute_DrivingSession_session_id",
                        column: x => x.session_id,
                        principalTable: "DrivingSession",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionRoadType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    road_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRoadType", x => x.id);
                    table.ForeignKey(
                        name: "FK_SessionRoadType_DrivingSession_session_id",
                        column: x => x.session_id,
                        principalTable: "DrivingSession",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRoadType_RoadType_road_type_id",
                        column: x => x.road_type_id,
                        principalTable: "RoadType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_package_id",
                table: "Booking",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDrivingSkill_DrivingSkillsId",
                table: "BookingDrivingSkill",
                column: "DrivingSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTimeRange_booking_id",
                table: "BookingTimeRange",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_DrivingSession_booking_id",
                table: "DrivingSession",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_booking_id",
                table: "Feedback",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageType_PackageId",
                table: "PackageType",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadType_BookingId",
                table: "RoadType",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLog_session_id",
                table: "RouteLog",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRoadType_road_type_id",
                table: "SessionRoadType",
                column: "road_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRoadType_session_id",
                table: "SessionRoadType",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRoute_session_id",
                table: "SessionRoute",
                column: "session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingDrivingSkill");

            migrationBuilder.DropTable(
                name: "BookingTimeRange");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "RouteLog");

            migrationBuilder.DropTable(
                name: "SessionRoadType");

            migrationBuilder.DropTable(
                name: "SessionRoute");

            migrationBuilder.DropTable(
                name: "DrivingSkill");

            migrationBuilder.DropTable(
                name: "RoadType");

            migrationBuilder.DropTable(
                name: "DrivingSession");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "PackageType");

            migrationBuilder.DropTable(
                name: "Package");
        }
    }
}
