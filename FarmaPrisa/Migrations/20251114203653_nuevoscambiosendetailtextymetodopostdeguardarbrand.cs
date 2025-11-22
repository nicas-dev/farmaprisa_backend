using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class nuevoscambiosendetailtextymetodopostdeguardarbrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DetailText",
                table: "ProductDetails",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Brands",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Brands",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Brands",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Brands");

            migrationBuilder.AlterColumn<string>(
                name: "DetailText",
                table: "ProductDetails",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");
        }
    }
}
