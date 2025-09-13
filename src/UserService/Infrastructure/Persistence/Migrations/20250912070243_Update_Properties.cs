using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocuments_DocumentTypes_TypeId",
                table: "InstructorDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocuments_InstructorApplications_InstructorApplic~",
                table: "InstructorDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_NoviceDrivers_InstructorApplications_ApplicationId",
                table: "NoviceDrivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NoviceDrivers",
                table: "NoviceDrivers");

            migrationBuilder.DropIndex(
                name: "IX_NoviceDrivers_ApplicationId",
                table: "NoviceDrivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorDocuments",
                table: "InstructorDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorApplications",
                table: "InstructorApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "NoviceDrivers");

            migrationBuilder.DropColumn(
                name: "DrivingLicense",
                table: "NoviceDrivers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InstructorApplications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DocumentTypes");

            migrationBuilder.RenameTable(
                name: "NoviceDrivers",
                newName: "NoviceDriver");

            migrationBuilder.RenameTable(
                name: "Instructors",
                newName: "Instructor");

            migrationBuilder.RenameTable(
                name: "InstructorDocuments",
                newName: "InstructorDocument");

            migrationBuilder.RenameTable(
                name: "InstructorApplications",
                newName: "InstructorApplication");

            migrationBuilder.RenameTable(
                name: "DocumentTypes",
                newName: "DocumentType");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NoviceDriver",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "NoviceDriver",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleTe",
                table: "NoviceDriver",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "AllowedBooking",
                table: "NoviceDriver",
                newName: "allowed_booking");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Instructor",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Instructor",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Instructor",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Instructor",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleTe",
                table: "Instructor",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "Experence",
                table: "Instructor",
                newName: "experience");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "InstructorDocument",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InstructorDocument",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "InstructorDocument",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "InstructorDocument",
                newName: "type_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleTe",
                table: "InstructorDocument",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "InstructorDocument",
                newName: "application_id");

            migrationBuilder.RenameColumn(
                name: "InstructorApplicationId",
                table: "InstructorDocument",
                newName: "DocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorDocuments_TypeId",
                table: "InstructorDocument",
                newName: "IX_InstructorDocument_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorDocuments_InstructorApplicationId",
                table: "InstructorDocument",
                newName: "IX_InstructorDocument_DocumentTypeId");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "InstructorApplication",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InstructorApplication",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "InstructorApplication",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DocumentType",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DocumentType",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DocumentType",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "DocumentType",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "address",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "address",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "address",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "address",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleTe",
                table: "address",
                newName: "is_delete");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "NoviceDriver",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "NoviceDriver",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "is_delete",
                table: "NoviceDriver",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "driving_license_image_url",
                table: "NoviceDriver",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "Instructor",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "Instructor",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "Instructor",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "is_delete",
                table: "Instructor",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "InstructorDocument",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "InstructorDocument",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "InstructorDocument",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "is_delete",
                table: "InstructorDocument",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "InstructorDocument",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "InstructorApplication",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "InstructorApplication",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "InstructorApplication",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "InstructorApplication",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "review_at",
                table: "InstructorApplication",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "reviewer_id",
                table: "InstructorApplication",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "InstructorApplication",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "submit_at",
                table: "InstructorApplication",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "DocumentType",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "DocumentType",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "DocumentType",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "DocumentType",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "DocumentType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "address",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "location",
                table: "address",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "address",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "is_delete",
                table: "address",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoviceDriver",
                table: "NoviceDriver",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructor",
                table: "Instructor",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorDocument",
                table: "InstructorDocument",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorApplication",
                table: "InstructorApplication",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentType",
                table: "DocumentType",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                column: "id");

            migrationBuilder.CreateTable(
                name: "ScheduleAvailability",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    instructor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp", nullable: false)
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
                name: "IX_InstructorApplication_reviewer_id",
                table: "InstructorApplication",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_user_id",
                table: "address",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAvailability_instructor_id",
                table: "ScheduleAvailability",
                column: "instructor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_address_Users_user_id",
                table: "address",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Users_id",
                table: "Instructor",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorApplication_Instructor_id",
                table: "InstructorApplication",
                column: "id",
                principalTable: "Instructor",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorApplication_Users_reviewer_id",
                table: "InstructorApplication",
                column: "reviewer_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocument_DocumentType_DocumentTypeId",
                table: "InstructorDocument",
                column: "DocumentTypeId",
                principalTable: "DocumentType",
                principalColumn: "id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_NoviceDriver_Users_id",
                table: "NoviceDriver",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_id",
                table: "RefreshToken",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_address_Users_user_id",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Users_id",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorApplication_Instructor_id",
                table: "InstructorApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorApplication_Users_reviewer_id",
                table: "InstructorApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_DocumentType_DocumentTypeId",
                table: "InstructorDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_DocumentType_type_id",
                table: "InstructorDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_InstructorApplication_application_id",
                table: "InstructorDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_NoviceDriver_Users_id",
                table: "NoviceDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_id",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ScheduleAvailability");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NoviceDriver",
                table: "NoviceDriver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorDocument",
                table: "InstructorDocument");

            migrationBuilder.DropIndex(
                name: "IX_InstructorDocument_application_id",
                table: "InstructorDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorApplication",
                table: "InstructorApplication");

            migrationBuilder.DropIndex(
                name: "IX_InstructorApplication_reviewer_id",
                table: "InstructorApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructor",
                table: "Instructor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentType",
                table: "DocumentType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.DropIndex(
                name: "IX_address_user_id",
                table: "address");

            migrationBuilder.DropColumn(
                name: "driving_license_image_url",
                table: "NoviceDriver");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "InstructorDocument");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "review_at",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "reviewer_id",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "status",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "submit_at",
                table: "InstructorApplication");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "DocumentType");

            migrationBuilder.RenameTable(
                name: "NoviceDriver",
                newName: "NoviceDrivers");

            migrationBuilder.RenameTable(
                name: "InstructorDocument",
                newName: "InstructorDocuments");

            migrationBuilder.RenameTable(
                name: "InstructorApplication",
                newName: "InstructorApplications");

            migrationBuilder.RenameTable(
                name: "Instructor",
                newName: "Instructors");

            migrationBuilder.RenameTable(
                name: "DocumentType",
                newName: "DocumentTypes");

            migrationBuilder.RenameTable(
                name: "address",
                newName: "Addresses");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NoviceDrivers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "NoviceDrivers",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "NoviceDrivers",
                newName: "IsDeleTe");

            migrationBuilder.RenameColumn(
                name: "allowed_booking",
                table: "NoviceDrivers",
                newName: "AllowedBooking");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "InstructorDocuments",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "InstructorDocuments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "InstructorDocuments",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "InstructorDocuments",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "InstructorDocuments",
                newName: "IsDeleTe");

            migrationBuilder.RenameColumn(
                name: "application_id",
                table: "InstructorDocuments",
                newName: "ApplicationId");

            migrationBuilder.RenameColumn(
                name: "DocumentTypeId",
                table: "InstructorDocuments",
                newName: "InstructorApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorDocument_type_id",
                table: "InstructorDocuments",
                newName: "IX_InstructorDocuments_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorDocument_DocumentTypeId",
                table: "InstructorDocuments",
                newName: "IX_InstructorDocuments_InstructorApplicationId");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "InstructorApplications",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "InstructorApplications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "InstructorApplications",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Instructors",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "Instructors",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Instructors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "Instructors",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "Instructors",
                newName: "IsDeleTe");

            migrationBuilder.RenameColumn(
                name: "experience",
                table: "Instructors",
                newName: "Experence");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "DocumentTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "DocumentTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DocumentTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "DocumentTypes",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Addresses",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Addresses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Addresses",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "Addresses",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "Addresses",
                newName: "IsDeleTe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "NoviceDrivers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "NoviceDrivers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleTe",
                table: "NoviceDrivers",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "NoviceDrivers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicense",
                table: "NoviceDrivers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "InstructorDocuments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "InstructorDocuments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "InstructorDocuments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleTe",
                table: "InstructorDocuments",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "InstructorApplications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "InstructorApplications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "InstructorApplications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InstructorApplications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "Instructors",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "Instructors",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleTe",
                table: "Instructors",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DocumentTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DocumentTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "DocumentTypes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "DocumentTypes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DocumentTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Addresses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleTe",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoviceDrivers",
                table: "NoviceDrivers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorDocuments",
                table: "InstructorDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorApplications",
                table: "InstructorApplications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NoviceDrivers_ApplicationId",
                table: "NoviceDrivers",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocuments_DocumentTypes_TypeId",
                table: "InstructorDocuments",
                column: "TypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocuments_InstructorApplications_InstructorApplic~",
                table: "InstructorDocuments",
                column: "InstructorApplicationId",
                principalTable: "InstructorApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoviceDrivers_InstructorApplications_ApplicationId",
                table: "NoviceDrivers",
                column: "ApplicationId",
                principalTable: "InstructorApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
