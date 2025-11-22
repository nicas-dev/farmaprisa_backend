using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para los usuarios.
/// </summary>
public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool? EstaActivo { get; set; }

    public int? ProveedorId { get; set; }

    public int PaisId { get; set; }

    public int PuntosFidelidad { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();

    public virtual ICollection<Direccione> Direcciones { get; set; } = new List<Direccione>();

    public virtual ICollection<MetodosPagoUsuario> MetodosPagoUsuarios { get; set; } = new List<MetodosPagoUsuario>();

    public virtual ICollection<OpinionesProducto> OpinionesProductos { get; set; } = new List<OpinionesProducto>();

    public virtual ICollection<PaginasInformativa> PaginasInformativas { get; set; } = new List<PaginasInformativa>();

    public virtual DivisionesGeografica Pais { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual Proveedore? Proveedor { get; set; }

    public virtual ICollection<RecetasMedica> RecetasMedicas { get; set; } = new List<RecetasMedica>();

    public virtual ICollection<UsuarioAseguradora> UsuarioAseguradoras { get; set; } = new List<UsuarioAseguradora>();

    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();

    public virtual ICollection<OtpUsuario> OtpsUsuario { get; set; } = new List<OtpUsuario>();
}
