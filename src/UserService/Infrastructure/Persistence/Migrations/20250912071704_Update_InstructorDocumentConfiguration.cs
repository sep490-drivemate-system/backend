using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_InstructorDocumentConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_address_Users_user_id",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorDocument_DocumentType_DocumentTypeId",
                table: "InstructorDocument");

            migrationBuilder.DropIndex(
                name: "IX_InstructorDocument_DocumentTypeId",
                table: "InstructorDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "InstructorDocument");

            migrationBuilder.RenameTable(
                name: "address",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_address_user_id",
                table: "Address",
                newName: "IX_Address_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Users_user_id",
                table: "Address",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Users_user_id",
                table: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "address");

            migrationBuilder.RenameIndex(
                name: "IX_Address_user_id",
                table: "address",
                newName: "IX_address_user_id");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "InstructorDocument",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorDocument_DocumentTypeId",
                table: "InstructorDocument",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_address_Users_user_id",
                table: "address",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorDocument_DocumentType_DocumentTypeId",
                table: "InstructorDocument",
                column: "DocumentTypeId",
                principalTable: "DocumentType",
                principalColumn: "id");
        }
    }
}
