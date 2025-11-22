using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para los proveedores o laboratorios.
/// </summary>
public partial class Proveedore
{
    //[StringLength(36)]
    //public string? IdExterno { get; set; } // ID de la tabla vieja
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? LogoUrl { get; set; }
    public string? Direccion { get; set; }
    public string? NombreContacto { get; set; }
    public string? EmailContacto { get; set; }
    public string? TelefonoContacto { get; set; }
    public string? IdentificacionFiscal { get; set; } // Para taxid o vatid

    [Column("esta_activo")]
    public bool EstaActivo { get; set; } = true;

    public virtual ICollection<Product> Productos { get; set; } = new List<Product>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
