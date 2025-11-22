using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities
{
    public class PlantillaProductos
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
