namespace FarmaPrisa.Models.Dtos.Promocion
{
    public class PromocionDetalleDto
    {
        // Propiedades Básicas de la Promoción (Copiadas de PromocionAdminDto)
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string CodigoCupon { get; set; } = null!;
        public string TipoDescuento { get; set; } = null!;
        public decimal ValorDescuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool? EstaActiva { get; set; }

        // === Detalle de las Asignaciones ===

        /// <summary>Lista de IDs de productos a los que aplica la promoción.</summary>
        public List<int?> ProductosAsociadosIds { get; set; } = new List<int?>();

        /// <summary>Lista de IDs de categorías a las que aplica la promoción.</summary>
        public List<int> CategoriasAsociadasIds { get; set; } = new List<int>();
    }
}
