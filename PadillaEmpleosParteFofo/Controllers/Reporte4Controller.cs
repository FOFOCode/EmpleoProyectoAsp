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
    public class Reporte4Controller : Controller
    {
        private readonly empleoDBContext _context;

        public Reporte4Controller(empleoDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/Reportes/Reporte4.cshtml");
        }

        public IActionResult GenerarReporte()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                Document document = new Document(pdf);

                // Establecer márgenes de la página (opcional)
                document.SetMargins(40, 40, 40, 40); // Top, Left, Bottom, Right

                // Cargar fuentes personalizadas
                PdfFont boldFont = PdfFontFactory.CreateFont("Helvetica-Bold");
                PdfFont normalFont = PdfFontFactory.CreateFont("Helvetica");

                // Añadir título con fuente en negrita y color
                document.Add(new Paragraph("Reporte de Requisitos Más Frecuentes en las Ofertas de Empleo")
                    .SetFont(boldFont).SetFontSize(16).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLUE));
                document.Add(new Paragraph($"Generado el: {System.DateTime.Now}\n\n")
                    .SetFont(normalFont).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                // Obtener datos desde la base de datos
                var requisitos = (from ro in _context.RequisitoOferta
                                  join r in _context.Requisito on ro.id_requisito equals r.id_requisito
                                  group ro by ro.id_requisito into g
                                  select new
                                  {
                                      Requisito = g.FirstOrDefault().Requisito.nombre,  // Obtener el nombre del requisito
                                      Tipo = "Mínimo",  // Puedes cambiarlo si lo necesitas
                                      Frecuencia = g.Count(),
                                  })
                  .OrderByDescending(r => r.Frecuencia)
                  .ToList();

                // Crear tabla con bordes, colores de celdas y estilos
                Table table = new Table(3); // Tres columnas: Requisito, Tipo, Frecuencia

                // Definir anchos de las columnas
                table.AddCell(new Cell().Add(new Paragraph("Requisito").SetFont(boldFont).SetFontSize(12).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Tipo").SetFont(boldFont).SetFontSize(12).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Frecuencia").SetFont(boldFont).SetFontSize(12).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                // Añadir filas a la tabla con alternancia de colores
                bool isAlternatingColor = false;
                Color lightYellow = new DeviceRgb(255, 255, 153);  // Color amarillo claro

                foreach (var req in requisitos)
                {
                    var backgroundColor = isAlternatingColor ? lightYellow : iText.Kernel.Colors.ColorConstants.WHITE;
                    table.AddCell(new Cell().Add(new Paragraph(req.Requisito ?? "N/A").SetFont(normalFont).SetFontSize(10).SetBackgroundColor(backgroundColor).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(req.Tipo ?? "N/A").SetFont(normalFont).SetFontSize(10).SetBackgroundColor(backgroundColor).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(req.Frecuencia.ToString()).SetFont(normalFont).SetFontSize(10).SetBackgroundColor(backgroundColor).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                    // Alternar el color de las filas
                    isAlternatingColor = !isAlternatingColor;
                }

                // Centrar la tabla (añadir alineación horizontal)
                table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                // 50% ancho de la página

                // Añadir la tabla al documento
                document.Add(table);

                // Cerrar el documento
                document.Close();

                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "Reporte_Requisitos_Frecuentes.pdf");
            }
        }
    }
}
