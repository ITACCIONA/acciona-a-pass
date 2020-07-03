using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    TableName = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    KeyValues = table.Column<int>(nullable: true),
                    OldValues = table.Column<string>(nullable: true),
                    NewValues = table.Column<string>(nullable: true),
                    RequestId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentroTrabajo",
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
                    table.PrimaryKey("PK_CentroTrabajo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
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
                    table.PrimaryKey("PK_Departamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Division",
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
                    table.PrimaryKey("PK_Division", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipoProteccion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoProteccion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoPasaporte",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoPasaporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FactorRiesgo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorRiesgo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FichaMedica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichaMedica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestMedico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestMedico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoParametroMedico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoParametroMedico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoSintomas",
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
                    table.PrimaryKey("PK_TipoSintomas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FichaLaboral",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdDivision = table.Column<int>(nullable: false),
                    IdDepartamento = table.Column<int>(nullable: false),
                    IdCentroTrabajo = table.Column<int>(nullable: false),
                    IdResponsableDirecto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichaLaboral", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichaLaboral_CentroTrabajo",
                        column: x => x.IdCentroTrabajo,
                        principalTable: "CentroTrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FichaLaboral_Departamento",
                        column: x => x.IdDepartamento,
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FichaLaboral_Division",
                        column: x => x.IdDivision,
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluacionFactoresRiesgo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdFichaMedica = table.Column<int>(nullable: false)
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
                name: "SeguimientoMedico",
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
                    Comentarios = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientoMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeguimientoMedico_FichaMedica",
                        column: x => x.IdFichaMedica,
                        principalTable: "FichaMedica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoTestMedico",
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
                    IdTestMedico = table.Column<int>(nullable: false),
                    Control = table.Column<bool>(nullable: false),
                    IGG = table.Column<bool>(nullable: false),
                    IGM = table.Column<bool>(nullable: false),
                    FechaTest = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoTestMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoTestMedico_FichaMedica",
                        column: x => x.IdFichaMedica,
                        principalTable: "FichaMedica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadoTestMedico_TestMedico",
                        column: x => x.IdTestMedico,
                        principalTable: "TestMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParametroMedico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdTipoParametro = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametroMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParametroMedico_TipoParametroMedico",
                        column: x => x.IdTipoParametro,
                        principalTable: "TipoParametroMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoEncuestaSintomas",
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
                    IdTipoSintoma = table.Column<int>(nullable: false),
                    Valor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoEncuestaSintomas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoEncuestaSintomas_FichaMedica",
                        column: x => x.IdFichaMedica,
                        principalTable: "FichaMedica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadoEncuestaSintomas_TipoSintomas",
                        column: x => x.IdTipoSintoma,
                        principalTable: "TipoSintomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdFichaLaboral = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<int>(nullable: false),
                    IdFichaMedica = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    NumEmpleado = table.Column<int>(nullable: true),
                    Edad = table.Column<int>(nullable: true),
                    Genero = table.Column<int>(nullable: true),
                    UltimaModif = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleado_FichaLaboral",
                        column: x => x.IdFichaLaboral,
                        principalTable: "FichaLaboral",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empleado_FichaMedica",
                        column: x => x.IdFichaMedica,
                        principalTable: "FichaMedica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empleado_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValoracionFactorRiesgo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdFactorRiesgo = table.Column<int>(nullable: false),
                    IdEvaluacionFactoresRiesgo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValoracionFactorRiesgo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValoracionFactorRiesgo_EvaluacionFactoresRiesgo",
                        column: x => x.IdEvaluacionFactoresRiesgo,
                        principalTable: "EvaluacionFactoresRiesgo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValoracionFactorRiesgo_FactorRiesgo",
                        column: x => x.IdFactorRiesgo,
                        principalTable: "FactorRiesgo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValoracionParametroMedico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdParametroMedico = table.Column<int>(nullable: false),
                    IdSegumientoMedico = table.Column<int>(nullable: false),
                    Valor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValoracionParametroMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValoracionParametroMedico_ParametroMedico",
                        column: x => x.IdParametroMedico,
                        principalTable: "ParametroMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValoracionParametroMedico_SeguimientoMedico",
                        column: x => x.IdSegumientoMedico,
                        principalTable: "SeguimientoMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertaEmpleado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEmpleado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertaEmpleado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertaEmpleado_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertaServiciosMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEmpleado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertaServiciosMedicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertaServiciosMedicos_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntregaEquipoProteccion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEquipoProteccion = table.Column<int>(nullable: false),
                    IdEmpleado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregaEquipoProteccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntregaEquipoProteccion_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntregaEquipoProteccion_EquipoProteccion",
                        column: x => x.IdEquipoProteccion,
                        principalTable: "EquipoProteccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pasaporte",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEstadoPasaporte = table.Column<int>(nullable: false),
                    IdAccion = table.Column<int>(nullable: false),
                    IdEmpleado = table.Column<int>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasaporte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasaporte_Accion",
                        column: x => x.IdAccion,
                        principalTable: "Accion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pasaporte_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pasaporte_EstadoPasaporte",
                        column: x => x.IdEstadoPasaporte,
                        principalTable: "EstadoPasaporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertaEmpleado_IdEmpleado",
                table: "AlertaEmpleado",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_AlertaServiciosMedicos_IdEmpleado",
                table: "AlertaServiciosMedicos",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdFichaLaboral",
                table: "Empleado",
                column: "IdFichaLaboral");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdFichaMedica",
                table: "Empleado",
                column: "IdFichaMedica");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdUsuario",
                table: "Empleado",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaEquipoProteccion_IdEmpleado",
                table: "EntregaEquipoProteccion",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaEquipoProteccion_IdEquipoProteccion",
                table: "EntregaEquipoProteccion",
                column: "IdEquipoProteccion");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionFactoresRiesgo_IdFichaMedica",
                table: "EvaluacionFactoresRiesgo",
                column: "IdFichaMedica");

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdCentroTrabajo",
                table: "FichaLaboral",
                column: "IdCentroTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdDepartamento",
                table: "FichaLaboral",
                column: "IdDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdDivision",
                table: "FichaLaboral",
                column: "IdDivision");

            migrationBuilder.CreateIndex(
                name: "IX_ParametroMedico_IdTipoParametro",
                table: "ParametroMedico",
                column: "IdTipoParametro");

            migrationBuilder.CreateIndex(
                name: "IX_Pasaporte_IdAccion",
                table: "Pasaporte",
                column: "IdAccion");

            migrationBuilder.CreateIndex(
                name: "IX_Pasaporte_IdEmpleado",
                table: "Pasaporte",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Pasaporte_IdEstadoPasaporte",
                table: "Pasaporte",
                column: "IdEstadoPasaporte");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoEncuestaSintomas_IdFichaMedica",
                table: "ResultadoEncuestaSintomas",
                column: "IdFichaMedica");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoEncuestaSintomas_IdTipoSintoma",
                table: "ResultadoEncuestaSintomas",
                column: "IdTipoSintoma");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoTestMedico_IdFichaMedica",
                table: "ResultadoTestMedico",
                column: "IdFichaMedica");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoTestMedico_IdTestMedico",
                table: "ResultadoTestMedico",
                column: "IdTestMedico");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientoMedico_IdFichaMedica",
                table: "SeguimientoMedico",
                column: "IdFichaMedica");

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionFactorRiesgo_IdEvaluacionFactoresRiesgo",
                table: "ValoracionFactorRiesgo",
                column: "IdEvaluacionFactoresRiesgo");

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionFactorRiesgo_IdFactorRiesgo",
                table: "ValoracionFactorRiesgo",
                column: "IdFactorRiesgo");

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionParametroMedico_IdParametroMedico",
                table: "ValoracionParametroMedico",
                column: "IdParametroMedico");

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionParametroMedico_IdSegumientoMedico",
                table: "ValoracionParametroMedico",
                column: "IdSegumientoMedico");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertaEmpleado");

            migrationBuilder.DropTable(
                name: "AlertaServiciosMedicos");

            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "EntregaEquipoProteccion");

            migrationBuilder.DropTable(
                name: "Pasaporte");

            migrationBuilder.DropTable(
                name: "ResultadoEncuestaSintomas");

            migrationBuilder.DropTable(
                name: "ResultadoTestMedico");

            migrationBuilder.DropTable(
                name: "ValoracionFactorRiesgo");

            migrationBuilder.DropTable(
                name: "ValoracionParametroMedico");

            migrationBuilder.DropTable(
                name: "EquipoProteccion");

            migrationBuilder.DropTable(
                name: "Accion");

            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "EstadoPasaporte");

            migrationBuilder.DropTable(
                name: "TipoSintomas");

            migrationBuilder.DropTable(
                name: "TestMedico");

            migrationBuilder.DropTable(
                name: "EvaluacionFactoresRiesgo");

            migrationBuilder.DropTable(
                name: "FactorRiesgo");

            migrationBuilder.DropTable(
                name: "ParametroMedico");

            migrationBuilder.DropTable(
                name: "SeguimientoMedico");

            migrationBuilder.DropTable(
                name: "FichaLaboral");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TipoParametroMedico");

            migrationBuilder.DropTable(
                name: "FichaMedica");

            migrationBuilder.DropTable(
                name: "CentroTrabajo");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "Division");
        }
    }
}
