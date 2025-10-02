using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Upadate_Car_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_DocumentType_type_id",
                table: "InstructorDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_InstructorApplication_application_id",
                table: "InstructorDocument");

            migrationBuilder.DropTable(
                name: "InstructorApplication");

            migrationBuilder.DropTable(
                name: "ScheduleAvailability");

            migrationBuilder.DropIndex(
                name: "IX_InstructorDocument_application_id",
                table: "InstructorDocument");

            migrationBuilder.DropIndex(
                name: "IX_InstructorDocument_type_id",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "application_id",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "InstructorDocument");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "InstructorDocument",
                newName: "permanent_address");

            migrationBuilder.AddColumn<DateOnly>(
                name: "date_of_birth",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "gender",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "background_profile_status",
                table: "InstructorDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "citizen_expiry_date",
                table: "InstructorDocument",
                type: "date",
                maxLength: 500,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "citizen_id_back",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "citizen_id_front",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "citizen_id_number",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "citizen_id_status",
                table: "InstructorDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "citizen_issue_date",
                table: "InstructorDocument",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "citizen_issue_place",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "driving_license_back",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "driving_license_expiry",
                table: "InstructorDocument",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "driving_license_front",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "driving_license_issue_date",
                table: "InstructorDocument",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "driving_license_number",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "driving_license_status",
                table: "InstructorDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "submit_at",
                table: "InstructorDocument",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "teaching_license_back",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "teaching_license_front",
                table: "InstructorDocument",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "teaching_status",
                table: "InstructorDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationTracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTracking", x => x.id);
                    table.ForeignKey(
                        name: "FK_ApplicationTracking_DocumentType_type_id",
                        column: x => x.type_id,
                        principalTable: "DocumentType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationTracking_InstructorDocument_application_id",
                        column: x => x.application_id,
                        principalTable: "InstructorDocument",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationTracking_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LicenseCategory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
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
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleUnavailability",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleUnavailability", x => x.id);
                    table.ForeignKey(
                        name: "FK_ScheduleUnavailability_Instructor_instructor_id",
                        column: x => x.instructor_id,
                        principalTable: "Instructor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    thumbnail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    insurance = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Fuel = table.Column<string>(type: "text", nullable: false),
                    insurance_end_time = table.Column<DateOnly>(type: "date", nullable: false),
                    vehicle_registration = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    seat = table.Column<int>(type: "integer", nullable: false),
                    car_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    license_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.id);
                    table.ForeignKey(
                        name: "FK_Car_Instructor_instructor_id",
                        column: x => x.instructor_id,
                        principalTable: "Instructor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Car_LicenseCategory_license_category_id",
                        column: x => x.license_category_id,
                        principalTable: "LicenseCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Car_Manufacturer_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "Manufacturer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    car_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
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
                name: "PackageCar",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    car_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageCar", x => x.id);
                    table.ForeignKey(
                        name: "FK_PackageCar_Car_car_id",
                        column: x => x.car_id,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageCar_Package_package_id",
                        column: x => x.package_id,
                        principalTable: "Package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_application_id",
                table: "ApplicationTracking",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_type_id",
                table: "ApplicationTracking",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_UserId",
                table: "ApplicationTracking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_instructor_id",
                table: "Car",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Car_license_category_id",
                table: "Car",
                column: "license_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Car_manufacturer_id",
                table: "Car",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_CarImage_car_id",
                table: "CarImage",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_Package_name",
                table: "Package",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_PackageCar_car_id",
                table: "PackageCar",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageCar_package_id",
                table: "PackageCar",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageCar_package_id_car_id",
                table: "PackageCar",
                columns: new[] { "package_id", "car_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUnavailability_date",
                table: "ScheduleUnavailability",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUnavailability_instructor_id",
                table: "ScheduleUnavailability",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUnavailability_instructor_id_date",
                table: "ScheduleUnavailability",
                columns: new[] { "instructor_id", "date" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocument_Instructor_id",
                table: "InstructorDocument",
                column: "id",
                principalTable: "Instructor",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_Instructor_id",
                table: "InstructorDocument");

            migrationBuilder.DropTable(
                name: "ApplicationTracking");

            migrationBuilder.DropTable(
                name: "CarImage");

            migrationBuilder.DropTable(
                name: "PackageCar");

            migrationBuilder.DropTable(
                name: "ScheduleUnavailability");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "LicenseCategory");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "background_profile_status",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_expiry_date",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_id_back",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_id_front",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_id_number",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_id_status",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_issue_date",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "citizen_issue_place",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "driving_license_back",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "driving_license_expiry",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "driving_license_front",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "driving_license_issue_date",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "driving_license_number",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "driving_license_status",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "submit_at",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "teaching_license_back",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "teaching_license_front",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "teaching_status",
                table: "InstructorDocument");

            migrationBuilder.RenameColumn(
                name: "permanent_address",
                table: "InstructorDocument",
                newName: "image_url");

            migrationBuilder.AddColumn<Guid>(
                name: "application_id",
                table: "InstructorDocument",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "type_id",
                table: "InstructorDocument",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InstructorApplication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    review_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    submit_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorApplication", x => x.id);
                    table.ForeignKey(
                        name: "FK_InstructorApplication_Instructor_id",
                        column: x => x.id,
                        principalTable: "Instructor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorApplication_Users_reviewer_id",
                        column: x => x.reviewer_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleAvailability",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleAvailability", x => x.id);
                    table.ForeignKey(
                        name: "FK_ScheduleAvailability_Instructor_instructor_id",
                        column: x => x.instructor_id,
                        principalTable: "Instructor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorDocument_application_id",
                table: "InstructorDocument",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorDocument_type_id",
                table: "InstructorDocument",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorApplication_reviewer_id",
                table: "InstructorApplication",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAvailability_instructor_id",
                table: "ScheduleAvailability",
                column: "instructor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocument_DocumentType_type_id",
                table: "InstructorDocument",
                column: "type_id",
                principalTable: "DocumentType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocument_InstructorApplication_application_id",
                table: "InstructorDocument",
                column: "application_id",
                principalTable: "InstructorApplication",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
