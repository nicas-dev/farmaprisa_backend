using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para guardar los pedidos de los clientes.
/// </summary>
public partial class Pedido
{
    [Key]
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string TipoEntrega { get; set; } = null!;

    public int? DireccionId { get; set; }

    public int? SucursalRecogidaId { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public int? RecetaId { get; set; }

    public decimal Subtotal { get; set; }

    public decimal CostoEnvio { get; set; }

    public decimal MontoDescuento { get; set; }

    public decimal MontoImpuesto { get; set; }

    public DateTime? FechaPedido { get; set; }

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual Direccione? Direccion { get; set; }

    public virtual RecetasMedica? Receta { get; set; }

    public virtual Sucursale? SucursalRecogida { get; set; }

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();

    public virtual Usuario Usuario { get; set; } = null!;
}
