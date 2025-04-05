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
                               sc.id_categoriaprofesional,
                               sc.id_subcategoriaprofesional,
                               o.titulo,
                               o.fecha_publicacion,
                               o.id_ofertaempleo
                           }).GroupBy(x => new { x.id_categoriaprofesional, x.id_subcategoriaprofesional,x.titulo, x.fecha_publicacion, x.id_ofertaempleo })
                           .ToList();

            ViewBag.Ofertas = ofertas;
            return View(); 
        }
    }
}
