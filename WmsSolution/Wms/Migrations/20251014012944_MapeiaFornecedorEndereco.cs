using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class MapeiaFornecedorEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armazem_Endereco_enderecoId",
                table: "Armazem");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Armazem_ArmazemId",
                table: "Inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Produto_ProdutoId",
                table: "Inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Cadastro_fornecedorId",
                table: "Produto");

            migrationBuilder.DropTable(
                name: "Cadastro");

            migrationBuilder.DropIndex(
                name: "IX_Produto_fornecedorId",
                table: "Produto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Inventario_ArmazemId_PosicaoCodigo",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Armazem_enderecoId",
                table: "Armazem");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "PosicaoCodigo",
                table: "Inventario");

            migrationBuilder.RenameColumn(
                name: "rua",
                table: "Endereco",
                newName: "Rua");

            migrationBuilder.RenameColumn(
                name: "numero",
                table: "Endereco",
                newName: "Numero");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Endereco",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "complemento",
                table: "Endereco",
                newName: "Complemento");

            migrationBuilder.RenameColumn(
                name: "cidade",
                table: "Endereco",
                newName: "Cidade");

            migrationBuilder.RenameColumn(
                name: "cep",
                table: "Endereco",
                newName: "Cep");

            migrationBuilder.RenameColumn(
                name: "bairro",
                table: "Endereco",
                newName: "Bairro");

            migrationBuilder.RenameColumn(
                name: "capacidade",
                table: "Armazem",
                newName: "Capacidade");

            migrationBuilder.AlterColumn<int>(
                name: "fornecedorId",
                table: "Produto",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ultimaMovimentacao",
                table: "Inventario",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Posicoes",
                table: "Armazem",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProdutoPosicao",
                table: "Armazem",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario",
                columns: new[] { "ArmazemId", "ProdutoId" });

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
                name: "IX_Fornecedor_cnpj",
                table: "Fornecedor",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_EnderecoId",
                table: "Fornecedor",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Armazem_ArmazemId",
                table: "Inventario",
                column: "ArmazemId",
                principalTable: "Armazem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Produto_ProdutoId",
                table: "Inventario",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Armazem_ArmazemId",
                table: "Inventario");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Produto_ProdutoId",
                table: "Inventario");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "ultimaMovimentacao",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "Posicoes",
                table: "Armazem");

            migrationBuilder.DropColumn(
                name: "ProdutoPosicao",
                table: "Armazem");

            migrationBuilder.RenameColumn(
                name: "Rua",
                table: "Endereco",
                newName: "rua");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "Endereco",
                newName: "numero");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Endereco",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Complemento",
                table: "Endereco",
                newName: "complemento");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Endereco",
                newName: "cidade");

            migrationBuilder.RenameColumn(
                name: "Cep",
                table: "Endereco",
                newName: "cep");

            migrationBuilder.RenameColumn(
                name: "Bairro",
                table: "Endereco",
                newName: "bairro");

            migrationBuilder.RenameColumn(
                name: "Capacidade",
                table: "Armazem",
                newName: "capacidade");

            migrationBuilder.AlterColumn<int>(
                name: "fornecedorId",
                table: "Produto",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "Produto",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PosicaoCodigo",
                table: "Inventario",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario",
                columns: new[] { "ArmazemId", "PosicaoCodigo", "ProdutoId" });

            migrationBuilder.CreateTable(
                name: "Cadastro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    cpf = table.Column<string>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    idEndereco = table.Column<int>(type: "INTEGER", nullable: true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    telefone = table.Column<string>(type: "TEXT", nullable: false),
                    tipo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadastro", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_fornecedorId",
                table: "Produto",
                column: "fornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ArmazemId_PosicaoCodigo",
                table: "Inventario",
                columns: new[] { "ArmazemId", "PosicaoCodigo" });

            migrationBuilder.CreateIndex(
                name: "IX_Armazem_enderecoId",
                table: "Armazem",
                column: "enderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armazem_Endereco_enderecoId",
                table: "Armazem",
                column: "enderecoId",
                principalTable: "Endereco",
                principalColumn: "Id");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Cadastro_fornecedorId",
                table: "Produto",
                column: "fornecedorId",
                principalTable: "Cadastro",
                principalColumn: "Id");
        }
    }
}
