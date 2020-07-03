using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class NuevoCampoGrupoResEncuSint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GrupoRespuestas",
                table: "ResultadoEncuestaSintomas",
                nullable: true);
            migrationBuilder.Sql(@"
DECLARE @GruposRes TABLE(FechaFiltro DATETIME, NuevoGuid uniqueidentifier)
INSERT INTO @GruposRes SELECT DISTINCT RESU.LastActionDate as FECHAFILTRO, NEWID() as NUEVOGUID 
						FROM ResultadoEncuestaSintomas RESU
UPDATE ResultadoEncuestaSintomas SET GrupoRespuestas = V1.NUEVOGUID 
FROM @GruposRes AS V1 
WHERE LastActionDate = V1.FECHAFILTRO AND GrupoRespuestas IS NULL
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrupoRespuestas",
                table: "ResultadoEncuestaSintomas");
        }
    }
}
