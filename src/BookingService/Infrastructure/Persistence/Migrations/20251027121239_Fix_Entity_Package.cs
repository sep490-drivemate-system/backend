using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Entity_Package : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_PackageType_package_id",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageType_Package_PackageId",
                table: "PackageType");

            migrationBuilder.DropIndex(
                name: "IX_PackageType_PackageId",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "car_id",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "price",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Package");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Package",
                newName: "price");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "PackageType",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "PackageType",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "value",
                table: "PackageType",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageTypeId",
                table: "Package",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "car_id",
                table: "Package",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "instructor_id",
                table: "Package",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "type_rental",
                table: "Package",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "instructor_id",
                table: "Feedback",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Package_PackageTypeId",
                table: "Package",
                column: "PackageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Package_package_id",
                table: "Booking",
                column: "package_id",
                principalTable: "Package",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Package_PackageType_PackageTypeId",
                table: "Package",
                column: "PackageTypeId",
                principalTable: "PackageType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Package_package_id",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Package_PackageType_PackageTypeId",
                table: "Package");

            migrationBuilder.DropIndex(
                name: "IX_Package_PackageTypeId",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "description",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "type",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "value",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "PackageTypeId",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "car_id",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "instructor_id",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "type_rental",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "instructor_id",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Package",
                newName: "value");

            migrationBuilder.AddColumn<Guid>(
                name: "PackageId",
                table: "PackageType",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "car_id",
                table: "PackageType",
                type: "uuid",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "PackageType",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Package",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Package",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PackageType_PackageId",
                table: "PackageType",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_PackageType_package_id",
                table: "Booking",
                column: "package_id",
                principalTable: "PackageType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageType_Package_PackageId",
                table: "PackageType",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
