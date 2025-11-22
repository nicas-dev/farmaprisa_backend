using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para conectar los productos con sus síntomas.
/// </summary>
public partial class ProductoSintoma
{
    [Key]
    public int Id { get; set; }

    public int? ProductoId { get; set; }

    public int SintomaId { get; set; }

    public virtual Product Producto { get; set; } = null!;

    public virtual Sintoma Sintoma { get; set; } = null!;
}
