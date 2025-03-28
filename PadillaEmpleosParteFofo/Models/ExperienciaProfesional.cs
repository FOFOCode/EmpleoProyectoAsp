using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class ExperienciaProfesional
    {
        [Key]
        public int id_experienciaprofesional { get; set; }
        [ForeignKey("Curriculum")]
        public int id_curriculum { get; set; }
        public string? puesto { get; set; }
        public string? empresa { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }

        public virtual Curriculum Curriculum { get; set; }
    }
}
