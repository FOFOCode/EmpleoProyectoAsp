using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class FormacionAcademica
    {
        [Key]
        public int id_formacionacademica { get; set; }
        [ForeignKey("Curriculum")]
        public int id_curriculum { get; set; }
        [ForeignKey("Especialidad")]
        public int id_especialidad { get; set; }
        [ForeignKey("Titulo")]
        public int id_titulo { get; set; }
        [ForeignKey("Institucion")]
        public int id_institucion { get; set; }

        public virtual Curriculum Curriculum { get; set; }
        public virtual Especialidad Especialidad { get; set; }
        public virtual Titulo Titulo { get; set; }
        public virtual Institucion Institucion { get; set; }
    }
}
