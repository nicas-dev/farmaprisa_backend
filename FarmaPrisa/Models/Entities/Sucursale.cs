using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para gestionar las sucursales
/// </summary>
public partial class Sucursale
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string DireccionCompleta { get; set; } = null!;

    public int CiudadId { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public string? Telefono { get; set; }

    public bool? EstaActiva { get; set; }

    public virtual DivisionesGeografica Ciudad { get; set; } = null!;

    public virtual ICollection<HorariosSucursal> HorariosSucursals { get; set; } = new List<HorariosSucursal>();

    public virtual ICollection<InventarioSucursal> InventarioSucursals { get; set; } = new List<InventarioSucursal>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<ZonasDomicilio> ZonasDomicilios { get; set; } = new List<ZonasDomicilio>();
}
