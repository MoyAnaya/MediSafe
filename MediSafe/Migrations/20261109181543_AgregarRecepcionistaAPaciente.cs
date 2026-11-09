using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediSafe.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRecepcionistaAPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_AspNetUsers_MedicoId",
                table: "Pacientes");

            migrationBuilder.AddColumn<string>(
                name: "RecepcionistaId",
                table: "Pacientes",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_RecepcionistaId",
                table: "Pacientes",
                column: "RecepcionistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_AspNetUsers_MedicoId",
                table: "Pacientes",
                column: "MedicoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_AspNetUsers_RecepcionistaId",
                table: "Pacientes",
                column: "RecepcionistaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_AspNetUsers_MedicoId",
                table: "Pacientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_AspNetUsers_RecepcionistaId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_RecepcionistaId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "RecepcionistaId",
                table: "Pacientes");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_AspNetUsers_MedicoId",
                table: "Pacientes",
                column: "MedicoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
