using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class CategoriaProfesional
    {
        [Key]
        public int id_categoria_profesional { get; set; }

        [Display(Name = "Categoria")]
        public string nombre_categoria_profesional { get; set; }
    }
}
