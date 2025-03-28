using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using PadillaEmpleosParteFofo.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using System.IO;
using System.Linq;
using iText.Kernel.Colors;

namespace PadillaEmpleosParteFofo.Controllers
{
    public class Reporte5Controller : Controller
    {
        private readonly empleoDBContext _context;

        public Reporte5Controller(empleoDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/Reportes/Reporte5.cshtml");
        }

        public IActionResult GenerarReporte()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                Document document = new Document(pdf);

                // Establecer márgenes de la página
                document.SetMargins(40, 40, 40, 40);

                // Cargar fuentes personalizadas
                PdfFont boldFont = PdfFontFactory.CreateFont("Helvetica-Bold");
                PdfFont normalFont = PdfFontFactory.CreateFont("Helvetica");

                // Añadir título
                document.Add(new Paragraph("Reporte de Suscripciones a Categorías Profesionales")
                    .SetFont(boldFont).SetFontSize(16).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLUE));
                document.Add(new Paragraph($"Generado el: {System.DateTime.Now}\n\n")
                    .SetFont(normalFont).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                // Obtener datos desde la base de datos
                var categorias = (from c in _context.CategoriaProfesional
                                  join s in _context.SuscripcionCategoria on c.id_categoriaprofesional equals s.id_categoriaprofesional into subs
                                  from sus in subs.DefaultIfEmpty()
                                  group sus by c into g
                                  select new
                                  {
                                      ID_Categoria = g.Key.id_categoriaprofesional,
                                      Nombre_Categoria = g.Key.nombre,
                                      Cantidad_Suscriptores = g.Count(s => s != null)
                                  })
                                  .OrderByDescending(c => c.Cantidad_Suscriptores)
                                  .ToList();

                // Crear tabla
                Table table = new Table(3);

                // Encabezados de tabla
                table.AddCell(new Cell().Add(new Paragraph("ID Categoría").SetFont(boldFont).SetFontSize(12).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Nombre Categoría").SetFont(boldFont).SetFontSize(12).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Cantidad Suscriptores").SetFont(boldFont).SetFontSize(12).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                // Añadir datos a la tabla con alternancia de colores
                bool isAlternatingColor = false;
                Color lightYellow = new DeviceRgb(255, 255, 153);

                foreach (var cat in categorias)
                {
                    var backgroundColor = isAlternatingColor ? lightYellow : iText.Kernel.Colors.ColorConstants.WHITE;
                    table.AddCell(new Cell().Add(new Paragraph(cat.ID_Categoria.ToString()).SetFont(normalFont).SetFontSize(10).SetBackgroundColor(backgroundColor).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(cat.Nombre_Categoria ?? "N/A").SetFont(normalFont).SetFontSize(10).SetBackgroundColor(backgroundColor).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(cat.Cantidad_Suscriptores.ToString()).SetFont(normalFont).SetFontSize(10).SetBackgroundColor(backgroundColor).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                    isAlternatingColor = !isAlternatingColor;
                }

                // Ajustar alineación de tabla
                table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                // Añadir tabla al documento
                document.Add(table);

                // Cerrar documento
                document.Close();

                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "Reporte_Suscripciones_Categorias.pdf");
            }
        }

    }
}
