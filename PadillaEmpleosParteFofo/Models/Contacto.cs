using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class Contacto
    {
        [Key]
        public int id_contacto { get; set; }
        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
