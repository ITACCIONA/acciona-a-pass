using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccionaCovid.Data.Migrations
{
    public partial class FiltradoPRL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdArea",
                table: "Localizacion",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTecnologia",
                table: "FichaLaboral",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tecnologia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnologia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmbitoAccesoEmpleadoPais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEmpleado = table.Column<int>(nullable: false),
                    IdPais = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmbitoAccesoEmpleadoPais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmbitoAccesoEmpleadoPais_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AmbitoAccesoEmpleadoPais_Pais",
                        column: x => x.IdPais,
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    IdPais = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_Pais",
                        column: x => x.IdPais,
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AmbitoAccesoEmpleadoRegion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEmpleado = table.Column<int>(nullable: false),
                    IdRegion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmbitoAccesoEmpleadoRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmbitoAccesoEmpleadoRegion_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AmbitoAccesoEmpleadoRegion_Region",
                        column: x => x.IdRegion,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    IdRegion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_Region",
                        column: x => x.IdRegion,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AmbitoAccesoEmpleadoArea",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    LastAction = table.Column<string>(maxLength: 10, nullable: false, defaultValueSql: "(N'CREATE')"),
                    LastActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUser = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IdEmpleado = table.Column<int>(nullable: false),
                    IdArea = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmbitoAccesoEmpleadoArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmbitoAccesoEmpleadoPais_Area",
                        column: x => x.IdArea,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AmbitoAccesoEmpleadoArea_Empleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Localizacion_IdArea",
                table: "Localizacion",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_FichaLaboral_IdTecnologia",
                table: "FichaLaboral",
                column: "IdTecnologia");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitoAccesoEmpleadoArea_IdArea",
                table: "AmbitoAccesoEmpleadoArea",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitoAccesoEmpleadoArea_IdEmpleado",
                table: "AmbitoAccesoEmpleadoArea",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitoAccesoEmpleadoPais_IdEmpleado",
                table: "AmbitoAccesoEmpleadoPais",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitoAccesoEmpleadoPais_IdPais",
                table: "AmbitoAccesoEmpleadoPais",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitoAccesoEmpleadoRegion_IdEmpleado",
                table: "AmbitoAccesoEmpleadoRegion",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitoAccesoEmpleadoRegion_IdRegion",
                table: "AmbitoAccesoEmpleadoRegion",
                column: "IdRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Area_IdRegion",
                table: "Area",
                column: "IdRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Area_Nombre",
                table: "Area",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pais_Nombre",
                table: "Pais",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Region_IdPais",
                table: "Region",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_Region_Nombre",
                table: "Region",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tecnologia_Nombre",
                table: "Tecnologia",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FichaLaboral_Tecnologia",
                table: "FichaLaboral",
                column: "IdTecnologia",
                principalTable: "Tecnologia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Localizacion_Area",
                table: "Localizacion",
                column: "IdArea",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichaLaboral_Tecnologia",
                table: "FichaLaboral");

            migrationBuilder.DropForeignKey(
                name: "FK_Localizacion_Area",
                table: "Localizacion");

            migrationBuilder.DropTable(
                name: "AmbitoAccesoEmpleadoArea");

            migrationBuilder.DropTable(
                name: "AmbitoAccesoEmpleadoPais");

            migrationBuilder.DropTable(
                name: "AmbitoAccesoEmpleadoRegion");

            migrationBuilder.DropTable(
                name: "Tecnologia");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Pais");

            migrationBuilder.DropIndex(
                name: "IX_Localizacion_IdArea",
                table: "Localizacion");

            migrationBuilder.DropIndex(
                name: "IX_FichaLaboral_IdTecnologia",
                table: "FichaLaboral");

            migrationBuilder.DropColumn(
                name: "IdArea",
                table: "Localizacion");

            migrationBuilder.DropColumn(
                name: "IdTecnologia",
                table: "FichaLaboral");
        }
    }
}
