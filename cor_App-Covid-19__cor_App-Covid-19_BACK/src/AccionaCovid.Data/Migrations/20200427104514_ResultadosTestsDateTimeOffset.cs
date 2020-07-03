using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class ResultadosTestsDateTimeOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FechaTest",
                table: "ResultadoTestPCR",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FechaTest",
                table: "ResultadoTestMedico",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaTest",
                table: "ResultadoTestPCR",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaTest",
                table: "ResultadoTestMedico",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldDefaultValueSql: "(getdate())");
        }
    }
}
