using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroVisitantes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCedulaFromVisitante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "Visitantes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "Visitantes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

