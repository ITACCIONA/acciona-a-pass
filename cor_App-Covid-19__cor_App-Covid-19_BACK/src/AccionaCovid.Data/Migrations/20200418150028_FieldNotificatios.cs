using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class FieldNotificatios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Valor",
                table: "ValoracionFactorRiesgo",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "IdResponsable",
                table: "UsuarioWorkDay",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsServicioMedico",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "Division",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Departamento",
                table: "UsuarioWorkDay",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "SeguimientoMedico",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Pasaporte",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Localizacion",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "(N'')",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAlta",
                table: "FichaMedica",
                type: "datetime",
                nullable: false,
                defaultValueSql: "('0001-01-01T00:00:00.000')",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<bool>(
                name: "Bloqueado",
                table: "Empleado",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "IdWorkday",
                table: "Division",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "IdWorkday",
                table: "Departamento",
                nullable: false,
                defaultValueSql: "(CONVERT([bigint],(0)))",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "AlertaServiciosMedicos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FechaNotificacion",
                table: "AlertaServiciosMedicos",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "AlertaServiciosMedicos",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "AlertaServiciosMedicos");

            migrationBuilder.DropColumn(
                name: "FechaNotificacion",
                table: "AlertaServiciosMedicos");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "AlertaServiciosMedicos");

            migrationBuilder.AlterColumn<bool>(
                name: "Valor",
                table: "ValoracionFactorRiesgo",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "IdResponsable",
                table: "UsuarioWorkDay",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsServicioMedico",
                table: "UsuarioWorkDay",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "Division",
                table: "UsuarioWorkDay",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "Departamento",
                table: "UsuarioWorkDay",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "SeguimientoMedico",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Pasaporte",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Localizacion",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldDefaultValueSql: "(N'')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAlta",
                table: "FichaMedica",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "('0001-01-01T00:00:00.000')");

            migrationBuilder.AlterColumn<bool>(
                name: "Bloqueado",
                table: "Empleado",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(CONVERT([bit],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "IdWorkday",
                table: "Division",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");

            migrationBuilder.AlterColumn<long>(
                name: "IdWorkday",
                table: "Departamento",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "(CONVERT([bigint],(0)))");
        }
    }
}
