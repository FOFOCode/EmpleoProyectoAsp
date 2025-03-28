using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class Titulo
    {
        [Key]
        public int id_titulo { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public string? tipo { get; set; }
    }
}
