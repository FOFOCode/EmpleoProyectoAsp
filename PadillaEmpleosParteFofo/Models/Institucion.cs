using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class Institucion
    {
        [Key]
        public int id_institucion { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
    }
}
