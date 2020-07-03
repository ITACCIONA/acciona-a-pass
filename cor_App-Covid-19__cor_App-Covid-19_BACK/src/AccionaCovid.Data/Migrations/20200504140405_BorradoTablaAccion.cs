using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class BorradoTablaAccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pasaporte_Accion",
                table: "Pasaporte");

            migrationBuilder.DropTable(
                name: "Accion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    LastAction = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accion", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Pasaporte_Accion",
                table: "Pasaporte",
                column: "IdAccion",
                principalTable: "Accion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
