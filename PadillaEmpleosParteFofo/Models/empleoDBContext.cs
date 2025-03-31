using Microsoft.EntityFrameworkCore;

namespace PadillaEmpleosParteFofo.Models
{
    public class empleoDBContext: DbContext
    {
        public empleoDBContext(DbContextOptions<empleoDBContext> options) : base(options)
        {
        }

        public DbSet<OfertaEmpleo> OfertaEmpleo { get; set; }
        public DbSet<SubcategoriaProfesional> SubcategoriaProfesional { get; set; }
        public DbSet<CategoriaProfesional> CategoriaProfesional { get; set; }
        public DbSet<OfertaCategoria> OfertaCategoria { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Provincia> Provincia { get; set; }
        public DbSet<RequisitoOferta> RequisitoOferta { get; set; }
        public DbSet<Requisito> Requisito { get; set; }
        public DbSet<SuscripcionCategoria> SuscripcionCategoria{ get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Contacto> Contacto { get; set; }


    }


}
