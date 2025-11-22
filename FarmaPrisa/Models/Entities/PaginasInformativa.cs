using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Páginas con información estática, con esta tabla, el administrador podrá crear y editar fácilmente el contenido de estas páginas desde el panel de control.
/// </summary>
public partial class PaginasInformativa
{
    [Key]
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string ContenidoHtml { get; set; } = null!;

    public bool? EstaPublicada { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? AutorId { get; set; }

    public virtual Usuario? Autor { get; set; }
}
