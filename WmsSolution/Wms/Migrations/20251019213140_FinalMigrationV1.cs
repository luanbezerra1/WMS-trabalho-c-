using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Categoria",
                table: "Produto",
                newName: "categoria");

            migrationBuilder.AlterColumn<string>(
                name: "categoria",
                table: "Produto",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "categoria",
                table: "Produto",
                newName: "Categoria");

            migrationBuilder.AlterColumn<int>(
                name: "Categoria",
                table: "Produto",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
