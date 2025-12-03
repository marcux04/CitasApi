using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasApi.Models
{
    [Table("citas")]
    public class Cita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }

        [Required]
        [ForeignKey("Medico")]
        public int MedicoId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan Hora { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "pendiente";

        public virtual Paciente Paciente { get; set; } = null!;
        public virtual Medico Medico { get; set; } = null!;
    }
}