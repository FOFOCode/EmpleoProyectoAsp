using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class CategoriaProfesional
    {
        [Key]
        public int id_categoriaprofesional { get; set; }

        [Display(Name = "Categoria")]
        public string nombre { get; set; }
    }
}
