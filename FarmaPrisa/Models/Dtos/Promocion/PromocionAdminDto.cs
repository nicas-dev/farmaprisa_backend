namespace FarmaPrisa.Models.Dtos.Promocion
{
    public class PromocionAdminDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string CodigoCupon { get; set; } = null!;
        public string TipoDescuento { get; set; } = null!; // 'porcentaje' o 'fijo'
        public decimal ValorDescuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool? EstaActiva { get; set; } // Estado actual de la promoción

        // Contadores para el panel de gestión
        public int TotalProductosAsociados { get; set; }
        public int TotalCategoriasAsociadas { get; set; }
    }
}
