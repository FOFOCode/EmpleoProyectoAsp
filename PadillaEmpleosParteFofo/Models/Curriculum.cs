using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class Curriculum
    {
        [Key]
        public int id_curriculum { get; set; }
        [ForeignKey("Postulante")]
        public int id_postulante { get; set; }
        public string? habilidades { get; set; }
        public string? idiomas { get; set; }
        public string? certificaciones { get; set; }

        public virtual Postulante Postulante { get; set; }
    }
}
