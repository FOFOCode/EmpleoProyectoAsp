using PadillaEmpleosParteFofo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OfertaCategoria
{
    [Key]
    public int id_oferta_categoria { get; set; }

    [ForeignKey("OfertaEmpleo")]
    public int id_oferta_empleo { get; set; } // <-- Corrige el nombre para coincidir con `OfertaEmpleo`

    [ForeignKey("CategoriaProfesional")]
    public int id_categoria_profesional { get; set; }
    public int id_subcategoria_profesional { get; set; }

    public CategoriaProfesional CategoriaProfesional { get; set; }
    public OfertaEmpleo OfertaEmpleo { get; set; }
}
