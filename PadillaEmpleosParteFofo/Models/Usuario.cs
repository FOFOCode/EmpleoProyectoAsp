using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }
        public string email { get; set; }
        public string contrasenia { get; set; }
        public string tipo_usuario { get; set; }
    }
}
