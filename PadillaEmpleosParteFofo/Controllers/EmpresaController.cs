using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PadillaEmpleosParteFofo.Models;

namespace PadillaEmpleosParteFofo.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly empleoDBContext _context;

        public EmpresaController(empleoDBContext context)
        {
            _context = context;
        }
        

    }
}
