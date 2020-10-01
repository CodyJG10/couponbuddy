using Microsoft.EntityFrameworkCore.Migrations;

namespace CouponBuddy.WebCore.Data.Migrations
{
    public partial class AddedAdToVendor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "HomePageAdId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ads");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Ads",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_VendorId",
                table: "Ads",
                column: "VendorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Vendors_VendorId",
                table: "Ads",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Vendors_VendorId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_VendorId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Ads");

            migrationBuilder.AddColumn<int>(
                name: "AdId",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomePageAdId",
                table: "Vendors",
                nullable: false,
                defaultValue: 0);

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
    }
}
