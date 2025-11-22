using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para la relación de los Usuarios con los Roles
/// </summary>
public partial class UsuarioRole
{
    [Key]
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int RolId { get; set; }

    public virtual Role Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
