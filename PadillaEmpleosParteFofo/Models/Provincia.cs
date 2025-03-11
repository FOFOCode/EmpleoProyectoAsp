using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class Provincia
    {
        [Key]
        public int id_provincia { get; set; }
        public int id_pais { get; set; }
        public string nombre_provincia { get; set; }
    }
}
