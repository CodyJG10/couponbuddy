using Microsoft.EntityFrameworkCore.Migrations;

namespace CouponBuddy.WebCore.Data.Migrations
{
    public partial class VendorEntityUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Vendors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Vendors");
        }
    }
}
