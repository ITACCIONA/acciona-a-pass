using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class CamposUsuarioWorkday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Empleado");

            migrationBuilder.AlterColumn<long>(
                name: "IdWorkDay",
                table: "UsuarioWorkDay",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Apellido1",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apellido2",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Departamento",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Division",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "EsServicioMedico",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdResponsable",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localizacion",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailCorporativo",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoCorporativo",
                table: "UsuarioWorkDay",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Empleado",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido1",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "Apellido2",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "EsServicioMedico",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "IdResponsable",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "Localizacion",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "MailCorporativo",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "TelefonoCorporativo",
                table: "UsuarioWorkDay");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Empleado");

            migrationBuilder.AlterColumn<string>(
                name: "IdWorkDay",
                table: "UsuarioWorkDay",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(long),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "UsuarioWorkDay",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "UsuarioWorkDay",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Empleado",
                type: "int",
                nullable: true);
        }
    }
}
