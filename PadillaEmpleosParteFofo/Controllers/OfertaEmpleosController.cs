using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PadillaEmpleosParteFofo.Models;

namespace PadillaEmpleosParteFofo.Controllers
{
    public class OfertaEmpleosController : Controller
    {

        private readonly empleoDBContext _context;

        public OfertaEmpleosController(empleoDBContext context)
        {
            _context = context;
        }

        public IActionResult Grafico5()
        {   
            return View("~/Views/Gráficos/Grafico5.cshtml");
        }
        public IActionResult Grafico8()
        {
            return View("~/Views/Gráficos/Grafico8.cshtml");
        }

        //Metodo para obtener los datos para el grafico 5
        [HttpGet]
        public async Task<IActionResult> GetSalarioVacantesData()
        {
            try
            {
                // 1. Verificar conexión a la base de datos
                if (!_context.Database.CanConnect())
                {
                    return Json(new { error = "No se pudo conectar a la base de datos" });
                }

                // 2. Consulta con logging
                var query = _context.OfertaEmpleo
                    .Include(o => o.Empresa)
                    .Where(o => o.salario > 0 && o.vacantes > 0)
                    .Select(o => new
                    {
                        o.salario,
                        o.vacantes,
                        o.titulo,
                        Empresa = o.Empresa.nombre
                    });

                Console.WriteLine($"SQL generado: {query.ToQueryString()}");

                var datos = await query.ToListAsync();
                Console.WriteLine($"Registros encontrados: {datos.Count}");

                if (!datos.Any())
                {
                    return Json(new { warning = "Consulta exitosa pero no hay registros que cumplan los criterios" });
                }

                return Json(datos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completo: {ex.ToString()}");
                return Json(new
                {
                    error = "Error en el servidor",
                    details = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        //Metodo para obtener los datos para el gráfico 8
        [HttpGet]
        public async Task<IActionResult> GetOfertasPorPaisData()
        {
            try
            {
                // 1. Verificar conexión a la base de datos
                if (!_context.Database.CanConnect())
                {
                    return Json(new { error = "No se pudo conectar a la base de datos" });
                }

                // 2. Consulta para obtener el total de ofertas
                var totalOfertas = await _context.OfertaEmpleo.CountAsync();

                if (totalOfertas == 0)
                {
                    return Json(new { warning = "No hay ofertas de empleo registradas" });
                }

                // 3. Consulta para obtener ofertas por país
                var datos = await _context.OfertaEmpleo
                    .Include(o => o.Pais)
                    .GroupBy(o => o.Pais.nombre)
                    .Select(g => new
                    {
                        pais = g.Key,
                        cantidad = g.Count(),
                        porcentaje = (g.Count() * 100.0) / totalOfertas
                    })
                    .OrderByDescending(x => x.cantidad)
                    .ToListAsync();

                return Json(datos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completo: {ex}");
                return Json(new
                {
                    error = "Error en el servidor",
                    details = ex.Message
                });
            }
        }




        // GET: OfertaEmpleos
        //public async Task<IActionResult> Index()
        //{
        //    int idEmpresa = 1;

        //    if (idEmpresa == null)
        //    {
        //        return NotFound(); // Retorna 404 si no se proporciona un id de empresa
        //    }

        //    // Consulta las ofertas de empleo filtradas por la empresa y con sus relaciones necesarias
        //    var ofertas = await _context.OfertaEmpleo
        //        .Include(o => o.Pais)
        //        .Include(o => o.OfertaCategoria)
        //            .ThenInclude(oc => oc.CategoriaProfesional) // Relación con la categoría profesional
        //        .Include(o => o.Empresa)
        //        .Where(o => o.id_empresa == idEmpresa)
        //        .ToListAsync();

        //    if (ofertas == null || !ofertas.Any())
        //    {
        //        return NotFound(); // Retorna 404 si no hay ofertas para esa empresa
        //    }

        //    return View(ofertas);
        //}



        // GET: OfertaEmpleos/Details/5
        //public async Task<IActionResult> Details()
        //{
        //    int idEmpresa = 1;

        //    if (idEmpresa == null)
        //    {
        //        return NotFound();
        //    }

        //    // Obtén todas las ofertas de empleo para la empresa con el id proporcionado
        //    var ofertas = await _context.OfertaEmpleo
        //        .Include(o => o.Pais)
        //        .Include(o => o.OfertaCategoria)
        //            .ThenInclude(oc => oc.CategoriaProfesional)
        //        .Include(o => o.Empresa)
        //        .Where(o => o.id_empresa == idEmpresa)
        //        .ToListAsync(); // Esperar a que la tarea se complete y obtener la lista

        //    if (ofertas == null || !ofertas.Any()) // Si no hay ofertas para esa empresa
        //    {
        //        return NotFound(); // Devuelve una página 404 si no hay ofertas
        //    }

        //    return View(ofertas); // Pasamos la lista de ofertas a la vista
        //}

        // GET: OfertaEmpleos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfertaEmpleos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_oferta_empleo,id_pais,id_oferta_categoria,id_empresa,titulo,descripcion,vacantes,salario,horario,duracion_contrato,estado")] OfertaEmpleo ofertaEmpleo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofertaEmpleo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ofertaEmpleo);
        }

        // GET: OfertaEmpleos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofertaEmpleo = await _context.OfertaEmpleo.FindAsync(id);
            if (ofertaEmpleo == null)
            {
                return NotFound();
            }
            return View(ofertaEmpleo);
        }

        // POST: OfertaEmpleos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("id_oferta_empleo,id_pais,id_oferta_categoria,id_empresa,titulo,descripcion,vacantes,salario,horario,duracion_contrato,estado")] OfertaEmpleo ofertaEmpleo)
        //{
        //    if (id != ofertaEmpleo.id_oferta_empleo)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(ofertaEmpleo);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OfertaEmpleoExists(ofertaEmpleo.id_oferta_empleo))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(ofertaEmpleo);
        //}

        // GET: OfertaEmpleos/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ofertaEmpleo = await _context.OfertaEmpleo
        //        .FirstOrDefaultAsync(m => m.id_oferta_empleo == id);
        //    if (ofertaEmpleo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ofertaEmpleo);
        //}

        // POST: OfertaEmpleos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ofertaEmpleo = await _context.OfertaEmpleo.FindAsync(id);
            if (ofertaEmpleo != null)
            {
                _context.OfertaEmpleo.Remove(ofertaEmpleo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool OfertaEmpleoExists(int id)
        //{
        //    return _context.OfertaEmpleo.Any(e => e.id_oferta_empleo == id);
        //}
    }
}
