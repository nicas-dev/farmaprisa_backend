namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ImportResultDto
    {
        public int RegistrosExitosos { get; set; }
        public int TotalRegistrosProcesados { get; set; }
        public bool Exito { get; set; }
        // Detalle de errores por fila
        public List<string> MensajesError { get; set; } = new List<string>();
    }
}
