using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class RemovePosicaoID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inventario_ArmazemId_PosicaoID",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "PosicaoID",
                table: "Inventario");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ArmazemId_NomePosicao",
                table: "Inventario",
                columns: new[] { "ArmazemId", "NomePosicao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inventario_ArmazemId_NomePosicao",
                table: "Inventario");

            migrationBuilder.AddColumn<int>(
                name: "PosicaoID",
                table: "Inventario",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ArmazemId_PosicaoID",
                table: "Inventario",
                columns: new[] { "ArmazemId", "PosicaoID" },
                unique: true);
        }
    }
}
