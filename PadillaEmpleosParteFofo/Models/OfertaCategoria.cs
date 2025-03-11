using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class OfertaCategoria
    {
        [Key]
        public int id_oferta_categoria { get; set; }
        [ForeignKey(nameof(CategoriaProfesional))]//No funciono de la otra forma el nommbre de la bd ha de estar malo
        public int id_categoria_profesional { get; set; }
        public int id_subcategoria_profesional { get; set; }

        public CategoriaProfesional CategoriaProfesional { get; set; }
    }
}
