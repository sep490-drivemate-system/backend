using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ThisShouldFixIt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedLocation_NoviceDriver_NoviceDriverId",
                table: "SavedLocation");

            migrationBuilder.DropIndex(
                name: "IX_SavedLocation_NoviceDriverId",
                table: "SavedLocation");

            migrationBuilder.DropColumn(
                name: "NoviceDriverId",
                table: "SavedLocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NoviceDriverId",
                table: "SavedLocation",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedLocation_NoviceDriverId",
                table: "SavedLocation",
                column: "NoviceDriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedLocation_NoviceDriver_NoviceDriverId",
                table: "SavedLocation",
                column: "NoviceDriverId",
                principalTable: "NoviceDriver",
                principalColumn: "id");
        }
    }
}
