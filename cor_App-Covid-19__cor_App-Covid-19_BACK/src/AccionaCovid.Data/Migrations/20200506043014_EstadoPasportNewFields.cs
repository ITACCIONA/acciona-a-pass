using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class EstadoPasportNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestInmune",
                table: "EstadoPasaporte");

            migrationBuilder.AddColumn<bool>(
                name: "Comment",
                table: "EstadoPasaporte",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TestInmuneIgG",
                table: "EstadoPasaporte",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TestInmuneIgM",
                table: "EstadoPasaporte",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "TestInmuneIgG",
                table: "EstadoPasaporte");

            migrationBuilder.DropColumn(
                name: "TestInmuneIgM",
                table: "EstadoPasaporte");

            migrationBuilder.AddColumn<bool>(
                name: "TestInmune",
                table: "EstadoPasaporte",
                type: "bit",
                nullable: true);
        }
    }
}
