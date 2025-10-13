using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class AddInventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armazem_Endereco_enderecoid",
                table: "Armazem");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Pessoas_fornecedorId",
                table: "Produto");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Usuario",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Endereco",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "enderecoid",
                table: "Armazem",
                newName: "enderecoId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Armazem",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Armazem_enderecoid",
                table: "Armazem",
                newName: "IX_Armazem_enderecoId");

            migrationBuilder.CreateTable(
                name: "Cadastro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    telefone = table.Column<string>(type: "TEXT", nullable: false),
                    cpf = table.Column<string>(type: "TEXT", nullable: true),
                    cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    idEndereco = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadastro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    ArmazemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PosicaoCodigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => new { x.ArmazemId, x.PosicaoCodigo, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_Inventario_Armazem_ArmazemId",
                        column: x => x.ArmazemId,
                        principalTable: "Armazem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventario_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ArmazemId_PosicaoCodigo",
                table: "Inventario",
                columns: new[] { "ArmazemId", "PosicaoCodigo" });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ProdutoId",
                table: "Inventario",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armazem_Endereco_enderecoId",
                table: "Armazem",
                column: "enderecoId",
                principalTable: "Endereco",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Cadastro_fornecedorId",
                table: "Produto",
                column: "fornecedorId",
                principalTable: "Cadastro",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armazem_Endereco_enderecoId",
                table: "Armazem");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Cadastro_fornecedorId",
                table: "Produto");

            migrationBuilder.DropTable(
                name: "Cadastro");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Usuario",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Endereco",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "enderecoId",
                table: "Armazem",
                newName: "enderecoid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Armazem",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Armazem_enderecoId",
                table: "Armazem",
                newName: "IX_Armazem_enderecoid");

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    enderecoid = table.Column<int>(type: "INTEGER", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    telefone = table.Column<string>(type: "TEXT", nullable: false),
                    cpf = table.Column<string>(type: "TEXT", nullable: true),
                    cnpj = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoas_Endereco_enderecoid",
                        column: x => x.enderecoid,
                        principalTable: "Endereco",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_enderecoid",
                table: "Pessoas",
                column: "enderecoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Armazem_Endereco_enderecoid",
                table: "Armazem",
                column: "enderecoid",
                principalTable: "Endereco",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Pessoas_fornecedorId",
                table: "Produto",
                column: "fornecedorId",
                principalTable: "Pessoas",
                principalColumn: "Id");
        }
    }
}
