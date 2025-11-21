using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixInstructorRegistrationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "driving_license_expiry_date",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "driving_license_issued_date",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "driving_license_number",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "id_expiry_date",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "id_issued_date",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "id_issued_location",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "national_id_number",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "permanent_address",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "teaching_license_back",
                table: "InstructorApplication");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "driving_license_expiry_date",
                table: "InstructorApplication",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "driving_license_issued_date",
                table: "InstructorApplication",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "driving_license_number",
                table: "InstructorApplication",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "id_expiry_date",
                table: "InstructorApplication",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "id_issued_date",
                table: "InstructorApplication",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "id_issued_location",
                table: "InstructorApplication",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "national_id_number",
                table: "InstructorApplication",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "permanent_address",
                table: "InstructorApplication",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "teaching_license_back",
                table: "InstructorApplication",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
