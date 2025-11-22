using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para la gestión de los métodos de pagos con
/// </summary>
public partial class MetodosPagoUsuario
{
    [Key]
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string GatewayToken { get; set; } = null!;

    public string? TipoTarjeta { get; set; }

    public string? UltimosCuatroDigitos { get; set; }

    public int? MesExpiracion { get; set; }

    public int? AnoExpiracion { get; set; }

    public bool? EsPredeterminado { get; set; }

    public DateTime? FechaAgregado { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
