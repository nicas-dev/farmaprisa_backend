using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class nd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_imagenes_productos_ProductoId1",
                table: "producto_imagenes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId1",
                table: "producto_imagenes",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");

            migrationBuilder.AddForeignKey(
                name: "FK_producto_imagenes_productos_ProductoId1",
                table: "producto_imagenes",
                column: "ProductoId1",
                principalTable: "productos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_imagenes_productos_ProductoId1",
                table: "producto_imagenes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId1",
                table: "producto_imagenes",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_producto_imagenes_productos_ProductoId1",
                table: "producto_imagenes",
                column: "ProductoId1",
                principalTable: "productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
