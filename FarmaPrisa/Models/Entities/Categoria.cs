using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Para organizar productos. El campo &apos;categoria_padre_id&apos; nos permite crear subcategorías.
/// </summary>
public partial class Categoria
{
    //[StringLength(36)]
    //public string? IdExterno { get; set; } // ID de la tabla vieja
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? CategoriaPadreId { get; set; }

    public virtual Categoria? CategoriaPadre { get; set; }

    public virtual ICollection<Categoria> InverseCategoriaPadre { get; set; } = new List<Categoria>();

    public virtual ICollection<Product> Productos { get; set; } = new List<Product>();

    public virtual ICollection<PromocionCategoria> PromocionCategoria { get; set; } = new List<PromocionCategoria>();
}
