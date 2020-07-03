using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class CambioEstructura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Usuario",
                table: "Empleado");

            migrationBuilder.DropForeignKey(
                name: "FK_FichaLaboral_CentroTrabajo",
                table: "FichaLaboral");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoTestMedico_TestMedico",
                table: "ResultadoTestMedico");

            migrationBuilder.DropForeignKey(
                name: "FK_ValoracionFactorRiesgo_EvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.DropTable(
                name: "CentroTrabajo");

            migrationBuilder.DropTable(
                name: "EvaluacionFactoresRiesgo");

            migrationBuilder.DropTable(
                name: "TestMedico");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_ValoracionFactorRiesgo_IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.DropIndex(
                name: "IX_ResultadoTestMedico_IdTestMedico",
                table: "ResultadoTestMedico");

            migrationBuilder.DropIndex(
                name: "IX_FichaLaboral_IdCentroTrabajo",
                table: "FichaLaboral");

            migrationBuilder.DropIndex(
                name: "IX_Empleado_IdUsuario",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.DropColumn(
                name: "IdTestMedico",
                table: "ResultadoTestMedico");

            migrationBuilder.DropColumn(
                name: "IdCentroTrabajo",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Empleado");

            migrationBuilder.AddColumn<int>(
                name: "IdFichaMedica",
                table: "ValoracionFactorRiesgo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "SeguimientoMedico",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Pasaporte",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "FichaMedica",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "IdResponsableDirecto",
                table: "FichaLaboral",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdLocalizacion",
                table: "FichaLaboral",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdLocalizacionAlter",
                table: "FichaLaboral",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailProf",
                table: "FichaLaboral",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Obra",
                table: "FichaLaboral",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoCorp",
                table: "FichaLaboral",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdFichaMedica",
                table: "Empleado",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdFichaLaboral",
                table: "Empleado",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Empleado",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioWorkDay",
                table: "Empleado",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Empleado",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIF",
                table: "Empleado",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Empleado",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Localizacion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoTestPCR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdFichaMedica = table.Column<int>(nullable: false),
                    Positivo = table.Column<bool>(nullable: false),
                    FechaTest = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoTestPCR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoTestPCR_FichaMedica",
                        column: x => x.IdFichaMedica,
                        principalTable: "FichaMedica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioWorkDay",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdWorkDay = table.Column<string>(maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    NumEmpleado = table.Column<int>(nullable: true),
                    Edad = table.Column<int>(nullable: true),
                    Genero = table.Column<int>(nullable: true),
                    UltimaModif = table.Column<DateTime>(type: "datetime", nullable: false),
                    Apellido = table.Column<string>(nullable: true),
                    NIF = table.Column<string>(maxLength: 50, nullable: true),
                    Mail = table.Column<string>(maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioWorkDay", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionFactorRiesgo_IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo",
                column: "IdFichaMedica");

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdLocalizacion",
                table: "FichaLaboral",
                column: "IdLocalizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdUsuario",
                table: "Empleado",
                column: "IdUsuarioWorkDay");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoTestPCR_IdFichaMedica",
                table: "ResultadoTestPCR",
                column: "IdFichaMedica");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_UsuarioWorkDay",
                table: "Empleado",
                column: "IdUsuarioWorkDay",
                principalTable: "UsuarioWorkDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FichaLaboral_Localizacion3",
                table: "FichaLaboral",
                column: "IdLocalizacion",
                principalTable: "Localizacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValoracionFactorRiesgo_FichaMedica",
                table: "ValoracionFactorRiesgo",
                column: "IdFichaMedica",
                principalTable: "FichaMedica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_UsuarioWorkDay",
                table: "Empleado");

            migrationBuilder.DropForeignKey(
                name: "FK_FichaLaboral_Localizacion3",
                table: "FichaLaboral");

            migrationBuilder.DropForeignKey(
                name: "FK_ValoracionFactorRiesgo_FichaMedica",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.DropTable(
                name: "Localizacion");

            migrationBuilder.DropTable(
                name: "ResultadoTestPCR");

            migrationBuilder.DropTable(
                name: "UsuarioWorkDay");

            migrationBuilder.DropIndex(
                name: "IX_ValoracionFactorRiesgo_IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.DropIndex(
                name: "IX_FichaLaboral_IdLocalizacion",
                table: "FichaLaboral");

            migrationBuilder.DropIndex(
                name: "IX_Empleado_IdUsuario",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "IdFichaMedica",
                table: "ValoracionFactorRiesgo");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "SeguimientoMedico");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Pasaporte");

            migrationBuilder.DropColumn(
                name: "FechaAlta",
                table: "FichaMedica");

            migrationBuilder.DropColumn(
                name: "IdLocalizacion",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "IdLocalizacionAlter",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "MailProf",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "Obra",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "TelefonoCorp",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "IdUsuarioWorkDay",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "NIF",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Empleado");

            migrationBuilder.AddColumn<int>(
                name: "IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTestMedico",
                table: "ResultadoTestMedico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdResponsableDirecto",
                table: "FichaLaboral",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCentroTrabajo",
                table: "FichaLaboral",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdFichaMedica",
                table: "Empleado",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdFichaLaboral",
                table: "Empleado",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Empleado",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CentroTrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    LastAction = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentroTrabajo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvaluacionFactoresRiesgo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IdFichaMedica = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    LastAction = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionFactoresRiesgo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluacionFactoresRiesgo_FichaMedica",
                        column: x => x.IdFichaMedica,
                        principalTable: "FichaMedica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestMedico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    LastAction = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestMedico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    LastAction = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionFactorRiesgo_IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo",
                column: "IdEvaluacionFactoresRiesgo");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoTestMedico_IdTestMedico",
                table: "ResultadoTestMedico",
                column: "IdTestMedico");

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdCentroTrabajo",
                table: "FichaLaboral",
                column: "IdCentroTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdUsuario",
                table: "Empleado",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionFactoresRiesgo_IdFichaMedica",
                table: "EvaluacionFactoresRiesgo",
                column: "IdFichaMedica");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Usuario",
                table: "Empleado",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FichaLaboral_CentroTrabajo",
                table: "FichaLaboral",
                column: "IdCentroTrabajo",
                principalTable: "CentroTrabajo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoTestMedico_TestMedico",
                table: "ResultadoTestMedico",
                column: "IdTestMedico",
                principalTable: "TestMedico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValoracionFactorRiesgo_EvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo",
                column: "IdEvaluacionFactoresRiesgo",
                principalTable: "EvaluacionFactoresRiesgo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
