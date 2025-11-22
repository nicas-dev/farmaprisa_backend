using System;
using System.Collections.Generic;
using FarmaPrisa.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace FarmaPrisa.Data;

public partial class FarmaPrisaContext : DbContext
{
    public FarmaPrisaContext()
    {
    }

    public FarmaPrisaContext(DbContextOptions<FarmaPrisaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aseguradora> Aseguradoras { get; set; }

    public virtual DbSet<CarritoItem> CarritoItems { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetallesPedido> DetallesPedidos { get; set; }

    public virtual DbSet<Direccione> Direcciones { get; set; }

    public virtual DbSet<DivisionesGeografica> DivisionesGeograficas { get; set; }

    public virtual DbSet<HorariosDomicilio> HorariosDomicilios { get; set; }

    public virtual DbSet<HorariosSucursal> HorariosSucursals { get; set; }

    public virtual DbSet<InventarioSucursal> InventarioSucursals { get; set; }

    public virtual DbSet<ItemsMedium> ItemsMedia { get; set; }

    public virtual DbSet<MetodosPagoUsuario> MetodosPagoUsuarios { get; set; }

    public virtual DbSet<OpinionesProducto> OpinionesProductos { get; set; }

    public virtual DbSet<PaginasInformativa> PaginasInformativas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<ProductoSintoma> ProductoSintomas { get; set; }
    public virtual DbSet<ProductoImagen> ProductoImagenes { get; set; }
    public virtual DbSet<TipoDetalle> TipoDetalles { get; set; }

    public virtual DbSet<PromocionCategoria> PromocionCategorias { get; set; }

    public virtual DbSet<PromocionProducto> PromocionProductos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<RecetasMedica> RecetasMedicas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SeccionesMedium> SeccionesMedia { get; set; }

    public virtual DbSet<Sintoma> Sintomas { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<TasasImpuesto> TasasImpuestos { get; set; }

    public virtual DbSet<Transaccione> Transacciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<OtpUsuario> OtpsUsuario { get; set; }

    public virtual DbSet<UsuarioAseguradora> UsuarioAseguradoras { get; set; }

    public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; }

    public virtual DbSet<ZonasDomicilio> ZonasDomicilios { get; set; }
    public virtual DbSet<PlantillaProductos> PlantillaProductos { get; set; }

    //Tablas agregadas
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetail> ProductDetails { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<DetailType> DetailType { get; set; }
    public DbSet<Currency> Currencys { get; set; }
    public DbSet<Company> Companys { get; set; }

    

    // Lo eliminamos para que Entity Framework utilice la configuración que ya establecimos en Program.cs, que es la forma correcta y centralizada de manejar la configuración de la base de datos en ASP.NET Core.
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseMySql("server=localhost;port=3308;database=dbfarmaprisa;uid=root;pwd=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("12.0.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
  

        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Aseguradora>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aseguradoras", tb => tb.HasComment("Tabla para gestionar las aseguradoras, contendrá la lista de todas las compañías de seguros con las que la farmacia tiene convenio"));

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ContactoEmail)
                .HasMaxLength(255)
                .HasColumnName("contacto_email");
            entity.Property(e => e.ContactoTelefono)
                .HasMaxLength(50)
                .HasColumnName("contacto_telefono");
            entity.Property(e => e.EstaActiva)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_activa");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });



        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categorias", tb => tb.HasComment("Para organizar productos. El campo 'categoria_padre_id' nos permite crear subcategorías."));

            entity.HasIndex(e => e.CategoriaPadreId, "categoria_padre_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CategoriaPadreId)
                .HasColumnType("int(11)")
                .HasColumnName("categoria_padre_id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");

            entity.HasOne(d => d.CategoriaPadre).WithMany(p => p.InverseCategoriaPadre)
                .HasForeignKey(d => d.CategoriaPadreId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("categorias_ibfk_1");
        });


        modelBuilder.Entity<Direccione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("direcciones", tb => tb.HasComment("Tabla para que los usuarios guarden múltiples direcciones de envío."));

            entity.HasIndex(e => e.CiudadId, "ciudad_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CiudadId)
                .HasColumnType("int(11)")
                .HasColumnName("ciudad_id");
            entity.Property(e => e.DireccionCompleta)
                .HasColumnType("text")
                .HasColumnName("direccion_completa");
            entity.Property(e => e.EsPredeterminada)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_predeterminada");
            entity.Property(e => e.Latitud)
                .HasPrecision(10, 8)
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasPrecision(11, 8)
                .HasColumnName("longitud");
            entity.Property(e => e.Referencia)
                .HasColumnType("text")
                .HasColumnName("referencia");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Ciudad).WithMany(p => p.Direcciones)
                .HasForeignKey(d => d.CiudadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("direcciones_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Direcciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("direcciones_ibfk_1");
        });

        modelBuilder.Entity<DivisionesGeografica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("divisiones_geograficas", tb => tb.HasComment("Tabla para las divisiones geográficas y referirnos a los continentes, países,  provincias, departamentos, ciudades, barrios, etc. según convenga"));

            entity.HasIndex(e => e.DivisionPadreId, "division_padre_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DivisionPadreId)
                .HasColumnType("int(11)")
                .HasColumnName("division_padre_id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoDivision)
                .HasMaxLength(100)
                .HasColumnName("tipo_division");

            entity.HasOne(d => d.DivisionPadre).WithMany(p => p.InverseDivisionPadre)
                .HasForeignKey(d => d.DivisionPadreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("divisiones_geograficas_ibfk_1");
        });

        modelBuilder.Entity<HorariosDomicilio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horarios_domicilio", tb => tb.HasComment("Tabla para gestión de horarios de los domicilio"));

            entity.HasIndex(e => e.ZonaId, "zona_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DiaSemana)
                .HasColumnType("enum('lunes','martes','miercoles','jueves','viernes','sabado','domingo')")
                .HasColumnName("dia_semana");
            entity.Property(e => e.HoraCierre)
                .HasColumnType("time")
                .HasColumnName("hora_cierre");
            entity.Property(e => e.HoraInicio)
                .HasColumnType("time")
                .HasColumnName("hora_inicio");
            entity.Property(e => e.ZonaId)
                .HasColumnType("int(11)")
                .HasColumnName("zona_id");

            entity.HasOne(d => d.Zona).WithMany(p => p.HorariosDomicilios)
                .HasForeignKey(d => d.ZonaId)
                .HasConstraintName("horarios_domicilio_ibfk_1");
        });

        modelBuilder.Entity<HorariosSucursal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horarios_sucursal", tb => tb.HasComment("Tabla para los horarios de las sucursal"));

            entity.HasIndex(e => e.SucursalId, "sucursal_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DiaSemana)
                .HasColumnType("enum('lunes','martes','miercoles','jueves','viernes','sabado','domingo')")
                .HasColumnName("dia_semana");
            entity.Property(e => e.HoraApertura)
                .HasColumnType("time")
                .HasColumnName("hora_apertura");
            entity.Property(e => e.HoraCierre)
                .HasColumnType("time")
                .HasColumnName("hora_cierre");
            entity.Property(e => e.SucursalId)
                .HasColumnType("int(11)")
                .HasColumnName("sucursal_id");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.HorariosSucursals)
                .HasForeignKey(d => d.SucursalId)
                .HasConstraintName("horarios_sucursal_ibfk_1");
        });



        modelBuilder.Entity<ItemsMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items_media", tb => tb.HasComment("Para poder guardar los elementos de los archivos medias en las secciones"));

            entity.HasIndex(e => e.SeccionId, "seccion_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstaActivo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_activo");
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(255)
                .HasColumnName("media_url");
            entity.Property(e => e.Orden)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("orden");
            entity.Property(e => e.SeccionId)
                .HasColumnType("int(11)")
                .HasColumnName("seccion_id");
            entity.Property(e => e.TipoMedia)
                .HasDefaultValueSql("'imagen'")
                .HasColumnType("enum('imagen','video')")
                .HasColumnName("tipo_media");
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .HasColumnName("titulo");
            entity.Property(e => e.UrlDestino)
                .HasMaxLength(255)
                .HasColumnName("url_destino");

            entity.HasOne(d => d.Seccion).WithMany(p => p.ItemsMedia)
                .HasForeignKey(d => d.SeccionId)
                .HasConstraintName("items_media_ibfk_1");
        });

        modelBuilder.Entity<MetodosPagoUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("metodos_pago_usuario", tb => tb.HasComment("Tabla para la gestión de los métodos de pagos con"));

            entity.HasIndex(e => e.GatewayToken, "gateway_token").IsUnique();

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AnoExpiracion)
                .HasColumnType("int(11)")
                .HasColumnName("ano_expiracion");
            entity.Property(e => e.EsPredeterminado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_predeterminado");
            entity.Property(e => e.FechaAgregado)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_agregado");
            entity.Property(e => e.GatewayToken).HasColumnName("gateway_token");
            entity.Property(e => e.MesExpiracion)
                .HasColumnType("int(11)")
                .HasColumnName("mes_expiracion");
            entity.Property(e => e.TipoTarjeta)
                .HasMaxLength(50)
                .HasColumnName("tipo_tarjeta");
            entity.Property(e => e.UltimosCuatroDigitos)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("ultimos_cuatro_digitos");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MetodosPagoUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("metodos_pago_usuario_ibfk_1");
        });


        modelBuilder.Entity<PaginasInformativa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paginas_informativas", tb => tb.HasComment("Páginas con información estática, con esta tabla, el administrador podrá crear y editar fácilmente el contenido de estas páginas desde el panel de control."));

            entity.HasIndex(e => e.AutorId, "autor_id");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AutorId)
                .HasColumnType("int(11)")
                .HasColumnName("autor_id");
            entity.Property(e => e.ContenidoHtml)
                .HasColumnType("text")
                .HasColumnName("contenido_html");
            entity.Property(e => e.EstaPublicada)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_publicada");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Slug).HasColumnName("slug");
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Autor).WithMany(p => p.PaginasInformativas)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("paginas_informativas_ibfk_1");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedidos", tb => tb.HasComment("Tabla para guardar los pedidos de los clientes."));

            entity.HasIndex(e => e.DireccionId, "direccion_id");

            entity.HasIndex(e => e.RecetaId, "receta_id");

            entity.HasIndex(e => e.SucursalRecogidaId, "sucursal_recogida_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CostoEnvio)
                .HasPrecision(10, 2)
                .HasColumnName("costo_envio");
            entity.Property(e => e.DireccionId)
                .HasColumnType("int(11)")
                .HasColumnName("direccion_id");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'pendiente'")
                .HasColumnType("enum('pendiente','procesando','listo_para_recoger','en_camino','entregado','cancelado')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_pedido");
            entity.Property(e => e.MontoDescuento)
                .HasPrecision(10, 2)
                .HasColumnName("monto_descuento");
            entity.Property(e => e.MontoImpuesto)
                .HasPrecision(10, 2)
                .HasColumnName("monto_impuesto");
            entity.Property(e => e.RecetaId)
                .HasColumnType("int(11)")
                .HasColumnName("receta_id");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.SucursalRecogidaId)
                .HasColumnType("int(11)")
                .HasColumnName("sucursal_recogida_id");
            entity.Property(e => e.TipoEntrega)
                .HasColumnType("enum('domicilio','recoger_en_tienda')")
                .HasColumnName("tipo_entrega");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Direccion).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.DireccionId)
                .HasConstraintName("pedidos_ibfk_2");

            entity.HasOne(d => d.Receta).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.RecetaId)
                .HasConstraintName("pedidos_ibfk_4");

            entity.HasOne(d => d.SucursalRecogida).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.SucursalRecogidaId)
                .HasConstraintName("pedidos_ibfk_3");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedidos_ibfk_1");
        });

   
        modelBuilder.Entity<ProductoImagen>(entity =>
        {
            // 1. Configuración de la tabla y clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("producto_imagenes");

            // 2. Mapeo de Columnas (Asegurar snake_case y tipos)
            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");

            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("producto_id"); // FK: Mapeo al nombre snake_case

            entity.Property(e => e.UrlImagen)
                .HasMaxLength(500)
                .HasColumnName("url_imagen"); // URL de la imagen guardada en el VPS

            entity.Property(e => e.Orden)
                .HasColumnType("int(11)")
                .HasColumnName("orden");

            // 3. Configuración de la Relación (Opcional, para evitar conflicto de filtro)
            entity.HasOne(d => d.Product)
                .WithMany(p => p.Imagenes) // El nombre de la colección en la entidad Producto
                .HasForeignKey(d => d.ProductoId)
                .IsRequired(false) // <--- ¡CRÍTICO! Permite que el Producto esté oculto por filtro
                .HasConstraintName("producto_imagenes_ibfk_1");
        });

        modelBuilder.Entity<PromocionCategoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("promocion_categorias", tb => tb.HasComment("Tabla para aplicar una promoción a categorías enteras"));

            entity.HasIndex(e => e.CategoriaId, "categoria_id");

            entity.HasIndex(e => e.PromocionId, "promocion_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CategoriaId)
                .HasColumnType("int(11)")
                .HasColumnName("categoria_id");
            entity.Property(e => e.PromocionId)
                .HasColumnType("int(11)")
                .HasColumnName("promocion_id");

            entity.HasOne(d => d.Categoria).WithMany(p => p.PromocionCategoria)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("promocion_categorias_ibfk_2");

            entity.HasOne(d => d.Promocion).WithMany(p => p.PromocionCategoria)
                .HasForeignKey(d => d.PromocionId)
                .HasConstraintName("promocion_categorias_ibfk_1");
        });


        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("promociones", tb => tb.HasComment("Tabla principal para definir cada promoción"));

            entity.HasIndex(e => e.CodigoCupon, "codigo_cupon").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CodigoCupon)
                .HasMaxLength(50)
                .HasColumnName("codigo_cupon");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstaActiva)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_activa");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoDescuento)
                .HasColumnType("enum('porcentaje','fijo')")
                .HasColumnName("tipo_descuento");
            entity.Property(e => e.ValorDescuento)
                .HasPrecision(10, 2)
                .HasColumnName("valor_descuento");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("proveedores", tb => tb.HasComment("Tabla para los proveedores o laboratorios."));

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(255)
                .HasColumnName("logo_url");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<RecetasMedica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recetas_medicas", tb => tb.HasComment("Tabla para que los usuarios suban sus recetas"));

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ComentariosAdmin)
                .HasColumnType("text")
                .HasColumnName("comentarios_admin");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'pendiente'")
                .HasColumnType("enum('pendiente','aprobada','rechazada','usada')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCarga)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_carga");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .HasColumnName("imagen_url");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RecetasMedicas)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("recetas_medicas_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles", tb => tb.HasComment("Tabla para los roles"));

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<SeccionesMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("secciones_media", tb => tb.HasComment("Tabla para definir las ubicaciones o secciones de media en el sitio"));

            entity.HasIndex(e => e.IdentificadorUnico, "identificador_unico").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.IdentificadorUnico)
                .HasMaxLength(100)
                .HasColumnName("identificador_unico");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Sintoma>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sintomas", tb => tb.HasComment("Tabla para almacenar todos los posibles síntomas."));

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sucursales", tb => tb.HasComment("Tabla para gestionar las sucursales"));

            entity.HasIndex(e => e.CiudadId, "ciudad_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CiudadId)
                .HasColumnType("int(11)")
                .HasColumnName("ciudad_id");
            entity.Property(e => e.DireccionCompleta)
                .HasColumnType("text")
                .HasColumnName("direccion_completa");
            entity.Property(e => e.EstaActiva)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_activa");
            entity.Property(e => e.Latitud)
                .HasPrecision(10, 8)
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasPrecision(11, 8)
                .HasColumnName("longitud");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.Ciudad).WithMany(p => p.Sucursales)
                .HasForeignKey(d => d.CiudadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sucursales_ibfk_1");
        });

        modelBuilder.Entity<TipoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("tipo_detalles");

            // Asumiendo que has mapeado las propiedades en tu clase TipoDetalle:
            entity.Property(e => e.IdentificadorUnico).HasColumnName("identificador_unico").IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.IdentificadorUnico).IsUnique();
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(255);
            entity.Property(e => e.EstaActivo).HasColumnName("esta_activo");
            // ... (otras propiedades de TipoDetalle)
        });

        modelBuilder.Entity<TasasImpuesto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tasas_impuestos", tb => tb.HasComment("Tabla para la gestión de los impuestos según el país"));

            entity.HasIndex(e => e.PaisId, "pais_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PaisId)
                .HasColumnType("int(11)")
                .HasColumnName("pais_id");
            entity.Property(e => e.Porcentaje)
                .HasPrecision(5, 2)
                .HasColumnName("porcentaje");

            entity.HasOne(d => d.Pais).WithMany(p => p.TasasImpuestos)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tasas_impuestos_ibfk_1");
        });

        modelBuilder.Entity<Transaccione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transacciones", tb => tb.HasComment("Tabla para guardar un registro del pago"));

            entity.HasIndex(e => e.PedidoId, "pedido_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasColumnType("enum('exitoso','fallido')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaTransaccion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_transaccion");
            entity.Property(e => e.IdTransaccionPasarela)
                .HasMaxLength(255)
                .HasColumnName("id_transaccion_pasarela");
            entity.Property(e => e.Monto)
                .HasPrecision(10, 2)
                .HasColumnName("monto");
            entity.Property(e => e.PasarelaPago)
                .HasMaxLength(50)
                .HasColumnName("pasarela_pago");
            entity.Property(e => e.PedidoId)
                .HasColumnType("int(11)")
                .HasColumnName("pedido_id");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transacciones_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios", tb => tb.HasComment("Tabla para los usuarios."));

            entity.HasQueryFilter(u => u.EstaActivo == true);

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.PaisId, "pais_id");

            entity.HasIndex(e => e.ProveedorId, "proveedor_id");

            entity.HasIndex(e => e.Telefono, "telefono").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.EstaActivo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_activo");
            entity.Property(e => e.FechaActualizacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(255)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.PaisId)
                .HasColumnType("int(11)")
                .HasColumnName("pais_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.ProveedorId)
                .HasColumnType("int(11)")
                .HasColumnName("proveedor_id");
            entity.Property(e => e.PuntosFidelidad)
                .HasColumnType("int(11)")
                .HasColumnName("puntos_fidelidad");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.Pais).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_ibfk_2");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("usuarios_ibfk_1");
        });

        modelBuilder.Entity<UsuarioAseguradora>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_aseguradoras", tb => tb.HasComment("Tabla para Vincular Usuarios con Aseguradoras  conecta a cada usuario con su respectiva aseguradora y guarda su número de póliza. Esto permite que un usuario pueda tener pólizas con una o más aseguradoras."));

            entity.HasIndex(e => e.AseguradoraId, "aseguradora_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AseguradoraId)
                .HasColumnType("int(11)")
                .HasColumnName("aseguradora_id");
            entity.Property(e => e.EstaVerificada)
                .HasDefaultValueSql("'0'")
                .HasColumnName("esta_verificada");
            entity.Property(e => e.FechaAgregado)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_agregado");
            entity.Property(e => e.NumeroPoliza)
                .HasMaxLength(100)
                .HasColumnName("numero_poliza");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Aseguradora).WithMany(p => p.UsuarioAseguradoras)
                .HasForeignKey(d => d.AseguradoraId)
                .HasConstraintName("usuario_aseguradoras_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioAseguradoras)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("usuario_aseguradoras_ibfk_1");
        });

        modelBuilder.Entity<UsuarioRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_roles", tb => tb.HasComment("Tabla para la relación de los Usuarios con los Roles"));

            entity.HasIndex(e => e.RolId, "rol_id");

            entity.HasIndex(e => new { e.UsuarioId, e.RolId }, "uq_usuario_rol").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.RolId)
                .HasColumnType("int(11)")
                .HasColumnName("rol_id");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("usuario_roles_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("usuario_roles_ibfk_1");
        });

        modelBuilder.Entity<ZonasDomicilio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("zonas_domicilio", tb => tb.HasComment("Tabla para gestión de las zonas de domicilio"));

            entity.HasIndex(e => e.SucursalId, "sucursal_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CostoEnvio)
                .HasPrecision(10, 2)
                .HasColumnName("costo_envio");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstaActiva)
                .HasDefaultValueSql("'1'")
                .HasColumnName("esta_activa");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.SucursalId)
                .HasColumnType("int(11)")
                .HasColumnName("sucursal_id");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.ZonasDomicilios)
                .HasForeignKey(d => d.SucursalId)
                .HasConstraintName("zonas_domicilio_ibfk_1");
        });

        modelBuilder.Entity<OtpUsuario>(entity =>
        {
            // 1. Configuración de la tabla y clave primaria
            entity.HasKey(e => e.Id);
            entity.ToTable("otps_usuario");

            // 2. Mapeo de Columnas (Para asegurar snake_case)
            entity.Property(e => e.Id).HasColumnName("id").HasColumnType("int(11)");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("int(11)");
            entity.Property(e => e.CodigoOtp).HasColumnName("codigo_otp").HasMaxLength(10);
            entity.Property(e => e.TipoUso).HasColumnName("tipo_uso").HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaExpiracion).HasColumnName("fecha_expiracion");
            entity.Property(e => e.EstaUsado).HasColumnName("esta_usado");

            // 3. Configuración de la Relación (Opcional, para evitar conflicto de filtro)
            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.OtpsUsuario)
                .HasForeignKey(d => d.UsuarioId) // Usa la clave foránea anulable
                .IsRequired(false)
                .HasConstraintName("otps_usuario_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
