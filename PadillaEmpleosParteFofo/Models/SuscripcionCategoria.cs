using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class SuscripcionCategoria
    {
        [Key]
        public int id_suscripcioncategoria { get; set; }

        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }

        [ForeignKey("CategoriaProfesional")]
        public int id_categoriaprofesional { get; set; }

        [ForeignKey("SubcategoriaProfesional")]
        public int id_subcategoriaprofesional { get; set; }


        public virtual Usuario Usuario { get; set; }
        public virtual CategoriaProfesional CategoriaProfesional { get; set; }
        public virtual SubcategoriaProfesional SubcategoriaProfesional { get; set; }
    }
}
