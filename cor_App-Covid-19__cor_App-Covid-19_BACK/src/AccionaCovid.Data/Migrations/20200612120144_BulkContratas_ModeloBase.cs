using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class BulkContratas_ModeloBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdIntegracionExternos",
                table: "Empleado",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EstadoObra",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoObra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracionExternos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nif = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(nullable: true),
                    Apellido1 = table.Column<string>(nullable: true),
                    Apellido2 = table.Column<string>(nullable: true),
                    Origen = table.Column<string>(nullable: true),
                    NifOriginal = table.Column<string>(nullable: true),
                    UltimaModif = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracionExternos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcontrata",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Cif = table.Column<string>(maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcontrata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Obra",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    CodigoObra = table.Column<string>(maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    IdEstadoObra = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obra_EstadoObra",
                        column: x => x.IdEstadoObra,
                        principalTable: "EstadoObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdjudicacionTrabajoObra",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdObra = table.Column<int>(nullable: false),
                    IdSubcontrata = table.Column<int>(nullable: false),
                    IdEmpleadoResponsable = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjudicacionTrabajoObra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdjudicacionTrabajoObra_Empleado",
                        column: x => x.IdEmpleadoResponsable,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdjudicacionTrabajoObra_Obra",
                        column: x => x.IdObra,
                        principalTable: "Obra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdjudicacionTrabajoObra_Subcontrata",
                        column: x => x.IdSubcontrata,
                        principalTable: "Subcontrata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionAdjudicacion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdFichaLaboral = table.Column<int>(nullable: false),
                    IdAdjudicacionTrabajoObra = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionAdjudicacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionAdjudicacion_AdjudicacionTrabajoObra",
                        column: x => x.IdAdjudicacionTrabajoObra,
                        principalTable: "AdjudicacionTrabajoObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionAdjudicacion_FichaLaboral",
                        column: x => x.IdFichaLaboral,
                        principalTable: "FichaLaboral",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdIntegracionExternos",
                table: "Empleado",
                column: "IdIntegracionExternos");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicacionTrabajoObra_IdEmpleadoResponsable",
                table: "AdjudicacionTrabajoObra",
                column: "IdEmpleadoResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicacionTrabajoObra_IdObra",
                table: "AdjudicacionTrabajoObra",
                column: "IdObra");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicacionTrabajoObra_IdSubcontrata",
                table: "AdjudicacionTrabajoObra",
                column: "IdSubcontrata");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionAdjudicacion_IdAdjudicacionTrabajoObra",
                table: "AsignacionAdjudicacion",
                column: "IdAdjudicacionTrabajoObra");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionAdjudicacion_IdFichaLaboral",
                table: "AsignacionAdjudicacion",
                column: "IdFichaLaboral");

            migrationBuilder.CreateIndex(
                name: "IX_IntegracionExternos_Nif",
                table: "IntegracionExternos",
                column: "Nif",
                unique: true,
                filter: "[Nif] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdEstadoObra",
                table: "Obra",
                column: "IdEstadoObra");

            migrationBuilder.CreateIndex(
                name: "IX_Subcontrata_Cif",
                table: "Subcontrata",
                column: "Cif",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_IntegracionExternos",
                table: "Empleado",
                column: "IdIntegracionExternos",
                principalTable: "IntegracionExternos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_IntegracionExternos",
                table: "Empleado");

            migrationBuilder.DropTable(
                name: "AsignacionAdjudicacion");

            migrationBuilder.DropTable(
                name: "IntegracionExternos");

            migrationBuilder.DropTable(
                name: "AdjudicacionTrabajoObra");

            migrationBuilder.DropTable(
                name: "Obra");

            migrationBuilder.DropTable(
                name: "Subcontrata");

            migrationBuilder.DropTable(
                name: "EstadoObra");

            migrationBuilder.DropIndex(
                name: "IX_Empleado_IdIntegracionExternos",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "IdIntegracionExternos",
                table: "Empleado");
        }
    }
}
