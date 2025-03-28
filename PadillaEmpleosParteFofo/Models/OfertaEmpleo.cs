using PadillaEmpleosParteFofo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PadillaEmpleosParteFofo.Models { 

    public class OfertaEmpleo
    {
        [Key]
        public int id_ofertaempleo { get; set; }

        [ForeignKey("Pais")]
        public int id_pais { get; set; }

        [ForeignKey("Provincia")]
        public int id_provincia { get; set; }

        [ForeignKey("Empresa")]
        public int id_empresa { get; set; }

        public string titulo { get; set; }

        public string descripcion { get; set; }

        public int vacantes { get; set; }

        public double salario { get; set; }

        public string horario { get; set; }

        public string duracion_contrato { get; set; }

        public DateTime fecha_publicacion { get; set; }

        public string estado { get; set; }

        public virtual Pais Pais { get; set; }
        public virtual Empresa Empresa { get; set; }

        public virtual Provincia Provincia { get; set; }
    }
}
