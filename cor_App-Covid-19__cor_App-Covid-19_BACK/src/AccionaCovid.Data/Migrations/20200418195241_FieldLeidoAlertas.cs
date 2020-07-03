using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class FieldLeidoAlertas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Leido",
                table: "AlertaServiciosMedicos",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(0)))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leido",
                table: "AlertaServiciosMedicos");
        }
    }
}
