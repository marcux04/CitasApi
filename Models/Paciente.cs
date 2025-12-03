using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasApi.Models
{
    [Table("catPacientes")]
    public class Paciente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [MaxLength(18)]
        public string Curp { get; set; } = null!;

        [MaxLength(10)]
        public string? Telefono { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}