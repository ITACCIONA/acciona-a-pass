using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class AñadidoColumUPN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UPN",
                table: "UsuarioWorkDay",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPN",
                table: "Empleado",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UPN",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "UPN",
                table: "Empleado");
        }
    }
}
