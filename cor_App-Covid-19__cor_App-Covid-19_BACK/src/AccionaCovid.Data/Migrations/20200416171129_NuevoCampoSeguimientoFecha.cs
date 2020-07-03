using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class NuevoCampoSeguimientoFecha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSeguimiento",
                table: "SeguimientoMedico",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaSeguimiento",
                table: "SeguimientoMedico");
        }
    }
}
