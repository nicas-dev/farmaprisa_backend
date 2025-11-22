namespace FarmaPrisa.Models.Dtos
{
    public class ApiResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; } // Para devolver el ID o cualquier otro dato
    }
}
