using AccionaCovid.Crosscutting;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class ContratasAdaptacionCampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdLocalizacion",
                table: "FichaLaboral",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "FichaLaboral",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "FichaLaboral",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuarioWorkDay",
                table: "Empleado",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ManualCreator",
                table: "Empleado",
                nullable: true);

            // Comprueba antes que SÍ existan filas ya creadas en Role, para no colisionar con el Seed de Role en un arranque desde 0 de la aplicación.
            // Si insertamos directo en esta migración, al arrancar desde 0, el seed no se va a ejecutar porque espera que la tabla esté vacía.
            migrationBuilder.Sql(@"
                IF (EXISTS(SELECT 1 FROM Role))
                BEGIN
                    INSERT INTO Role (Deleted, IdUser, Nombre) VALUES (0, 0, 'GestorContratas');
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EmpleadoRole WHERE IdRole IN (SELECT Id FROM Role WHERE Nombre = 'GestorContratas')");
            migrationBuilder.DeleteData(
                table: "Role", keyColumn: "Nombre", keyValue: "GestorContratas");

            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "ManualCreator",
                table: "Empleado");

            migrationBuilder.AlterColumn<int>(
                name: "IdLocalizacion",
                table: "FichaLaboral",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "FichaLaboral",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuarioWorkDay",
                table: "Empleado",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
