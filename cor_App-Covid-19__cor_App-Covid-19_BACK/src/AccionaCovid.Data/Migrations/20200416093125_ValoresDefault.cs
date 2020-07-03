using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class ValoresDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Valor",
                table: "ValoracionParametroMedico",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ValoracionParametroMedico",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ValoracionParametroMedico",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ValoracionFactorRiesgo",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ValoracionFactorRiesgo",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<bool>(
                name: "Valor",
                table: "ValoracionFactorRiesgo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "UsuarioWorkDay",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "UsuarioWorkDay",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "TipoSintomas",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "TipoSintomas",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "TipoParametroMedico",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "TipoParametroMedico",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "SeguimientoMedico",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "SeguimientoMedico",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ResultadoTestPCR",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ResultadoTestPCR",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ResultadoTestMedico",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ResultadoTestMedico",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ResultadoEncuestaSintomas",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ResultadoEncuestaSintomas",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Pasaporte",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Pasaporte",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ParametroMedico",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ParametroMedico",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Localizacion",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Localizacion",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "FichaMedica",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "FichaMedica",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "FichaLaboral",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "FichaLaboral",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "FactorRiesgo",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "FactorRiesgo",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "EstadoPasaporte",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "EstadoPasaporte",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "EquipoProteccion",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "EquipoProteccion",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "EntregaEquipoProteccion",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "EntregaEquipoProteccion",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Empleado",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Empleado",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Division",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Division",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Departamento",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Departamento",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Audits",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Audits",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "AlertaServiciosMedicos",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "AlertaServiciosMedicos",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "AlertaEmpleado",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "AlertaEmpleado",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Accion",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Accion",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "(N'CREATE')",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.AlterColumn<int>(
                name: "Valor",
                table: "ValoracionParametroMedico",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ValoracionParametroMedico",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ValoracionParametroMedico",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ValoracionFactorRiesgo",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ValoracionFactorRiesgo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "UsuarioWorkDay",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "UsuarioWorkDay",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "TipoSintomas",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "TipoSintomas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "TipoParametroMedico",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "TipoParametroMedico",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "SeguimientoMedico",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "SeguimientoMedico",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ResultadoTestPCR",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ResultadoTestPCR",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ResultadoTestMedico",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ResultadoTestMedico",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ResultadoEncuestaSintomas",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ResultadoEncuestaSintomas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Pasaporte",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Pasaporte",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "ParametroMedico",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "ParametroMedico",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Localizacion",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Localizacion",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "FichaMedica",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "FichaMedica",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "FichaLaboral",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "FichaLaboral",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "FactorRiesgo",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "FactorRiesgo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "EstadoPasaporte",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "EstadoPasaporte",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "EquipoProteccion",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "EquipoProteccion",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "EntregaEquipoProteccion",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "EntregaEquipoProteccion",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Empleado",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Empleado",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Division",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Division",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Departamento",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Departamento",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Audits",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Audits",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "AlertaServiciosMedicos",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "AlertaServiciosMedicos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "AlertaEmpleado",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "AlertaEmpleado",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActionDate",
                table: "Accion",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "LastAction",
                table: "Accion",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldDefaultValueSql: "(N'CREATE')");
        }
    }
}
