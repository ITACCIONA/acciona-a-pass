using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class NuevosCamposCargaWorkday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Localizacion",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "Localizacion",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion1",
                table: "Localizacion",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Localizacion",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "IdWorkday",
                table: "Division",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IdWorkday",
                table: "Departamento",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Localizacion");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "Localizacion");

            migrationBuilder.DropColumn(
                name: "Direccion1",
                table: "Localizacion");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Localizacion");

            migrationBuilder.DropColumn(
                name: "IdWorkday",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "IdWorkday",
                table: "Departamento");
        }
    }
}
