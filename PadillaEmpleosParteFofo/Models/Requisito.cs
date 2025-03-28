﻿using System.ComponentModel.DataAnnotations;

namespace PadillaEmpleosParteFofo.Models
{
    public class Requisito
    {
        [Key]
        public int id_requisito { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
    }
}
