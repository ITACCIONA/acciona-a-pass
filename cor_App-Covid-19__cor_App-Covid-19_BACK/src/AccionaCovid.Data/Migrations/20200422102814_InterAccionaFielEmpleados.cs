using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class InterAccionaFielEmpleados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InterAcciona",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.AddColumn<bool>(
                name: "InterAcciona",
                table: "Empleado",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterAcciona",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "InterAcciona",
                table: "Empleado");
        }
    }
}
