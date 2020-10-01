using Microsoft.EntityFrameworkCore.Migrations;

namespace CouponBuddy.WebCore.Data.Migrations
{
    public partial class AddedVendorAnalytics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorAnalytics",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    VendorId = table.Column<string>(nullable: true),
                    LocationId = table.Column<string>(nullable: true),
                    VendorImpressionsJson = table.Column<string>(nullable: true),
                    VendorClicksJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorAnalytics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorAnalytics");
        }
    }
}
