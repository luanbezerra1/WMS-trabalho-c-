using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleanFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rua = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Complemento = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    Cep = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nomeProduto = table.Column<string>(type: "TEXT", nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    lote = table.Column<int>(type: "INTEGER", nullable: false),
                    fornecedorId = table.Column<int>(type: "INTEGER", nullable: false),
                    preco = table.Column<double>(type: "REAL", nullable: false),
                    Categoria = table.Column<int>(type: "INTEGER", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    login = table.Column<string>(type: "TEXT", nullable: false),
                    senha = table.Column<string>(type: "TEXT", nullable: false),
                    cargo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Armazem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nomeArmazem = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    Posicoes = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoPosicao = table.Column<int>(type: "INTEGER", nullable: false),
                    Capacidade = table.Column<int>(type: "INTEGER", nullable: false),
                    EnderecoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armazem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armazem_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
                    Cpf = table.Column<string>(type: "TEXT", nullable: false),
                    EnderecoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cliente_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    telefone = table.Column<string>(type: "TEXT", nullable: false),
                    cnpj = table.Column<string>(type: "TEXT", nullable: false),
                    EnderecoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedor_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    ArmazemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PosicaoID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    UltimaMovimentacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => new { x.ArmazemId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_Inventario_Armazem_ArmazemId",
                        column: x => x.ArmazemId,
                        principalTable: "Armazem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventario_Produto_ProdutoId",
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

            migrationBuilder.CreateIndex(
                name: "IX_Armazem_EnderecoId",
                table: "Armazem",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Cpf",
                table: "Cliente",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_EnderecoId",
                table: "Cliente",
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
                name: "IX_Fornecedor_cnpj",
                table: "Fornecedor",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_EnderecoId",
                table: "Fornecedor",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_PosicaoID",
                table: "Inventario",
                column: "PosicaoID");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ProdutoId",
                table: "Inventario",
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
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "SaidaProduto");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropTable(
                name: "Armazem");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Endereco");
        }
    }
}
