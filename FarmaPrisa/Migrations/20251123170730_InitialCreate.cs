using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaPrisa.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "Currencys",
                columns: table => new
                {
                    IdCurrency = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CurrencyName = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Currencysymbol = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TestMigracion = table.Column<string>(type: "longtext", nullable: false, comment: "Campo de prueba", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencys", x => x.IdCurrency);
                })
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
                name: "Companies",
                columns: table => new
                {
                    IdCompany = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCountry = table.Column<int>(type: "int(11)", nullable: false),
                    IdCurrency = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ruc = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.IdCompany);
                    table.ForeignKey(
                        name: "FK_Companies_Currencys_IdCurrency",
                        column: x => x.IdCurrency,
                        principalTable: "Currencys",
                        principalColumn: "IdCurrency",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Companies_divisiones_geograficas_IdCountry",
                        column: x => x.IdCountry,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
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
                name: "Branches",
                columns: table => new
                {
                    IdBranch = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCompany = table.Column<int>(type: "int", nullable: false),
                    BranchName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DivisionesGeograficaId = table.Column<int>(type: "int(11)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.IdBranch);
                    table.ForeignKey(
                        name: "FK_Branches_Companies_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "Companies",
                        principalColumn: "IdCompany",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Branches_divisiones_geograficas_DivisionesGeograficaId",
                        column: x => x.DivisionesGeograficaId,
                        principalTable: "divisiones_geograficas",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "CarritoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int(11)", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarritoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarritoItems_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
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
                name: "Brands",
                columns: table => new
                {
                    IdBrand = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdBranch = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.IdBrand);
                    table.ForeignKey(
                        name: "FK_Brands_Branches_IdBranch",
                        column: x => x.IdBranch,
                        principalTable: "Branches",
                        principalColumn: "IdBranch",
                        onDelete: ReferentialAction.Cascade);
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
                    categoria_padre_id = table.Column<int>(type: "int(11)", nullable: true),
                    IdBranch = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_categorias_Branches_IdBranch",
                        column: x => x.IdBranch,
                        principalTable: "Branches",
                        principalColumn: "IdBranch",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ZonasDomicilios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CostoEnvio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SucursalId = table.Column<int>(type: "int", nullable: false),
                    EstaActiva = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonasDomicilios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZonasDomicilios_Branches_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Branches",
                        principalColumn: "IdBranch",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int(11)", nullable: true),
                    TipoEntrega = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DireccionId = table.Column<int>(type: "int(11)", nullable: true),
                    SucursalRecogidaId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Estado = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecetaId = table.Column<int>(type: "int(11)", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CostoEnvio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MontoImpuesto = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FechaPedido = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Branches_SucursalRecogidaId",
                        column: x => x.SucursalRecogidaId,
                        principalTable: "Branches",
                        principalColumn: "IdBranch");
                    table.ForeignKey(
                        name: "FK_Pedidos_direcciones_DireccionId",
                        column: x => x.DireccionId,
                        principalTable: "direcciones",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Pedidos_recetas_medicas_RecetaId",
                        column: x => x.RecetaId,
                        principalTable: "recetas_medicas",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Pedidos_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id");
                })
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
                name: "HorariosDomicilios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ZonaId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraCierre = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosDomicilios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosDomicilios_ZonasDomicilios_ZonaId",
                        column: x => x.ZonaId,
                        principalTable: "ZonasDomicilios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
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
                        principalTable: "Pedidos",
                        principalColumn: "Id");
                },
                comment: "Tabla para guardar un registro del pago")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "DetallesPedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_Products_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "InventarioSucursals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    SucursalId = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    stock_minimo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioSucursals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventarioSucursals_Branches_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Branches",
                        principalColumn: "IdBranch",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventarioSucursals_Products_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    IdInventory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    IdProvider = table.Column<int>(type: "int(11)", nullable: false),
                    IdBranch = table.Column<int>(type: "int", nullable: false),
                    DateIn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.IdInventory);
                    table.ForeignKey(
                        name: "FK_Inventories_Branches_IdBranch",
                        column: x => x.IdBranch,
                        principalTable: "Branches",
                        principalColumn: "IdBranch",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_proveedores_IdProvider",
                        column: x => x.IdProvider,
                        principalTable: "proveedores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "OpinionesProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    producto_id = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int(11)", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaOpinion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ProductoIdProduct = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpinionesProductos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpinionesProductos_Products_ProductoIdProduct",
                        column: x => x.ProductoIdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpinionesProductos_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
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
                    DetailText = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDetalleId = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ProductDetails_tipo_detalles_TipoDetalleId",
                        column: x => x.TipoDetalleId,
                        principalTable: "tipo_detalles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "producto_imagenes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    producto_id = table.Column<int>(type: "int(11)", nullable: false),
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
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "ProductoSintomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    SintomaId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoSintomas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoSintomas_Products_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                    table.ForeignKey(
                        name: "FK_ProductoSintomas_sintomas_SintomaId",
                        column: x => x.SintomaId,
                        principalTable: "sintomas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "PromocionProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PromocionId = table.Column<int>(type: "int(11)", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocionProductos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromocionProductos_Products_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Products",
                        principalColumn: "IdProduct");
                    table.ForeignKey(
                        name: "FK_PromocionProductos_promociones_PromocionId",
                        column: x => x.PromocionId,
                        principalTable: "promociones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "InventoryDetail",
                columns: table => new
                {
                    IdInventoryDetail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdInventory = table.Column<int>(type: "int", nullable: false),
                    MinQty = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MaxQty = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ReceivedQty = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDetail", x => x.IdInventoryDetail);
                    table.ForeignKey(
                        name: "FK_InventoryDetail_Inventories_IdInventory",
                        column: x => x.IdInventory,
                        principalTable: "Inventories",
                        principalColumn: "IdInventory",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "nombre",
                table: "aseguradoras",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_DivisionesGeograficaId",
                table: "Branches",
                column: "DivisionesGeograficaId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_IdCompany",
                table: "Branches",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_IdBranch",
                table: "Brands",
                column: "IdBranch");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoItems_UsuarioId",
                table: "CarritoItems",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "categoria_padre_id",
                table: "categorias",
                column: "categoria_padre_id");

            migrationBuilder.CreateIndex(
                name: "IX_categorias_IdBranch",
                table: "categorias",
                column: "IdBranch");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IdCountry",
                table: "Companies",
                column: "IdCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IdCurrency",
                table: "Companies",
                column: "IdCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_PedidoId",
                table: "DetallesPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_ProductoId",
                table: "DetallesPedidos",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "ciudad_id",
                table: "direcciones",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id",
                table: "direcciones",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "division_padre_id",
                table: "divisiones_geograficas",
                column: "division_padre_id");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosDomicilios_ZonaId",
                table: "HorariosDomicilios",
                column: "ZonaId");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioSucursals_ProductoId",
                table: "InventarioSucursals",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioSucursals_SucursalId",
                table: "InventarioSucursals",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IdBranch",
                table: "Inventories",
                column: "IdBranch");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IdProduct",
                table: "Inventories",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IdProvider",
                table: "Inventories",
                column: "IdProvider");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetail_IdInventory",
                table: "InventoryDetail",
                column: "IdInventory");

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
                name: "usuario_id1",
                table: "metodos_pago_usuario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_OpinionesProductos_ProductoIdProduct",
                table: "OpinionesProductos",
                column: "ProductoIdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_OpinionesProductos_UsuarioId",
                table: "OpinionesProductos",
                column: "UsuarioId");

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
                name: "IX_Pedidos_DireccionId",
                table: "Pedidos",
                column: "DireccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_RecetaId",
                table: "Pedidos",
                column: "RecetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_SucursalRecogidaId",
                table: "Pedidos",
                column: "SucursalRecogidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_DetailTypeId",
                table: "ProductDetails",
                column: "DetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_TipoDetalleId",
                table: "ProductDetails",
                column: "TipoDetalleId");

            migrationBuilder.CreateIndex(
                name: "IX_producto_imagenes_producto_id",
                table: "producto_imagenes",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoSintomas_ProductoId",
                table: "ProductoSintomas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoSintomas_SintomaId",
                table: "ProductoSintomas",
                column: "SintomaId");

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
                name: "categoria_id",
                table: "promocion_categorias",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "promocion_id",
                table: "promocion_categorias",
                column: "promocion_id");

            migrationBuilder.CreateIndex(
                name: "codigo_cupon",
                table: "promociones",
                column: "codigo_cupon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromocionProductos_ProductoId",
                table: "PromocionProductos",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocionProductos_PromocionId",
                table: "PromocionProductos",
                column: "PromocionId");

            migrationBuilder.CreateIndex(
                name: "nombre1",
                table: "proveedores",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "usuario_id2",
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
                name: "pais_id",
                table: "tasas_impuestos",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "IX_tipo_detalles_identificador_unico",
                table: "tipo_detalles",
                column: "identificador_unico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "pedido_id",
                table: "transacciones",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "aseguradora_id",
                table: "usuario_aseguradoras",
                column: "aseguradora_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id3",
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
                name: "proveedor_id",
                table: "usuarios",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "telefono",
                table: "usuarios",
                column: "telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZonasDomicilios_SucursalId",
                table: "ZonasDomicilios",
                column: "SucursalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarritoItems");

            migrationBuilder.DropTable(
                name: "DetallesPedidos");

            migrationBuilder.DropTable(
                name: "HorariosDomicilios");

            migrationBuilder.DropTable(
                name: "InventarioSucursals");

            migrationBuilder.DropTable(
                name: "InventoryDetail");

            migrationBuilder.DropTable(
                name: "items_media");

            migrationBuilder.DropTable(
                name: "metodos_pago_usuario");

            migrationBuilder.DropTable(
                name: "OpinionesProductos");

            migrationBuilder.DropTable(
                name: "otps_usuario");

            migrationBuilder.DropTable(
                name: "paginas_informativas");

            migrationBuilder.DropTable(
                name: "PlantillaProductos");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "producto_imagenes");

            migrationBuilder.DropTable(
                name: "ProductoSintomas");

            migrationBuilder.DropTable(
                name: "promocion_categorias");

            migrationBuilder.DropTable(
                name: "PromocionProductos");

            migrationBuilder.DropTable(
                name: "tasas_impuestos");

            migrationBuilder.DropTable(
                name: "transacciones");

            migrationBuilder.DropTable(
                name: "usuario_aseguradoras");

            migrationBuilder.DropTable(
                name: "usuario_roles");

            migrationBuilder.DropTable(
                name: "ZonasDomicilios");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "secciones_media");

            migrationBuilder.DropTable(
                name: "DetailType");

            migrationBuilder.DropTable(
                name: "tipo_detalles");

            migrationBuilder.DropTable(
                name: "sintomas");

            migrationBuilder.DropTable(
                name: "promociones");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "aseguradoras");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "direcciones");

            migrationBuilder.DropTable(
                name: "recetas_medicas");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Currencys");

            migrationBuilder.DropTable(
                name: "divisiones_geograficas");
        }
    }
}
