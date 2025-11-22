using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para los roles
/// </summary>
public partial class Role
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}
