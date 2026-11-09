using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediSafe.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEspecialidadMedicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "AspNetUsers");
        }
    }
}
