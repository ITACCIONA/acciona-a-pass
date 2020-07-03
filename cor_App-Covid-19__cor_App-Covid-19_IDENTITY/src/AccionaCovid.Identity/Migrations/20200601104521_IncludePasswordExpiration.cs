using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Identity.Migrations
{
    public partial class IncludePasswordExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PasswordExpiration",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordExpiration",
                table: "AspNetUsers");
        }
    }
}
