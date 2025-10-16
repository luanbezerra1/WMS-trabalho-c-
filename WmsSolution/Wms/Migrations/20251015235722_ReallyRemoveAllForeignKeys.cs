using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class ReallyRemoveAllForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armazem_Endereco_EnderecoId",
                table: "Armazem");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Endereco_EnderecoId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_EntradaProduto_Fornecedor_FornecedorId",
                table: "EntradaProduto");

            migrationBuilder.DropForeignKey(
                name: "FK_EntradaProduto_Produto_ProdutoId",
                table: "EntradaProduto");

            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedor_Endereco_EnderecoId",
                table: "Fornecedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Armazem_ArmazemId",
                table: "Inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Produto_ProdutoId",
                table: "Inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_SaidaProduto_Cliente_ClienteId",
                table: "SaidaProduto");

            migrationBuilder.DropForeignKey(
                name: "FK_SaidaProduto_Produto_ProdutoId",
                table: "SaidaProduto");

            migrationBuilder.DropIndex(
                name: "IX_SaidaProduto_ClienteId",
                table: "SaidaProduto");

            migrationBuilder.DropIndex(
                name: "IX_SaidaProduto_ProdutoId",
                table: "SaidaProduto");

            migrationBuilder.DropIndex(
                name: "IX_Inventario_ProdutoId",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Fornecedor_EnderecoId",
                table: "Fornecedor");

            migrationBuilder.DropIndex(
                name: "IX_EntradaProduto_FornecedorId",
                table: "EntradaProduto");

            migrationBuilder.DropIndex(
                name: "IX_EntradaProduto_ProdutoId",
                table: "EntradaProduto");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_EnderecoId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Armazem_EnderecoId",
                table: "Armazem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SaidaProduto_ClienteId",
                table: "SaidaProduto",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_SaidaProduto_ProdutoId",
                table: "SaidaProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ProdutoId",
                table: "Inventario",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_EnderecoId",
                table: "Fornecedor",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProduto_FornecedorId",
                table: "EntradaProduto",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProduto_ProdutoId",
                table: "EntradaProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_EnderecoId",
                table: "Cliente",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Armazem_EnderecoId",
                table: "Armazem",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armazem_Endereco_EnderecoId",
                table: "Armazem",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Endereco_EnderecoId",
                table: "Cliente",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntradaProduto_Fornecedor_FornecedorId",
                table: "EntradaProduto",
                column: "FornecedorId",
                principalTable: "Fornecedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntradaProduto_Produto_ProdutoId",
                table: "EntradaProduto",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedor_Endereco_EnderecoId",
                table: "Fornecedor",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Armazem_ArmazemId",
                table: "Inventario",
                column: "ArmazemId",
                principalTable: "Armazem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Produto_ProdutoId",
                table: "Inventario",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaidaProduto_Cliente_ClienteId",
                table: "SaidaProduto",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaidaProduto_Produto_ProdutoId",
                table: "SaidaProduto",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
