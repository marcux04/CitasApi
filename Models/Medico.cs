using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasApi.Models
{
    [Table("catMedicos")]
    public class Medico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column("especialidad")]
        [MaxLength(50)]
        public string Especialidad { get; set; } = null!;

        // Relaciones
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}