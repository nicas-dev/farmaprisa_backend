using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FarmaPrisa.Models.Entities
{
 
    public class Product
    {
        [Key]
        [Column("IdProduct")]
        public int IdProduct { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(100)]
        public string BarCode { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(10)]
        public string Language { get; set; } = "ES";

        public virtual Categoria Category { get; set; }
       
        public virtual Proveedore Supplier { get; set; }
  
        public virtual Brand Brand { get; set; }
    
        public virtual ICollection<ProductDetail> Details { get; set; }

        public virtual ICollection<ProductoImagen> Imagenes { get; set; } = new List<ProductoImagen>();

    }
}
