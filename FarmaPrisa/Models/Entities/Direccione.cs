using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para que los usuarios guarden múltiples direcciones de envío.
/// </summary>
public partial class Direccione
{
    [Key]
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string DireccionCompleta { get; set; } = null!;

    public int CiudadId { get; set; }

    public string? Referencia { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public bool? EsPredeterminada { get; set; }

    public virtual DivisionesGeografica Ciudad { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual Usuario Usuario { get; set; } = null!;
}
