using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para gestión de las zonas de domicilio
/// </summary>
public partial class ZonasDomicilio
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal CostoEnvio { get; set; }

    public int SucursalId { get; set; }

    public bool? EstaActiva { get; set; }

    public virtual ICollection<HorariosDomicilio> HorariosDomicilios { get; set; } = new List<HorariosDomicilio>();

    public virtual Branch Sucursal { get; set; } = null!;
}
