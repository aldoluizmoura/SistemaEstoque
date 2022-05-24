using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstoque.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabricante_Documento_FabricanteId",
                table: "Fabricante");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Documento_UsuarioId",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Usuario",
                newName: "DocumentoId");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_UsuarioId",
                table: "Usuario",
                newName: "IX_Usuario_DocumentoId");

            migrationBuilder.RenameColumn(
                name: "FabricanteId",
                table: "Fabricante",
                newName: "DocumentoId");

            migrationBuilder.RenameIndex(
                name: "IX_Fabricante_FabricanteId",
                table: "Fabricante",
                newName: "IX_Fabricante_DocumentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabricante_Documento_DocumentoId",
                table: "Fabricante",
                column: "DocumentoId",
                principalTable: "Documento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Documento_DocumentoId",
                table: "Usuario",
                column: "DocumentoId",
                principalTable: "Documento",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabricante_Documento_DocumentoId",
                table: "Fabricante");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Documento_DocumentoId",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "DocumentoId",
                table: "Usuario",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_DocumentoId",
                table: "Usuario",
                newName: "IX_Usuario_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "DocumentoId",
                table: "Fabricante",
                newName: "FabricanteId");

            migrationBuilder.RenameIndex(
                name: "IX_Fabricante_DocumentoId",
                table: "Fabricante",
                newName: "IX_Fabricante_FabricanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabricante_Documento_FabricanteId",
                table: "Fabricante",
                column: "FabricanteId",
                principalTable: "Documento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Documento_UsuarioId",
                table: "Usuario",
                column: "UsuarioId",
                principalTable: "Documento",
                principalColumn: "Id");
        }
    }
}
