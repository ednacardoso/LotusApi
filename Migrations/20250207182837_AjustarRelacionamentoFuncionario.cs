﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lotus.Migrations
{
    /// <inheritdoc />
    public partial class AjustarRelacionamentoFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Funcionarios_FuncionarioId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_FuncionarioId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "Cliente",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "Funcionario",
                table: "Agendamentos");

            migrationBuilder.AddColumn<int>(
                name: "FuncionarioNavigationId",
                table: "Agendamentos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_FuncionarioNavigationId",
                table: "Agendamentos",
                column: "FuncionarioNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Funcionarios_FuncionarioNavigationId",
                table: "Agendamentos",
                column: "FuncionarioNavigationId",
                principalTable: "Funcionarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Funcionarios_FuncionarioNavigationId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_FuncionarioNavigationId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "FuncionarioNavigationId",
                table: "Agendamentos");

            migrationBuilder.AddColumn<string>(
                name: "Cliente",
                table: "Agendamentos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Funcionario",
                table: "Agendamentos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_FuncionarioId",
                table: "Agendamentos",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Funcionarios_FuncionarioId",
                table: "Agendamentos",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
