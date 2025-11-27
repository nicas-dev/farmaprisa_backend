using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities
{
    public class TipoMovimiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } // Ej: "Compra", "Venta", "Merma", "Ajuste"

        public bool EsIngreso { get; set; } // true = Suma al stock, false = Resta del stock

        public bool EsAjuste { get; set; } // Para diferenciar movimientos administrativos

        public string Descripcion { get; set; }
    }
}
