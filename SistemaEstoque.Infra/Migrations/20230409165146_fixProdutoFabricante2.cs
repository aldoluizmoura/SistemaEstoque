using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstoque.Infra.Migrations
{
    public partial class fixProdutoFabricante2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabricante_Produto_ProdutoId",
                table: "Fabricante");

            migrationBuilder.DropIndex(
                name: "IX_Fabricante_ProdutoId",
                table: "Fabricante");

            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "Fabricante");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_FabricanteId",
                table: "Produto",
                column: "FabricanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Fabricante_FabricanteId",
                table: "Produto",
                column: "FabricanteId",
                principalTable: "Fabricante",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Fabricante_FabricanteId",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_FabricanteId",
                table: "Produto");

            migrationBuilder.AddColumn<Guid>(
                name: "ProdutoId",
                table: "Fabricante",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Fabricante_ProdutoId",
                table: "Fabricante",
                column: "ProdutoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fabricante_Produto_ProdutoId",
                table: "Fabricante",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id");
        }
    }
}
