using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class eliminaciondecampodescripciondebrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brands");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brands",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
