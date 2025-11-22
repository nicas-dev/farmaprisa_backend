namespace FarmaPrisa.Models.Dtos.Categoria
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        // Campo para identificar si es una subcategoría
        public int? CategoriaPadreId { get; set; }

        // Propiedad recursiva para manejar las subcategorías en un listado jerárquico
        public List<CategoriaDto>? Subcategorias { get; set; }
    }
}
