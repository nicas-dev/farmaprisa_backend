using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class CreandoBaseDeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aseguradoras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contacto_email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contacto_telefono = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_activa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabla para gestionar las aseguradoras, contendrá la lista de todas las compañías de seguros con las que la farmacia tiene convenio")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    IdBrand = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.IdBrand);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    categoria_padre_id = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "categorias_ibfk_1",
                        column: x => x.categoria_padre_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Para organizar productos. El campo 'categoria_padre_id' nos permite crear subcategorías.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "DetailType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailType", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "divisiones_geograficas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_division = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    division_padre_id = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "divisiones_geograficas_ibfk_1",
                        column: x => x.division_padre_id,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para las divisiones geográficas y referirnos a los continentes, países,  provincias, departamentos, ciudades, barrios, etc. según convenga")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "PlantillaProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Precio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillaProductos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "promociones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_descuento = table.Column<string>(type: "enum('porcentaje','fijo')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    valor_descuento = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    codigo_cupon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime", nullable: false),
                    esta_activa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabla principal para definir cada promoción")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    logo_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreContacto = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailContacto = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoContacto = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificacionFiscal = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabla para los proveedores o laboratorios.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabla para los roles")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "secciones_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identificador_unico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabla para definir las ubicaciones o secciones de media en el sitio")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "sintomas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabla para almacenar todos los posibles síntomas.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "tipo_detalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identificador_unico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "sucursales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion_completa = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad_id = table.Column<int>(type: "int(11)", nullable: false),
                    latitud = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: true),
                    longitud = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: true),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_activa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "sucursales_ibfk_1",
                        column: x => x.ciudad_id,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id");
                },
                comment: "Tabla para gestionar las sucursales")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "tasas_impuestos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    porcentaje = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    pais_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "tasas_impuestos_ibfk_1",
                        column: x => x.pais_id,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id");
                },
                comment: "Tabla para la gestión de los impuestos según el país")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "promocion_categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    promocion_id = table.Column<int>(type: "int(11)", nullable: false),
                    categoria_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "promocion_categorias_ibfk_1",
                        column: x => x.promocion_id,
                        principalTable: "promociones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "promocion_categorias_ibfk_2",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para aplicar una promoción a categorías enteras")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_producto = table.Column<string>(type: "enum('fisico','servicio')", nullable: false, defaultValueSql: "'fisico'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    precio_anterior = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    sku = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadMedida = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requiere_receta = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    puntos_para_canje = table.Column<int>(type: "int(11)", nullable: true),
                    categoria_id = table.Column<int>(type: "int(11)", nullable: true),
                    proveedor_id = table.Column<int>(type: "int(11)", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EstaActivo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "productos_ibfk_1",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "productos_ibfk_2",
                        column: x => x.proveedor_id,
                        principalTable: "proveedores",
                        principalColumn: "id");
                },
                comment: "La tabla principal del catálogo de productos.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BarCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int(11)", nullable: false),
                    SupplierId = table.Column<int>(type: "int(11)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Language = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.IdProduct);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "IdBrand",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_categorias_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_proveedores_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "proveedores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre_completo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_activo = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    proveedor_id = table.Column<int>(type: "int(11)", nullable: true),
                    pais_id = table.Column<int>(type: "int(11)", nullable: false),
                    puntos_fidelidad = table.Column<int>(type: "int(11)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "usuarios_ibfk_1",
                        column: x => x.proveedor_id,
                        principalTable: "proveedores",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "usuarios_ibfk_2",
                        column: x => x.pais_id,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id");
                },
                comment: "Tabla para los usuarios.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "items_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    seccion_id = table.Column<int>(type: "int(11)", nullable: false),
                    titulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    media_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_media = table.Column<string>(type: "enum('imagen','video')", nullable: false, defaultValueSql: "'imagen'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    url_destino = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orden = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    esta_activo = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "items_media_ibfk_1",
                        column: x => x.seccion_id,
                        principalTable: "secciones_media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Para poder guardar los elementos de los archivos medias en las secciones")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "horarios_sucursal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sucursal_id = table.Column<int>(type: "int(11)", nullable: false),
                    dia_semana = table.Column<string>(type: "enum('lunes','martes','miercoles','jueves','viernes','sabado','domingo')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hora_apertura = table.Column<TimeOnly>(type: "time", nullable: false),
                    hora_cierre = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "horarios_sucursal_ibfk_1",
                        column: x => x.sucursal_id,
                        principalTable: "sucursales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para los horarios de las sucursal")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "zonas_domicilio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    costo_envio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    sucursal_id = table.Column<int>(type: "int(11)", nullable: false),
                    esta_activa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "zonas_domicilio_ibfk_1",
                        column: x => x.sucursal_id,
                        principalTable: "sucursales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para gestión de las zonas de domicilio")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "inventario_sucursal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true),
                    sucursal_id = table.Column<int>(type: "int(11)", nullable: false),
                    stock = table.Column<int>(type: "int(11)", nullable: false),
                    stock_minimo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "inventario_sucursal_ibfk_1",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "inventario_sucursal_ibfk_2",
                        column: x => x.sucursal_id,
                        principalTable: "sucursales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para el inventario por sucursal")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "producto_detalle",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true),
                    tipo_detalle_id = table.Column<int>(type: "int(11)", nullable: false),
                    valor_detalle = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    media_url = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    idioma = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto_detalle", x => x.id);
                    table.ForeignKey(
                        name: "producto_detalle_ibfk_1",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "producto_detalle_ibfk_2",
                        column: x => x.tipo_detalle_id,
                        principalTable: "tipo_detalles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "producto_imagenes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true),
                    url_imagen = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orden = table.Column<int>(type: "int(11)", nullable: false),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "producto_imagenes_ibfk_1",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "producto_sintomas",
                columns: table => new
                {
                    producto_id = table.Column<int>(type: "int(11)", nullable: false),
                    sintoma_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto_sintomas", x => new { x.producto_id, x.sintoma_id });
                    table.ForeignKey(
                        name: "producto_sintomas_ibfk_1",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "producto_sintomas_ibfk_2",
                        column: x => x.sintoma_id,
                        principalTable: "sintomas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para conectar los productos con sus síntomas.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "promocion_productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    promocion_id = table.Column<int>(type: "int(11)", nullable: false),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "promocion_productos_ibfk_1",
                        column: x => x.promocion_id,
                        principalTable: "promociones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "promocion_productos_ibfk_2",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                },
                comment: "Tabla para aplicar una promoción a productos específicos")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    IdProductDetail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DetailTypeId = table.Column<int>(type: "int", nullable: false),
                    DetailText = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.IdProductDetail);
                    table.ForeignKey(
                        name: "FK_ProductDetails_DetailType_DetailTypeId",
                        column: x => x.DetailTypeId,
                        principalTable: "DetailType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "carrito_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: false),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true),
                    cantidad = table.Column<int>(type: "int(11)", nullable: false),
                    fecha_agregado = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "carrito_items_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "carrito_items_ibfk_2",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                },
                comment: "Tabla para el carrito de las compras\r\nA esta tabla se le hace un DELETE limpio cada que se hace una compra, porque son datos temporales y se usa como una mesa de trabajo donde el cliente coloca los productos y luego se deja limpio. Así se evita que se acumule basura y se hagan más lentas las consultas de los productos del carrito de compras por usuario")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "direcciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: true),
                    direccion_completa = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad_id = table.Column<int>(type: "int(11)", nullable: false),
                    referencia = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitud = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: true),
                    longitud = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: true),
                    es_predeterminada = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "direcciones_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "direcciones_ibfk_2",
                        column: x => x.ciudad_id,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id");
                },
                comment: "Tabla para que los usuarios guarden múltiples direcciones de envío.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "favoritos_usuario",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int(11)", nullable: false),
                    producto_id = table.Column<int>(type: "int(11)", nullable: false),
                    fecha_agregado = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.usuario_id, x.producto_id })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "favoritos_usuario_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "favoritos_usuario_ibfk_2",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                },
                comment: "Tabla para la gestión de los productos favoritos de los usuarios")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "metodos_pago_usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: true),
                    gateway_token = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_tarjeta = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ultimos_cuatro_digitos = table.Column<string>(type: "char(4)", fixedLength: true, maxLength: 4, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mes_expiracion = table.Column<int>(type: "int(11)", nullable: true),
                    ano_expiracion = table.Column<int>(type: "int(11)", nullable: true),
                    es_predeterminado = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    fecha_agregado = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "metodos_pago_usuario_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                },
                comment: "Tabla para la gestión de los métodos de pagos con")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "opiniones_productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: false),
                    calificacion = table.Column<int>(type: "int(11)", nullable: false),
                    comentario = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_opinion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "opiniones_productos_ibfk_1",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "opiniones_productos_ibfk_2",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                },
                comment: "Tabla de Opiniones y Valoraciones")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "otps_usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: false),
                    codigo_otp = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_uso = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_expiracion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    esta_usado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otps_usuario", x => x.id);
                    table.ForeignKey(
                        name: "otps_usuario_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "paginas_informativas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    titulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    slug = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contenido_html = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_publicada = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()"),
                    autor_id = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "paginas_informativas_ibfk_1",
                        column: x => x.autor_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                },
                comment: "Páginas con información estática, con esta tabla, el administrador podrá crear y editar fácilmente el contenido de estas páginas desde el panel de control.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "recetas_medicas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: true),
                    imagen_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "enum('pendiente','aprobada','rechazada','usada')", nullable: false, defaultValueSql: "'pendiente'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comentarios_admin = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_carga = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "recetas_medicas_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                },
                comment: "Tabla para que los usuarios suban sus recetas")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "usuario_aseguradoras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: true),
                    aseguradora_id = table.Column<int>(type: "int(11)", nullable: false),
                    numero_poliza = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    esta_verificada = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    fecha_agregado = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "usuario_aseguradoras_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "usuario_aseguradoras_ibfk_2",
                        column: x => x.aseguradora_id,
                        principalTable: "aseguradoras",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para Vincular Usuarios con Aseguradoras  conecta a cada usuario con su respectiva aseguradora y guarda su número de póliza. Esto permite que un usuario pueda tener pólizas con una o más aseguradoras.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "usuario_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: true),
                    rol_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "usuario_roles_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "usuario_roles_ibfk_2",
                        column: x => x.rol_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para la relación de los Usuarios con los Roles")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "horarios_domicilio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    zona_id = table.Column<int>(type: "int(11)", nullable: false),
                    dia_semana = table.Column<string>(type: "enum('lunes','martes','miercoles','jueves','viernes','sabado','domingo')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hora_inicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    hora_cierre = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "horarios_domicilio_ibfk_1",
                        column: x => x.zona_id,
                        principalTable: "zonas_domicilio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tabla para gestión de horarios de los domicilio")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int(11)", nullable: true),
                    tipo_entrega = table.Column<string>(type: "enum('domicilio','recoger_en_tienda')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion_id = table.Column<int>(type: "int(11)", nullable: true),
                    sucursal_recogida_id = table.Column<int>(type: "int(11)", nullable: true),
                    total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    estado = table.Column<string>(type: "enum('pendiente','procesando','listo_para_recoger','en_camino','entregado','cancelado')", nullable: false, defaultValueSql: "'pendiente'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    receta_id = table.Column<int>(type: "int(11)", nullable: true),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    costo_envio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    monto_descuento = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    monto_impuesto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    fecha_pedido = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "pedidos_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "pedidos_ibfk_2",
                        column: x => x.direccion_id,
                        principalTable: "direcciones",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "pedidos_ibfk_3",
                        column: x => x.sucursal_recogida_id,
                        principalTable: "sucursales",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "pedidos_ibfk_4",
                        column: x => x.receta_id,
                        principalTable: "recetas_medicas",
                        principalColumn: "id");
                },
                comment: "Tabla para guardar los pedidos de los clientes.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "detalles_pedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pedido_id = table.Column<int>(type: "int(11)", nullable: false),
                    producto_id = table.Column<int>(type: "int(11)", nullable: true),
                    cantidad = table.Column<int>(type: "int(11)", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "detalles_pedido_ibfk_1",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "detalles_pedido_ibfk_2",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id");
                },
                comment: "Tabla de cruce para saber qué productos van en cada pedido.")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "transacciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pedido_id = table.Column<int>(type: "int(11)", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    pasarela_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_transaccion_pasarela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "enum('exitoso','fallido')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_transaccion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "transacciones_ibfk_1",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "id");
                },
                comment: "Tabla para guardar un registro del pago")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "nombre",
                table: "aseguradoras",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "producto_id",
                table: "carrito_items",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id",
                table: "carrito_items",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "categoria_padre_id",
                table: "categorias",
                column: "categoria_padre_id");

            migrationBuilder.CreateIndex(
                name: "pedido_id",
                table: "detalles_pedido",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "producto_id1",
                table: "detalles_pedido",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ciudad_id",
                table: "direcciones",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id1",
                table: "direcciones",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "division_padre_id",
                table: "divisiones_geograficas",
                column: "division_padre_id");

            migrationBuilder.CreateIndex(
                name: "producto_id2",
                table: "favoritos_usuario",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "zona_id",
                table: "horarios_domicilio",
                column: "zona_id");

            migrationBuilder.CreateIndex(
                name: "sucursal_id",
                table: "horarios_sucursal",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "producto_id3",
                table: "inventario_sucursal",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "sucursal_id1",
                table: "inventario_sucursal",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "seccion_id",
                table: "items_media",
                column: "seccion_id");

            migrationBuilder.CreateIndex(
                name: "gateway_token",
                table: "metodos_pago_usuario",
                column: "gateway_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "usuario_id2",
                table: "metodos_pago_usuario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "producto_id4",
                table: "opiniones_productos",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id3",
                table: "opiniones_productos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_otps_usuario_usuario_id",
                table: "otps_usuario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "autor_id",
                table: "paginas_informativas",
                column: "autor_id");

            migrationBuilder.CreateIndex(
                name: "slug",
                table: "paginas_informativas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "direccion_id",
                table: "pedidos",
                column: "direccion_id");

            migrationBuilder.CreateIndex(
                name: "receta_id",
                table: "pedidos",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "sucursal_recogida_id",
                table: "pedidos",
                column: "sucursal_recogida_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id4",
                table: "pedidos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_DetailTypeId",
                table: "ProductDetails",
                column: "DetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_producto_detalle_producto_id",
                table: "producto_detalle",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_producto_detalle_tipo_detalle_id",
                table: "producto_detalle",
                column: "tipo_detalle_id");

            migrationBuilder.CreateIndex(
                name: "IX_producto_imagenes_producto_id",
                table: "producto_imagenes",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "producto_id5",
                table: "producto_sintomas",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "sintoma_id",
                table: "producto_sintomas",
                column: "sintoma_id");

            migrationBuilder.CreateIndex(
                name: "categoria_id",
                table: "productos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_productos_sku",
                table: "productos",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "proveedor_id",
                table: "productos",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "sku",
                table: "productos",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "categoria_id1",
                table: "promocion_categorias",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "promocion_id",
                table: "promocion_categorias",
                column: "promocion_id");

            migrationBuilder.CreateIndex(
                name: "producto_id6",
                table: "promocion_productos",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "promocion_id1",
                table: "promocion_productos",
                column: "promocion_id");

            migrationBuilder.CreateIndex(
                name: "codigo_cupon",
                table: "promociones",
                column: "codigo_cupon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "nombre1",
                table: "proveedores",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "usuario_id5",
                table: "recetas_medicas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "nombre2",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "identificador_unico",
                table: "secciones_media",
                column: "identificador_unico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "nombre3",
                table: "sintomas",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ciudad_id1",
                table: "sucursales",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "pais_id",
                table: "tasas_impuestos",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "IX_tipo_detalles_identificador_unico",
                table: "tipo_detalles",
                column: "identificador_unico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "pedido_id1",
                table: "transacciones",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "aseguradora_id",
                table: "usuario_aseguradoras",
                column: "aseguradora_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id6",
                table: "usuario_aseguradoras",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "rol_id",
                table: "usuario_roles",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "uq_usuario_rol",
                table: "usuario_roles",
                columns: new[] { "usuario_id", "rol_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "pais_id1",
                table: "usuarios",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "proveedor_id1",
                table: "usuarios",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "telefono",
                table: "usuarios",
                column: "telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sucursal_id2",
                table: "zonas_domicilio",
                column: "sucursal_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carrito_items");

            migrationBuilder.DropTable(
                name: "detalles_pedido");

            migrationBuilder.DropTable(
                name: "favoritos_usuario");

            migrationBuilder.DropTable(
                name: "horarios_domicilio");

            migrationBuilder.DropTable(
                name: "horarios_sucursal");

            migrationBuilder.DropTable(
                name: "inventario_sucursal");

            migrationBuilder.DropTable(
                name: "items_media");

            migrationBuilder.DropTable(
                name: "metodos_pago_usuario");

            migrationBuilder.DropTable(
                name: "opiniones_productos");

            migrationBuilder.DropTable(
                name: "otps_usuario");

            migrationBuilder.DropTable(
                name: "paginas_informativas");

            migrationBuilder.DropTable(
                name: "PlantillaProductos");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "producto_detalle");

            migrationBuilder.DropTable(
                name: "producto_imagenes");

            migrationBuilder.DropTable(
                name: "producto_sintomas");

            migrationBuilder.DropTable(
                name: "promocion_categorias");

            migrationBuilder.DropTable(
                name: "promocion_productos");

            migrationBuilder.DropTable(
                name: "tasas_impuestos");

            migrationBuilder.DropTable(
                name: "transacciones");

            migrationBuilder.DropTable(
                name: "usuario_aseguradoras");

            migrationBuilder.DropTable(
                name: "usuario_roles");

            migrationBuilder.DropTable(
                name: "zonas_domicilio");

            migrationBuilder.DropTable(
                name: "secciones_media");

            migrationBuilder.DropTable(
                name: "DetailType");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "tipo_detalles");

            migrationBuilder.DropTable(
                name: "sintomas");

            migrationBuilder.DropTable(
                name: "promociones");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "pedidos");

            migrationBuilder.DropTable(
                name: "aseguradoras");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "direcciones");

            migrationBuilder.DropTable(
                name: "sucursales");

            migrationBuilder.DropTable(
                name: "recetas_medicas");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "divisiones_geograficas");
        }
    }
}
