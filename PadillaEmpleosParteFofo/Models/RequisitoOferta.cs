using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class RequisitoOferta
    {
        [Key]
        public int id_requisitooferta { get; set; }
        [ForeignKey("OfertaEmpleo")]
        public int id_ofertaempleo { get; set; }
        [ForeignKey("Requisito")]
        public int id_requisito { get; set; }


        // Relación con el modelo de Oferta (Si existe)
        public virtual OfertaEmpleo OfertaEmpleo { get; set; }

        // Relación con el modelo de Requisito (Si existe)
        public virtual Requisito Requisito { get; set; }
        public int? Frecuencia { get; internal set; }
    }
}
