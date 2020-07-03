using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class EliminarUniqueDeNombreArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Area_Nombre",
                table: "Area");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Area_Nombre",
                table: "Area",
                column: "Nombre",
                unique: true);
        }
    }
}
