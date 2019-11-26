using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorTownship.Web.Data.Migrations
{
    public partial class AddedUserLocationsToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationReference_AspNetUsers_UserId",
                table: "LocationReference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationReference",
                table: "LocationReference");

            migrationBuilder.RenameTable(
                name: "LocationReference",
                newName: "UserLocations");

            migrationBuilder.RenameIndex(
                name: "IX_LocationReference_UserId",
                table: "UserLocations",
                newName: "IX_UserLocations_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLocations",
                table: "UserLocations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocations_AspNetUsers_UserId",
                table: "UserLocations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLocations_AspNetUsers_UserId",
                table: "UserLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLocations",
                table: "UserLocations");

            migrationBuilder.RenameTable(
                name: "UserLocations",
                newName: "LocationReference");

            migrationBuilder.RenameIndex(
                name: "IX_UserLocations_UserId",
                table: "LocationReference",
                newName: "IX_LocationReference_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationReference",
                table: "LocationReference",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationReference_AspNetUsers_UserId",
                table: "LocationReference",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
