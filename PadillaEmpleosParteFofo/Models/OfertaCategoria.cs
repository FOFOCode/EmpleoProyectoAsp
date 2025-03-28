using PadillaEmpleosParteFofo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models { 

    public class OfertaCategoria
    {
        [Key]
        public int id_ofertacategoria { get; set; }

        [ForeignKey("OfertaEmpleo")]
        public int id_ofertaempleo { get; set; } 

        [ForeignKey("CategoriaProfesional")]
        public int id_categoriaprofesional { get; set; }
        public int id_subcategoriaprofesional { get; set; }

        public virtual CategoriaProfesional CategoriaProfesional { get; set; }
        public virtual OfertaEmpleo OfertaEmpleo { get; set; }
        public virtual SubcategoriaProfesional SubCategoriaProfesional { get; set; }
    }
}
