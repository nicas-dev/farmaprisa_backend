using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para almacenar todos los posibles síntomas.
/// </summary>
public partial class Sintoma
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<ProductoSintoma> ProductoSintomas { get; set; } = new List<ProductoSintoma>();
}
