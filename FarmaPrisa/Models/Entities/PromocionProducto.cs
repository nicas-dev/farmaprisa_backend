using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para aplicar una promoción a productos específicos
/// </summary>
public partial class PromocionProducto
{
    [Key]
    public int Id { get; set; }

    public int PromocionId { get; set; }

    public int? ProductoId { get; set; }

    public virtual Product Producto { get; set; } = null!;

    public virtual Promocione Promocion { get; set; } = null!;
}
