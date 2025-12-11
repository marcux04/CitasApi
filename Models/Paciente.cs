using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasApi.Models
{
    [Table("catPacientes")]
    public class Paciente
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
        [Column("curp")]
        [MaxLength(18)]
        public string Curp { get; set; } = null!;

        [Column("telefono")]
        [MaxLength(10)]
        public string? Telefono { get; set; }

        [Required]
        [Column("correo")]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        [Column("password")]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        // Relaciones
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}