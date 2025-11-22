using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para la gestión de los impuestos según el país
/// </summary>
public partial class TasasImpuesto
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Porcentaje { get; set; }

    public int PaisId { get; set; }

    public virtual DivisionesGeografica Pais { get; set; } = null!;
}
