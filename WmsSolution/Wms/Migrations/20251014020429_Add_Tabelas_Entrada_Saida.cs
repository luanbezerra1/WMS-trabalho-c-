using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tabelas_Entrada_Saida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntradaProduto",
                columns: table => new
                {
                    EntradaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    FornecedorId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeRecebida = table.Column<int>(type: "INTEGER", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaProduto", x => new { x.EntradaId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_EntradaProduto_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntradaProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaidaProduto",
                columns: table => new
                {
                    SaidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeRetirada = table.Column<int>(type: "INTEGER", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaidaProduto", x => new { x.SaidaId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_SaidaProduto_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaidaProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProduto_FornecedorId",
                table: "EntradaProduto",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProduto_ProdutoId",
                table: "EntradaProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_SaidaProduto_ClienteId",
                table: "SaidaProduto",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_SaidaProduto_ProdutoId",
                table: "SaidaProduto",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradaProduto");

            migrationBuilder.DropTable(
                name: "SaidaProduto");
        }
    }
}
