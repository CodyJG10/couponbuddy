using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorTownship.Web.Data.Migrations
{
    public partial class VendorLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationsJson",
                table: "Vendors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationsJson",
                table: "Vendors");
        }
    }
}
