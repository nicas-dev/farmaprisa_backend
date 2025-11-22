using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class Cambiosenbackendseeliminaronentidadesyconfiguracionesdesamir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritosUsuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritosUsuarios",
                columns: table => new
                {
                    IdFavorito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int(11)", nullable: true),
                    FechaAgregado = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritosUsuarios", x => x.IdFavorito);
                    table.ForeignKey(
                        name: "FK_FavoritosUsuarios_Products_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                    table.ForeignKey(
                        name: "FK_FavoritosUsuarios_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritosUsuarios_ProductoId",
                table: "FavoritosUsuarios",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritosUsuarios_UsuarioId",
                table: "FavoritosUsuarios",
                column: "UsuarioId");
        }
    }
}
