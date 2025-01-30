using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lotus.Migrations
{
    /// <inheritdoc />
    public partial class AddAgendamentoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuncionarioId",
                table: "Agendamentos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelamento",
                table: "Agendamentos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Agendamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "MotivoCancelamento",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Agendamentos");
        }
    }
}
