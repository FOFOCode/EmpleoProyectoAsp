using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class SubcategoriaProfesional
    {
        [Key]
        public int id_subcategoria_profesional { get; set; }
        public string nombre_subcategoria_profesional { get; set; }
    }
}
