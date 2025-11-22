using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla de Opiniones y Valoraciones
/// </summary>
public partial class OpinionesProducto
{
    [Key]
    public int Id { get; set; }

    public int? producto_id { get; set; }

    public int UsuarioId { get; set; }

    public int Calificacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaOpinion { get; set; }

    public virtual Product Producto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
