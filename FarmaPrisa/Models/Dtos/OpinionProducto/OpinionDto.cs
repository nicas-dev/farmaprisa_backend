namespace FarmaPrisa.Models.Dtos.OpinionProducto
{
    public class OpinionDto
    {
        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime? FechaOpinion { get; set; }
        public string UsuarioNombre { get; set; } // Nombre del usuario que opinó
        public string? ProductoNombre { get; set; }
    }
}
