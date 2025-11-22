using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para el carrito de las compras
/// A esta tabla se le hace un DELETE limpio cada que se hace una compra, porque son datos temporales y se usa como una mesa de trabajo donde el cliente coloca los productos y luego se deja limpio. Así se evita que se acumule basura y se hagan más lentas las consultas de los productos del carrito de compras por usuario
/// </summary>
public partial class CarritoItem
{
    [Key]
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int? ProductoId { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaAgregado { get; set; }

    //public virtual Producto Producto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
