namespace FarmaPrisa.Models.Dtos.Categoria
{
    public class CategoriaResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CategoriaDataDto Data { get; set; }
    }
}
