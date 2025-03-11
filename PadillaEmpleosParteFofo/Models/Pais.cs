using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class Pais
    {
        [Key]
        public int id_pais { get; set; }
        public string nombre_pais { get; set; }
    }
}
