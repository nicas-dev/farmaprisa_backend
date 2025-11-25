using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para las divisiones geográficas y referirnos a los continentes, países,  provincias, departamentos, ciudades, barrios, etc. según convenga
/// </summary>
public partial class DivisionesGeografica
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string TipoDivision { get; set; } = null!;

    public int? DivisionPadreId { get; set; }

    public virtual ICollection<Direccione> Direcciones { get; set; } = new List<Direccione>();

    public virtual DivisionesGeografica? DivisionPadre { get; set; }

    public virtual ICollection<DivisionesGeografica> InverseDivisionPadre { get; set; } = new List<DivisionesGeografica>();

    public virtual ICollection<Branch> Sucursales { get; set; } = new List<Branch>();

    public virtual ICollection<TasasImpuesto> TasasImpuestos { get; set; } = new List<TasasImpuesto>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
