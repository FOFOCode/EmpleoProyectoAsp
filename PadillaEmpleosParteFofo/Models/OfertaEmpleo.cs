using PadillaEmpleosParteFofo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OfertaEmpleo
{
    [Key]
    public int id_oferta_empleo { get; set; }

    [ForeignKey("Pais")]
    public int id_pais { get; set; }

    [ForeignKey("Empresa")]
    public int id_empresa { get; set; }

    [ForeignKey("OfertaCategoria")]  // <-- Agregar esto
    public int id_oferta_categoria { get; set; } // <-- Asegúrate de que exista en la BD

    public string titulo { get; set; }
    public string descripcion { get; set; }
    public int vacantes { get; set; }
    public double salario { get; set; }
    public string horario { get; set; }
    public string duracion_contrato { get; set; }
    public string estado { get; set; }

    public Pais Pais { get; set; }
    public OfertaCategoria OfertaCategoria { get; set; }
    public Empresa Empresa { get; set; }
}
