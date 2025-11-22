namespace FarmaPrisa.Models.Dtos.Usuario
{
    public class PedidoAdminDto
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } // pendiente, procesando, entregado, etc.
        public string TipoEntrega { get; set; } // domicilio o recoger_en_tienda
        public decimal Total { get; set; }

        // Información del Cliente para el listado
        public int UsuarioId { get; set; }
        public string ClienteNombre { get; set; }
    }
}
