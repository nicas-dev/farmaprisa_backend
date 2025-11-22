using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class nuevosCambios2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "producto_id",
                table: "producto_imagenes",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductIdProduct",
                table: "producto_imagenes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_producto_imagenes_ProductIdProduct",
                table: "producto_imagenes",
                column: "ProductIdProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_producto_imagenes_Products_ProductIdProduct",
                table: "producto_imagenes",
                column: "ProductIdProduct",
                principalTable: "Products",
                principalColumn: "IdProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_producto_imagenes_Products_ProductIdProduct",
                table: "producto_imagenes");

            migrationBuilder.DropIndex(
                name: "IX_producto_imagenes_ProductIdProduct",
                table: "producto_imagenes");

            migrationBuilder.DropColumn(
                name: "ProductIdProduct",
                table: "producto_imagenes");

            migrationBuilder.AlterColumn<int>(
                name: "producto_id",
                table: "producto_imagenes",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");
        }
    }
}
