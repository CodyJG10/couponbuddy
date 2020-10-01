using Microsoft.EntityFrameworkCore.Migrations;

namespace CouponBuddy.WebCore.Data.Migrations
{
    public partial class AddedForeignKeyToVendorMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorMedia_Vendors_VendorId",
                table: "VendorMedia");

            migrationBuilder.AlterColumn<int>(
                name: "VendorId",
                table: "VendorMedia",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VendorMedia_Vendors_VendorId",
                table: "VendorMedia",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorMedia_Vendors_VendorId",
                table: "VendorMedia");

            migrationBuilder.AlterColumn<int>(
                name: "VendorId",
                table: "VendorMedia",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorMedia_Vendors_VendorId",
                table: "VendorMedia",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
