using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Change_Create_Entities : Migration
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
                    thumbnail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingSkill", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    thumbnail = table.Column<string>(type: "text", nullable: false),
                    duration = table.Column<double>(type: "double precision", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RoadType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false),
                    thumbnail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    document_blob = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    license_tier = table.Column<int>(type: "integer", nullable: false),
                    car_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fuel = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    seat = table.Column<int>(type: "integer", nullable: false),
                    insurance_end_time = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.id);
                    table.ForeignKey(
                        name: "FK_Car_Manufacturer_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "Manufacturer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrivingSkillPackage",
                columns: table => new
                {
                    DrivingSkillsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackagesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingSkillPackage", x => new { x.DrivingSkillsId, x.PackagesId });
                    table.ForeignKey(
                        name: "FK_DrivingSkillPackage_DrivingSkill_DrivingSkillsId",
                        column: x => x.DrivingSkillsId,
                        principalTable: "DrivingSkill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrivingSkillPackage_Package_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageRoadType",
                columns: table => new
                {
                    PackagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoadTypesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageRoadType", x => new { x.PackagesId, x.RoadTypesId });
                    table.ForeignKey(
                        name: "FK_PackageRoadType_Package_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageRoadType_RoadType_RoadTypesId",
                        column: x => x.RoadTypesId,
                        principalTable: "RoadType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    duration = table.Column<double>(type: "double precision", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    car_id = table.Column<Guid>(type: "uuid", nullable: true),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.id);
                    table.ForeignKey(
                        name: "FK_Booking_Car_car_id",
                        column: x => x.car_id,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Package_package_id",
                        column: x => x.package_id,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    car_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImage", x => x.id);
                    table.ForeignKey(
                        name: "FK_CarImage_Car_car_id",
                        column: x => x.car_id,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarPackage",
                columns: table => new
                {
                    CarsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackagesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPackage", x => new { x.CarsId, x.PackagesId });
                    table.ForeignKey(
                        name: "FK_CarPackage_Car_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarPackage_Package_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrivingSession",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp", nullable: false),
                    actual_start_time = table.Column<DateTime>(type: "timestamp", nullable: false),
                    actual_end_time = table.Column<DateTime>(type: "timestamp", nullable: false),
                    distance = table.Column<decimal>(type: "numeric", nullable: false),
                    speed = table.Column<decimal>(type: "numeric", nullable: false),
                    starting_lat = table.Column<decimal>(type: "numeric", nullable: false),
                    starting_long = table.Column<decimal>(type: "numeric", nullable: false),
                    end_lat = table.Column<decimal>(type: "numeric", nullable: false),
                    end_long = table.Column<decimal>(type: "numeric", nullable: false),
                    novice_driver_note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    instructor_note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
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
                    rating_instructor = table.Column<int>(type: "integer", nullable: false),
                    description_instructor = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    rating_car = table.Column<int>(type: "integer", nullable: false),
                    description_car = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    car_id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    novice_driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
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
                    table.ForeignKey(
                        name: "FK_Feedback_Car_car_id",
                        column: x => x.car_id,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RescheduleRequest",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    request_side = table.Column<int>(type: "integer", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescheduleRequest", x => x.id);
                    table.ForeignKey(
                        name: "FK_RescheduleRequest_DrivingSession_SessionId",
                        column: x => x.SessionId,
                        principalTable: "DrivingSession",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteLog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    street_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    longtitude = table.Column<decimal>(type: "numeric", nullable: false),
                    heading = table.Column<string>(type: "text", nullable: false),
                    speed = table.Column<decimal>(type: "numeric", maxLength: 500, nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
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
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
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
                name: "IX_Booking_car_id",
                table: "Booking",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_package_id",
                table: "Booking",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_Car_manufacturer_id",
                table: "Car",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_CarImage_car_id",
                table: "CarImage",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_CarPackage_PackagesId",
                table: "CarPackage",
                column: "PackagesId");

            migrationBuilder.CreateIndex(
                name: "IX_DrivingSession_booking_id",
                table: "DrivingSession",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_DrivingSkillPackage_PackagesId",
                table: "DrivingSkillPackage",
                column: "PackagesId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_booking_id",
                table: "Feedback",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_car_id",
                table: "Feedback",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageRoadType_RoadTypesId",
                table: "PackageRoadType",
                column: "RoadTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_RescheduleRequest_SessionId",
                table: "RescheduleRequest",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLog_session_id",
                table: "RouteLog",
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
                name: "CarImage");

            migrationBuilder.DropTable(
                name: "CarPackage");

            migrationBuilder.DropTable(
                name: "DrivingSkillPackage");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "PackageRoadType");

            migrationBuilder.DropTable(
                name: "RescheduleRequest");

            migrationBuilder.DropTable(
                name: "RouteLog");

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
                name: "Car");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Manufacturer");
        }
    }
}
