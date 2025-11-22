using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para los horarios de las sucursal
/// </summary>
public partial class HorariosSucursal
{
    [Key]
    public int Id { get; set; }

    public int SucursalId { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraApertura { get; set; }

    public TimeOnly HoraCierre { get; set; }

    public virtual Sucursale Sucursal { get; set; } = null!;
}
