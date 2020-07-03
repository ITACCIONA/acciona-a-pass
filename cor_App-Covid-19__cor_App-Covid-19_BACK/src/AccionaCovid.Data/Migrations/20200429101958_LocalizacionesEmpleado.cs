using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class LocalizacionesEmpleado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalizacionEmpleados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEmpleado = table.Column<int>(nullable: false),
                    IdLocalizacion = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizacionEmpleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizacionEmpleados_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocalizacionEmpleados_Localizacion",
                        column: x => x.IdLocalizacion,
                        principalTable: "Localizacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizacionEmpleados_IdEmpleado",
                table: "LocalizacionEmpleados",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizacionEmpleados_IdLocalizacion",
                table: "LocalizacionEmpleados",
                column: "IdLocalizacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalizacionEmpleados");
        }
    }
}
