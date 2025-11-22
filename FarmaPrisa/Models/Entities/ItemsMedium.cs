using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Para poder guardar los elementos de los archivos medias en las secciones
/// </summary>
public partial class ItemsMedium
{
    [Key]
    public int Id { get; set; }

    public int SeccionId { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public string MediaUrl { get; set; } = null!;

    public string TipoMedia { get; set; } = null!;

    public string? UrlDestino { get; set; }

    public int? Orden { get; set; }

    public bool? EstaActivo { get; set; }

    public virtual SeccionesMedium Seccion { get; set; } = null!;
}
