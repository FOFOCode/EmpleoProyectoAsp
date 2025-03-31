﻿using Microsoft.AspNetCore.Mvc;
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
        //Grap 2 - Número de Inscripciones por Oferta
        public async Task<IActionResult> InscripcionesPorOferta()
        {
            try
            {
                var datos = await (from oferta in _context.OfertaEmpleo
                                   join inscripcion in _context.OfertaCandidatos
                                   on oferta.id_ofertaempleo equals inscripcion.id_ofertaempleo into inscripciones
                                   select new InscripcionesPorOfertaViewModel
                                   {
                                       Oferta = oferta.titulo,
                                       TotalInscripciones = inscripciones.Count()
                                   }).ToListAsync();

                System.Diagnostics.Debug.WriteLine($"Datos obtenidos (Inscripciones por Oferta): {Newtonsoft.Json.JsonConvert.SerializeObject(datos)}");

                if (!datos.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No se encontraron inscripciones para las ofertas.");
                }

                return View("~/Views/Gráficos/InscripcionesPorOferta.cshtml", datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        //Grap 7
        public async Task<IActionResult> OfertasPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var datos = await _context.OfertaEmpleo
                    .Where(o => o.fecha_publicacion >= fechaInicio && o.fecha_publicacion <= fechaFin)
                    .GroupBy(o => o.fecha_publicacion)
                    .Select(g => new
                    {
                        Fecha = g.Key,
                        TotalOfertas = g.Count()
                    })
                    .OrderBy(g => g.Fecha)
                    .ToListAsync();

                System.Diagnostics.Debug.WriteLine($"Datos obtenidos (Ofertas por Rango de Fechas): {Newtonsoft.Json.JsonConvert.SerializeObject(datos)}");

                if (!datos.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No se encontraron ofertas publicadas en el rango de fechas especificado.");
                }

                return View("~/Views/Gráficos/OfertasPorRangoFechas.cshtml", datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        //Grap 3
        public async Task<IActionResult> DistribucionCandidatosPorFormacionAcademica()
        {
            try
            {
                // Realizamos la consulta para obtener la distribución de candidatos por formación académica
                var datos = await (from fa in _context.FormacionAcademica
                                   join t in _context.Titulo on fa.id_titulo equals t.id_titulo
                                   group fa by t.tipo into g
                                   select new
                                   {
                                       NivelFormacion = g.Key,
                                       TotalCandidatos = g.Count()
                                   }).ToListAsync();

                // Log de los datos obtenidos
                System.Diagnostics.Debug.WriteLine($"Datos obtenidos (Distribución de Candidatos por Formación Académica): {Newtonsoft.Json.JsonConvert.SerializeObject(datos)}");

                // Retornamos los datos a la vista
                return View("~/Views/Gráficos/DistribucionCandidatosPorFormacionAcademica.cshtml", datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }



    }
}