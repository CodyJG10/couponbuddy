﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorTownship.Web.Data.Migrations
{
    public partial class ChangedUserLocationsToCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationReference_AspNetUsers_UserId",
                table: "LocationReference");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LocationReference",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationReference_AspNetUsers_UserId",
                table: "LocationReference",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationReference_AspNetUsers_UserId",
                table: "LocationReference");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LocationReference",
                nullable: true,
                oldClrType: typeof(string));

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
