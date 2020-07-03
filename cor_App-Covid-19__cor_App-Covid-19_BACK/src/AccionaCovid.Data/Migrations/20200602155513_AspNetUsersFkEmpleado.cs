using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class AspNetUsersFkEmpleado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdEmpleado",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "IdEmpleado",
                keyValue: 0,
                column: "IdEmpleado",
                value: null);

            migrationBuilder.AddForeignKey(
                    name: "FK_AspNetUsers_Empleado",
                    table: "AspNetUsers",
                    column: "IdEmpleado",
                    principalTable: "Empleado",
                    principalColumn: "Id");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empleado",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "IdEmpleado",
                keyValue: null,
                column: "IdEmpleado",
                value: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdEmpleado",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
