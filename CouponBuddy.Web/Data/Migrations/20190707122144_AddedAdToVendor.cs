using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorTownship.Web.Data.Migrations
{
    public partial class AddedAdToVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdId",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Ads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_AdId",
                table: "Vendors",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Ads_AdId",
                table: "Vendors",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Ads_AdId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_AdId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ads");
        }
    }
}
