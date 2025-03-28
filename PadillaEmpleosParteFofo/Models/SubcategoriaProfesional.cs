using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class SubcategoriaProfesional
    {
        [Key]
        public int id_subcategoriaprofesional { get; set; }
        [ForeignKey("CategoriaProfesional")]
        public int id_categoriaprofesional { get; set; }
        public string? nombre { get; set; }

        public virtual CategoriaProfesional CategoriaProfesional { get; set; }
    }
}
