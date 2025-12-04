using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class agregarsucursales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchIdBranch",
                table: "usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Branches",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_BranchIdBranch",
                table: "usuarios",
                column: "BranchIdBranch");

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_Branches_BranchIdBranch",
                table: "usuarios",
                column: "BranchIdBranch",
                principalTable: "Branches",
                principalColumn: "IdBranch");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_Branches_BranchIdBranch",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_BranchIdBranch",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "BranchIdBranch",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Branches");
        }
    }
}
