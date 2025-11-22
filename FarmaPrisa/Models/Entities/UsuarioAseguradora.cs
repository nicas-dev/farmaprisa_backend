using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para Vincular Usuarios con Aseguradoras  conecta a cada usuario con su respectiva aseguradora y guarda su número de póliza. Esto permite que un usuario pueda tener pólizas con una o más aseguradoras.
/// </summary>
public partial class UsuarioAseguradora
{
    [Key]
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int AseguradoraId { get; set; }

    public string NumeroPoliza { get; set; } = null!;

    public bool? EstaVerificada { get; set; }

    public DateTime? FechaAgregado { get; set; }

    public virtual Aseguradora Aseguradora { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
