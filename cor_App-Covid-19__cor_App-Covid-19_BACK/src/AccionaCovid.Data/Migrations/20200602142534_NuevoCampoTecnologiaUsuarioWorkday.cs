using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class NuevoCampoTecnologiaUsuarioWorkday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tecnologia",
                table: "UsuarioWorkDay",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tecnologia",
                table: "UsuarioWorkDay");
        }
    }
}
