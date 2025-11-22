using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla de cruce para saber qué productos van en cada pedido.
/// </summary>
public partial class DetallesPedido
{
    [Key]
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int? ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Product Producto { get; set; } = null!;
}
