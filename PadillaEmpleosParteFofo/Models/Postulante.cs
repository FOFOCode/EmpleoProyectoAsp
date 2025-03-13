using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadillaEmpleosParteFofo.Models
{
    public class Postulante
    {
        [Key]
        public int id_postulante { get; set; }

        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }

        public string nombre { get; set; }
        public string direccion { get; set; }
        public Usuario Usuario { get; set; }
    }
}
