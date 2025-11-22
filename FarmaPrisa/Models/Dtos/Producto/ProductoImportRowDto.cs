using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoImportRowDto
    {
        // Campos Obligatorios para la entidad Producto
        [Required]
        public string NOMBRE { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public decimal PRECIO { get; set; }
        [Required]
        public string CATEGORIA_IDENTIFICADOR { get; set; }
        [Required]
        public string PROVEEDOR_IDENTIFICADOR { get; set; }

        // Campos de Producto Opcionales/Booleanos
        public decimal? PRECIO_ANTERIOR { get; set; }
        public string? IMAGEN_URL { get; set; }
        [Required] // Requiere 1 o 0
        public int REQUIERE_RECETA { get; set; }
        [Required] // Requiere 1 o 0
        public int ACTIVO { get; set; }

        [Required] // Idioma es necesario para saber cómo guardar los detalles
        public string IDIOMA { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? PRODUCT_SPECIFICATIONS { get; set; }
        public string? INGREDIENTS { get; set; }
        public string? NUTRITION_FACTS { get; set; }
        public string? WARNINGS { get; set; }
        public string? SHIPPING_SPECIFICATIONS { get; set; }
        public string? FROM_THE_BRAND { get; set; }
    }
}
