using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para definir las ubicaciones o secciones de media en el sitio
/// </summary>
public partial class SeccionesMedium
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string IdentificadorUnico { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<ItemsMedium> ItemsMedia { get; set; } = new List<ItemsMedium>();
}
