using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para gestión de horarios de los domicilio
/// </summary>
public partial class HorariosDomicilio
{
    [Key]
    public int Id { get; set; }

    public int ZonaId { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraCierre { get; set; }

    public virtual ZonasDomicilio Zona { get; set; } = null!;
}
