using Microsoft.AspNetCore.Mvc;
using PadillaEmpleosParteFofo.Models;

namespace PadillaEmpleosParteFofo.Controllers
{
    public class NotificacionesController : Controller
    {
        private readonly empleoDBContext empleoDBContext;

        public NotificacionesController(empleoDBContext empleoDBContext)
        {
            this.empleoDBContext = empleoDBContext;
        }
        [HttpGet]
        public IActionResult Notificaciones() 
        {
            //int id_usuario = Convert.ToInt32(HttpContext.Session.GetInt32("id_usuario")); 
            var ofertas = (from sc in empleoDBContext.SuscripcionCategoria
                           join oc in empleoDBContext.OfertaCategoria
                           on new { sc.id_categoriaprofesional, sc.id_subcategoriaprofesional} equals new { oc.id_categoriaprofesional, oc.id_subcategoriaprofesional }
                           join o in empleoDBContext.OfertaEmpleo
                           on oc.id_ofertaempleo equals o.id_ofertaempleo
                           where o.estado == 'A' && o.fecha_publicacion >= DateTime.Now.AddYears(-4)
                           && sc.id_usuario == 1
                           select new
                           {
                               IdCateProf = sc.id_categoriaprofesional,
                               IdSubCateProf = sc.id_subcategoriaprofesional,
                               Titulo = o.titulo,
                               Fecha = o.fecha_publicacion,
                               Id = o.id_ofertaempleo
                           }).GroupBy(x => new { x.IdCateProf, x.IdSubCateProf, x.Titulo, x.Fecha, x.Id })
               .Select(g => g.Key) // Extrae solo las claves del grupo
               .ToList();

            ViewBag.Ofertas = ofertas;
            return View(); 
        }
    }
}
