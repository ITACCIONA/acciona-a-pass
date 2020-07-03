using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class PrioridadColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "ColorEstado",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ColorEstado",
                keyColumn: "Nombre",
                keyValues: new object[]{ "Gris", "Verde" },
                column: "Prioridad",
                values: new object[]{ 20, 10 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "ColorEstado");
        }
    }
}
