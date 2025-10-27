using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class Add_Feild_Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_PackageType_PackageTypeId",
                table: "Package");

            migrationBuilder.RenameColumn(
                name: "PackageTypeId",
                table: "Package",
                newName: "package_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_Package_PackageTypeId",
                table: "Package",
                newName: "IX_Package_package_type_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Feedback",
                newName: "novice_driver_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_PackageType_package_type_id",
                table: "Package",
                column: "package_type_id",
                principalTable: "PackageType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_PackageType_package_type_id",
                table: "Package");

            migrationBuilder.RenameColumn(
                name: "package_type_id",
                table: "Package",
                newName: "PackageTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Package_package_type_id",
                table: "Package",
                newName: "IX_Package_PackageTypeId");

            migrationBuilder.RenameColumn(
                name: "novice_driver_id",
                table: "Feedback",
                newName: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_PackageType_PackageTypeId",
                table: "Package",
                column: "PackageTypeId",
                principalTable: "PackageType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
