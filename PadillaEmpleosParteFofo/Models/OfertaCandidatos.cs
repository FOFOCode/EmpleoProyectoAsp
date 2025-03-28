using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class OfertaCandidatos
    {
        [Key]
        public int id_ofertacandidatos { get; set; }
        [ForeignKey("OfertaEmpleo")]
        public int id_ofertaempleo { get; set; }
        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }

        public virtual OfertaEmpleo OfertaEmpleo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
