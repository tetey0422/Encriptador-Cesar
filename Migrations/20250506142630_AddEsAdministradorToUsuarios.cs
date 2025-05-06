using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class AddEsAdministradorToUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsAdministrador",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsAdministrador",
                table: "Usuarios");
        }
    }
}
