namespace FarmaPrisa.Models.Dtos.Inventory
{
    public class InventoryDto
    {
        public int ProductoId { get; set; }
        public string SKU { get; set; }
        public string Producto { get; set; }
        public string Sucursal { get; set; } // Opcional, null si es global
        public int StockTotal { get; set; }
    }
}
