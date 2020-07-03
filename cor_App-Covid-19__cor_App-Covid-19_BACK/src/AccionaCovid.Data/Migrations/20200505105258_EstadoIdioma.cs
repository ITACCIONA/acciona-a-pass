using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class EstadoIdioma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoPasaporteIdioma",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Idioma = table.Column<string>(maxLength: 10, nullable: false),
                    IdEstadoPasaporte = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoPasaporteIdioma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstadoPasaporteIdioma_EstadoPasaporte",
                        column: x => x.IdEstadoPasaporte,
                        principalTable: "EstadoPasaporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadoPasaporteIdioma_IdEstadoPasaporte",
                table: "EstadoPasaporteIdioma",
                column: "IdEstadoPasaporte");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoPasaporteIdioma");
        }
    }
}
