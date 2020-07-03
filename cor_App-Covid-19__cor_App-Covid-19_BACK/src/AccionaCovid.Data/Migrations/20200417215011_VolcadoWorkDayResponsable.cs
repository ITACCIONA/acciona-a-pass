using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class VolcadoWorkDayResponsable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IdResponsable",
                table: "UsuarioWorkDay",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdResponsableDirecto",
                table: "FichaLaboral",
                column: "IdResponsableDirecto");

            migrationBuilder.AddForeignKey(
                name: "FK_FichaLaboral_Empleado",
                table: "FichaLaboral",
                column: "IdResponsableDirecto",
                principalTable: "Empleado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichaLaboral_Empleado",
                table: "FichaLaboral");

            migrationBuilder.DropIndex(
                name: "IX_FichaLaboral_IdResponsableDirecto",
                table: "FichaLaboral");

            migrationBuilder.AlterColumn<string>(
                name: "IdResponsable",
                table: "UsuarioWorkDay",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
