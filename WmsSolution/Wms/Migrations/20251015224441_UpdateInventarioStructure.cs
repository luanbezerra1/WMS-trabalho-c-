using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInventarioStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Inventario_PosicaoID",
                table: "Inventario");

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "Inventario",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Inventario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "NomePosicao",
                table: "Inventario",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ArmazemId_PosicaoID",
                table: "Inventario",
                columns: new[] { "ArmazemId", "PosicaoID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Inventario_ArmazemId_PosicaoID",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "NomePosicao",
                table: "Inventario");

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "Inventario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventario",
                table: "Inventario",
                columns: new[] { "ArmazemId", "ProdutoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_PosicaoID",
                table: "Inventario",
                column: "PosicaoID");
        }
    }
}
