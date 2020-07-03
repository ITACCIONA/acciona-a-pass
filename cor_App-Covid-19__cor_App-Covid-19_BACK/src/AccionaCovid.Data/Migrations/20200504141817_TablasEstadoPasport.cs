using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class TablasEstadoPasport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DiasValidez",
                table: "EstadoPasaporte",
                nullable: true,
                defaultValueSql: "((1))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AddColumn<int>(
                name: "IdColorEstado",
                table: "EstadoPasaporte",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTipoEstado",
                table: "EstadoPasaporte",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PCRAnt",
                table: "EstadoPasaporte",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PCRUlt",
                table: "EstadoPasaporte",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TestInmune",
                table: "EstadoPasaporte",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ColorEstado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorEstado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEstado",
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
                    Prioridad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEstado", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadoPasaporte_IdColorEstado",
                table: "EstadoPasaporte",
                column: "IdColorEstado");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoPasaporte_IdTipoEstado",
                table: "EstadoPasaporte",
                column: "IdTipoEstado");

            migrationBuilder.AddForeignKey(
                name: "FK_EstadoPasaporte_ColorEstado",
                table: "EstadoPasaporte",
                column: "IdColorEstado",
                principalTable: "ColorEstado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadoPasaporte_TipoEstado",
                table: "EstadoPasaporte",
                column: "IdTipoEstado",
                principalTable: "TipoEstado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstadoPasaporte_ColorEstado",
                table: "EstadoPasaporte");

            migrationBuilder.DropForeignKey(
                name: "FK_EstadoPasaporte_TipoEstado",
                table: "EstadoPasaporte");

            migrationBuilder.DropTable(
                name: "ColorEstado");

            migrationBuilder.DropTable(
                name: "TipoEstado");

            migrationBuilder.DropIndex(
                name: "IX_EstadoPasaporte_IdColorEstado",
                table: "EstadoPasaporte");

            migrationBuilder.DropIndex(
                name: "IX_EstadoPasaporte_IdTipoEstado",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "IdColorEstado",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "IdTipoEstado",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "PCRAnt",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "PCRUlt",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "TestInmune",
                table: "EstadoPasaporte");

            migrationBuilder.AlterColumn<int>(
                name: "DiasValidez",
                table: "EstadoPasaporte",
                type: "int",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValueSql: "((1))");
        }
    }
}
