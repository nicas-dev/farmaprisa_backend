using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities;

/// <summary>
/// Tabla para guardar un registro del pago
/// </summary>
public partial class Transaccione
{
    [Key]
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public decimal Monto { get; set; }

    public string? PasarelaPago { get; set; }

    public string? IdTransaccionPasarela { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime? FechaTransaccion { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
