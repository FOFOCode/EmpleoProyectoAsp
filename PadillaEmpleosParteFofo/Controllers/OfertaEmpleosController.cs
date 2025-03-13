using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: OfertaEmpleos
        public async Task<IActionResult> Index()
        {
            //var ofertas = await _context.OfertaEmpleo

            //    //o es una variable por eso => expresion lambda,hacen referencia a los objetos de los modelos de la clase(Punto 2 documento)

            //    .Include(o => o.Pais)           // Relación con la tabla Pais

            //    .Include(o => o.OfertaCategoria)// Relación con la tabla OfertaCategoria
            //        .ThenInclude(oc => oc.CategoriaProfesional)//Relacion oferta categoria a categoria profesional

            //    .Include(o => o.Empresa)        // Relación con la tabla Empresa
            //    .ToListAsync();

            //return View(ofertas);

            int idEmpresa = 1;

            if (idEmpresa == null)
            {
                return NotFound();
            }

            // Obtén todas las ofertas de empleo para la empresa con el id proporcionado
            var ofertas = await _context.OfertaEmpleo
                .Include(o => o.Pais)
                .Include(o => o.OfertaCategoria)
                    .ThenInclude(oc => oc.CategoriaProfesional)
                .Include(o => o.Empresa)
                .Where(o => o.id_empresa == idEmpresa)
                .ToListAsync(); // Esperar a que la tarea se complete y obtener la lista

            if (ofertas == null || !ofertas.Any()) // Si no hay ofertas para esa empresa
            {
                return NotFound(); // Devuelve una página 404 si no hay ofertas
            }

            return View(ofertas); // Pasamos la lista de ofertas a la vista
        }


        // GET: OfertaEmpleos/Details/5
        public async Task<IActionResult> Details()
        {
            int idEmpresa = 1;

            if (idEmpresa == null)
            {
                return NotFound();
            }

            // Obtén todas las ofertas de empleo para la empresa con el id proporcionado
            var ofertas = await _context.OfertaEmpleo
                .Include(o => o.Pais)
                .Include(o => o.OfertaCategoria)
                    .ThenInclude(oc => oc.CategoriaProfesional)
                .Include(o => o.Empresa)
                .Where(o => o.id_empresa == idEmpresa)
                .ToListAsync(); // Esperar a que la tarea se complete y obtener la lista

            if (ofertas == null || !ofertas.Any()) // Si no hay ofertas para esa empresa
            {
                return NotFound(); // Devuelve una página 404 si no hay ofertas
            }

            return View(ofertas); // Pasamos la lista de ofertas a la vista
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_oferta_empleo,id_pais,id_oferta_categoria,id_empresa,titulo,descripcion,vacantes,salario,horario,duracion_contrato,estado")] OfertaEmpleo ofertaEmpleo)
        {
            if (id != ofertaEmpleo.id_oferta_empleo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofertaEmpleo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfertaEmpleoExists(ofertaEmpleo.id_oferta_empleo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ofertaEmpleo);
        }

        // GET: OfertaEmpleos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofertaEmpleo = await _context.OfertaEmpleo
                .FirstOrDefaultAsync(m => m.id_oferta_empleo == id);
            if (ofertaEmpleo == null)
            {
                return NotFound();
            }

            return View(ofertaEmpleo);
        }

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

        private bool OfertaEmpleoExists(int id)
        {
            return _context.OfertaEmpleo.Any(e => e.id_oferta_empleo == id);
        }
    }
}
