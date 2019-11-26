using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorTownship.Web.Data.Migrations
{
    public partial class AddedVendorMediaToVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "VendorMedia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VendorMedia_VendorId",
                table: "VendorMedia",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorMedia_Vendors_VendorId",
                table: "VendorMedia",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorMedia_Vendors_VendorId",
                table: "VendorMedia");

            migrationBuilder.DropIndex(
                name: "IX_VendorMedia_VendorId",
                table: "VendorMedia");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "VendorMedia");
        }
    }
}
