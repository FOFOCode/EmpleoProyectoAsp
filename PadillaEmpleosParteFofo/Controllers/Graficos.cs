using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PadillaEmpleosParteFofo.Models;
using PadillaEmpleosParteFofo.Models.Graficos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadillaEmpleosParteFofo.Controllers
{
    public class Graficos : Controller
    {
        private readonly empleoDBContext _context;

        public Graficos(empleoDBContext context)
        {
            _context = context;
        }
        //Grap 4
        public async Task<IActionResult> PorcentajeCandidatosPorCategoria()
        {
            try
            {
                // Consulta en una línea para obtener los datos
                var datos = await _context.CategoriaProfesional
                    .GroupJoin(_context.SuscripcionCategoria, cp => cp.id_categoriaprofesional, sc => sc.id_categoriaprofesional, (cp, subs) => new SuscripcionCategoriaViewModel
                    {
                        Categoria = cp.nombre,
                        CantidadCandidatos = subs.Count(),
                        Porcentaje = _context.SuscripcionCategoria.Count() > 0 ? (subs.Count() * 100.0 / _context.SuscripcionCategoria.Count()) : 0
                    }).ToListAsync();

                // Depuración
                System.Diagnostics.Debug.WriteLine($"Datos obtenidos: {Newtonsoft.Json.JsonConvert.SerializeObject(datos)}");

                return View("~/Views/Gráficos/PorcentajeCandidatosPorCategoria.cshtml", datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        //Grap 1
        public async Task<IActionResult> VacantesPorEmpresa()
        {
            try
            {
                // Consulta para obtener el total de vacantes por empresa
                var datos = await (from e in _context.Empresa
                                   join oe in _context.OfertaEmpleo on e.id_empresa equals oe.id_empresa into ofertas
                                   from oferta in ofertas.DefaultIfEmpty()
                                   group oferta by e.nombre into g
                                   select new VacantesPorEmpresaViewModel
                                   {
                                       Empresa = g.Key,
                                       TotalVacantes = g.Sum(x => x != null ? x.vacantes : 0)
                                   }).ToListAsync();

                // Depuración
                System.Diagnostics.Debug.WriteLine($"Datos obtenidos (Vacantes por Empresa): {Newtonsoft.Json.JsonConvert.SerializeObject(datos)}");

                if (!datos.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No se encontraron datos para el gráfico de vacantes por empresa.");
                }

                return View("~/Views/Gráficos/VacantesPorEmpresa.cshtml", datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        //Grap 6
        public async Task<IActionResult> OfertasPorEmpresa()
        {
            try
            {
                
                var datos = await (from e in _context.Empresa
                                   join oe in _context.OfertaEmpleo on e.id_empresa equals oe.id_empresa into ofertas
                                   from oferta in ofertas.DefaultIfEmpty()
                                   group oferta by e.nombre into g
                                   select new OfertasPorEmpresaViewModel
                                   {
                                       Empresa = g.Key,
                                       TotalOfertas = g.Count(x => x != null)
                                   }).ToListAsync();

                
                System.Diagnostics.Debug.WriteLine($"Datos obtenidos (Ofertas por Empresa): {Newtonsoft.Json.JsonConvert.SerializeObject(datos)}");

                if (!datos.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No se encontraron datos para el gráfico de ofertas por empresa.");
                }

                return View("~/Views/Gráficos/OfertasPorEmpresa.cshtml", datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}