using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para que los usuarios suban sus recetas
/// </summary>
public partial class RecetasMedica
{
    [Key]
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string ImagenUrl { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? ComentariosAdmin { get; set; }

    public DateTime? FechaCarga { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual Usuario Usuario { get; set; } = null!;
}
