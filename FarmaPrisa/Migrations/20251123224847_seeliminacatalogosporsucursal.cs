using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class seeliminacatalogosporsucursal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Branches_IdBranch",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_categorias_Branches_IdBranch",
                table: "categorias");

            migrationBuilder.RenameColumn(
                name: "ReceivedQty",
                table: "InventoryDetail",
                newName: "stock");

            migrationBuilder.RenameColumn(
                name: "IdBranch",
                table: "categorias",
                newName: "BranchIdBranch");

            migrationBuilder.RenameIndex(
                name: "IX_categorias_IdBranch",
                table: "categorias",
                newName: "IX_categorias_BranchIdBranch");

            migrationBuilder.RenameColumn(
                name: "IdBranch",
                table: "Brands",
                newName: "BranchIdBranch");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_IdBranch",
                table: "Brands",
                newName: "IX_Brands_BranchIdBranch");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "InventoryDetail",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Branches_BranchIdBranch",
                table: "Brands",
                column: "BranchIdBranch",
                principalTable: "Branches",
                principalColumn: "IdBranch",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categorias_Branches_BranchIdBranch",
                table: "categorias",
                column: "BranchIdBranch",
                principalTable: "Branches",
                principalColumn: "IdBranch",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Branches_BranchIdBranch",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_categorias_Branches_BranchIdBranch",
                table: "categorias");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "InventoryDetail");

            migrationBuilder.RenameColumn(
                name: "stock",
                table: "InventoryDetail",
                newName: "ReceivedQty");

            migrationBuilder.RenameColumn(
                name: "BranchIdBranch",
                table: "categorias",
                newName: "IdBranch");

            migrationBuilder.RenameIndex(
                name: "IX_categorias_BranchIdBranch",
                table: "categorias",
                newName: "IX_categorias_IdBranch");

            migrationBuilder.RenameColumn(
                name: "BranchIdBranch",
                table: "Brands",
                newName: "IdBranch");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_BranchIdBranch",
                table: "Brands",
                newName: "IX_Brands_IdBranch");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Branches_IdBranch",
                table: "Brands",
                column: "IdBranch",
                principalTable: "Branches",
                principalColumn: "IdBranch",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categorias_Branches_IdBranch",
                table: "categorias",
                column: "IdBranch",
                principalTable: "Branches",
                principalColumn: "IdBranch",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
