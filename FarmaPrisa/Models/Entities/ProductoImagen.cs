using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities
{
    [Table("producto_imagenes")]
    public class ProductoImagen
    {
        [Key]
        public int Id { get; set; }


        [Column("producto_id")]
        public int ProductoId { get; set; } // FK a Product, obligatorio

        [ForeignKey("ProductoId")]
        public Product? Product { get; set; } // Propiedad de Navegación

        [Column("url_imagen")]
        [StringLength(500)]
        public string UrlImagen { get; set; } = null!; // La ruta pública en el VPS

        [Column("orden")]
        public int Orden { get; set; } = 0; // Orden de visualización en la galería

        [Column("es_principal")]
        public bool EsPrincipal { get; set; } = false;
    }
}
