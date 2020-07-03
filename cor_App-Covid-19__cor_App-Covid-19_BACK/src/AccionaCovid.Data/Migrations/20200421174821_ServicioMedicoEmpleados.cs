using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class ServicioMedicoEmpleados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsServicioMedico",
                table: "Empleado",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsServicioMedico",
                table: "Empleado");
        }
    }
}
