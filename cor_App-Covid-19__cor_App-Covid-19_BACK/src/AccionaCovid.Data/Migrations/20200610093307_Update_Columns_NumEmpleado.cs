using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class Update_Columns_NumEmpleado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "NumEmpleado",
                table: "UsuarioWorkDay",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "NumEmpleado",
                table: "Empleado",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.Sql(@"UPDATE
                                        e
                                   SET
                                        e.NumEmpleado = uwd.IdWorkDay
                                   FROM
                                        Empleado e
                                        INNER JOIN UsuarioWorkDay uwd
                                            ON uwd.Id = e.IdUsuarioWorkDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE
                                        e
                                   SET
                                        e.NumEmpleado = null");

            migrationBuilder.AlterColumn<int>(
                name: "NumEmpleado",
                table: "UsuarioWorkDay",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumEmpleado",
                table: "Empleado",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
