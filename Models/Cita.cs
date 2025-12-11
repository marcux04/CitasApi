using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasApi.Models
{
    [Table("citas")]
    public class Cita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("paciente_id")]
        public int PacienteId { get; set; }

        [Required]
        [Column("medico_id")]
        public int MedicoId { get; set; }

        [Required]
        [Column("fecha", TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column("hora")]
        public TimeSpan Hora { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string Estado { get; set; } = "pendiente";

        // Propiedades de navegación
        [ForeignKey("PacienteId")]
        public virtual Paciente? Paciente { get; set; }

        [ForeignKey("MedicoId")]
        public virtual Medico? Medico { get; set; }
    }
}