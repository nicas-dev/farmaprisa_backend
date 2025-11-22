using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla principal para definir cada promoción
/// </summary>
public partial class Promocione
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string TipoDescuento { get; set; } = null!;

    public decimal ValorDescuento { get; set; }

    public string? CodigoCupon { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public bool? EstaActiva { get; set; }

    public virtual ICollection<PromocionCategoria> PromocionCategoria { get; set; } = new List<PromocionCategoria>();

    public virtual ICollection<PromocionProducto> PromocionProductos { get; set; } = new List<PromocionProducto>();
}
