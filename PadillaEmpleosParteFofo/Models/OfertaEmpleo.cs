using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class OfertaEmpleo
    {
        [Key]
        public int id_oferta_empleo { get; set; }

        [ForeignKey("Pais")]
        [Display(Name = "Pais")]
        public int id_pais { get; set; }

        [ForeignKey("OfertaCategoria")]
        [Display(Name = "Categoria")]
        public int id_oferta_categoria { get; set; }

        [ForeignKey("Empresa")]
        [Display(Name = "Empresa")]
        public int id_empresa { get; set; }

        public string titulo { get; set; }

        public string descripcion { get; set; }

        public int vacantes { get; set; }

        public double salario { get; set; }

        public string horario { get; set; }

        public string duracion_contrato { get; set; }

        public string estado { get; set; }


        // Relación con otras tablas
        public Pais Pais { get; set; }
        public OfertaCategoria OfertaCategoria { get; set; }
        public Empresa Empresa { get; set; }
    }
}
