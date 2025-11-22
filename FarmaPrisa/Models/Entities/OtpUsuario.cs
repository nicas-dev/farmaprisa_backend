using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    /// <summary>
    /// Tabla para la gestión y validación de Códigos de Uso Único (OTP).
    /// </summary>
    [Table("otps_usuario")] // Define el nombre de la tabla en MariaDB
    public class OtpUsuario
    {
        [Key]
        [Column("id")]
        public int? Id { get; set; }

        // Clave Foránea al Usuario
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } // Propiedad de Navegación

        [Column("codigo_otp")]
        [StringLength(10)]
        public string CodigoOtp { get; set; } = null!;

        [Column("tipo_uso")]
        [StringLength(50)]
        public string TipoUso { get; set; } = null!; // Ej: 'REGISTRO', 'RECUPERACION_CLAVE'

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("fecha_expiracion")]
        public DateTime FechaExpiracion { get; set; }

        [Column("esta_usado")]
        public bool EstaUsado { get; set; } = false;
    }
}
