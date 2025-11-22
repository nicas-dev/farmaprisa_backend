using FarmaPrisa.Models.Dtos.Producto;

namespace FarmaPrisa.Models.Dtos
{
    public class DashboardDataDto
    {
        // KPIs del Dashboard
        public decimal IngresosDiarios { get; set; }
        public int TotalUsuariosRegistrados { get; set; }
        public int NuevasOrdenesDiarias { get; set; }

        // Gráficos de Ventas
        public Dictionary<string, decimal> VentasUltimos6Meses { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, int> PedidosDiarios { get; set; } = new Dictionary<string, int>();

        // Listas de Gestión (Productos a Vencer / Inventario Crítico)
        //public List<ProductoVencimientoDto> ProductosAproximadosAVencer { get; set; } = new List<ProductoVencimientoDto>();
    }
}
