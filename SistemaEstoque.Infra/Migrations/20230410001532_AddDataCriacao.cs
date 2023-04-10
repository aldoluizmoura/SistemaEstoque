using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstoque.Infra.Migrations
{
    public partial class AddDataCriacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCadastro",
                table: "Produto",
                newName: "DataCriacao");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Usuario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "FabricanteId",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Fabricante",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Fabricante",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Fabricante",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Endereco",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Documento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Categoria",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Fabricante_UsuarioId",
                table: "Fabricante",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabricante_Usuario_UsuarioId",
                table: "Fabricante",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabricante_Usuario_UsuarioId",
                table: "Fabricante");

            migrationBuilder.DropIndex(
                name: "IX_Fabricante_UsuarioId",
                table: "Fabricante");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "FabricanteId",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Fabricante");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Fabricante");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Fabricante");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Endereco");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Categoria");

            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Produto",
                newName: "DataCadastro");
        }
    }
}
