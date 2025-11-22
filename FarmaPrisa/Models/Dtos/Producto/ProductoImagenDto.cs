namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoImagenDto
    {
        public int Id { get; set; }
        public string UrlImagen { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
