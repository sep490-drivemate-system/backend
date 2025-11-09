using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstructorApplicationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "driving_license_level",
                table: "InstructorApplication",
                newName: "driving_license_tier");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "InstructorApplication",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.AddColumn<int>(
                name: "teaching_license_tier",
                table: "InstructorApplication",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "teaching_license_tier",
                table: "InstructorApplication");

            migrationBuilder.RenameColumn(
                name: "driving_license_tier",
                table: "InstructorApplication",
                newName: "driving_license_level");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "InstructorApplication",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);
        }
    }
}
