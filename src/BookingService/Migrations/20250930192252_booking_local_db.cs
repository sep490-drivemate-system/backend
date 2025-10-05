using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class booking_local_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    upadated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RoadType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PackageType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    car_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    package_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "BookingTimeRange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    booking_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    booking_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    actual_start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actual_end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    speed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    starting_lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    starting_long = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    end_lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    end_long = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    booking_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating_instructor = table.Column<int>(type: "int", nullable: false),
                    description_instructor = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    rating_car = table.Column<int>(type: "int", nullable: false),
                    description_car = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "RouteLog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    street_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    longtitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    speed = table.Column<decimal>(type: "decimal(18,2)", maxLength: 500, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "SessionRoadType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    road_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "SessionRoute",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    text_instruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    street_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude_start = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    longtitude_start = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Booking_package_id",
                table: "Booking",
                column: "package_id");

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
