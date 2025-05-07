using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Celular",
                table: "Usuarios",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Usuarios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Usuarios_Documento",
                table: "Usuarios",
                column: "Documento");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_EnfermeroDocumento",
                table: "Citas",
                column: "EnfermeroDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteDocumento",
                table: "Citas",
                column: "PacienteDocumento");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Usuarios_EnfermeroDocumento",
                table: "Citas",
                column: "EnfermeroDocumento",
                principalTable: "Usuarios",
                principalColumn: "Documento",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Usuarios_PacienteDocumento",
                table: "Citas",
                column: "PacienteDocumento",
                principalTable: "Usuarios",
                principalColumn: "Documento",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Usuarios_EnfermeroDocumento",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Usuarios_PacienteDocumento",
                table: "Citas");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Usuarios_Documento",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Citas_EnfermeroDocumento",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_PacienteDocumento",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Celular",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Usuarios");
        }
    }
}
