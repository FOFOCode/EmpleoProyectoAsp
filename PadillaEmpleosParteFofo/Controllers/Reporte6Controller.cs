using Microsoft.AspNetCore.Mvc;
using PadillaEmpleosParteFofo.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using System.IO;
using System.Linq;
using iText.Kernel.Colors;
using iText.Layout.Properties;

namespace PadillaEmpleosParteFofo.Controllers
{
    public class Reporte6Controller : Controller
    {
        private readonly empleoDBContext _context;

        public Reporte6Controller(empleoDBContext context)
        {
            _context = context;
        }

        public IActionResult Reporte6()
        {
            return View("~/Views/Reportes/Reporte6.cshtml");
        }

        public IActionResult GenerarReporte()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Configuración del documento
                document.SetMargins(40, 40, 40, 40);
                PdfFont boldFont = PdfFontFactory.CreateFont("Helvetica-Bold");
                PdfFont normalFont = PdfFontFactory.CreateFont("Helvetica");

                // Título del reporte
                document.Add(new Paragraph("Reporte de Empresas Registradas en el Sistema")
                    .SetFont(boldFont).SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.BLUE));

                document.Add(new Paragraph($"Generado el: {System.DateTime.Now}\n\n")
                    .SetFont(normalFont).SetFontSize(12)
                    .SetTextAlignment(TextAlignment.RIGHT));

                // Obtener datos de empresas
                var empresas = _context.Empresa
                    .Join(_context.Usuario,
                        e => e.id_usuario,
                        u => u.id_usuario,
                        (e, u) => new { Empresa = e, Usuario = u })
                    .GroupJoin(_context.Contacto,
                        eu => eu.Usuario.id_usuario,
                        c => c.id_usuario,
                        (eu, contactos) => new { eu.Empresa, Contactos = contactos })
                    .AsEnumerable()
                    .Select(euc => new
                    {
                        IdEmpresa = euc.Empresa.id_empresa,
                        Nombre = euc.Empresa.nombre,
                        Telefonos = string.Join(", ", euc.Contactos.Select(c => c.telefono).Distinct()),
                        Emails = string.Join(", ", euc.Contactos.Select(c => c.email).Distinct())
                    })
                    .OrderBy(e => e.Nombre)
                    .ToList();

                // Crear tabla
                Table table = new Table(4);
                table.SetWidth(UnitValue.CreatePercentValue(100));

                // Encabezados de tabla
                Cell headerCellId = new Cell();
                headerCellId.Add(new Paragraph("ID"));
                headerCellId.SetFont(boldFont);
                headerCellId.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                headerCellId.SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(headerCellId);

                Cell headerCellEmpresa = new Cell();
                headerCellEmpresa.Add(new Paragraph("Empresa"));
                headerCellEmpresa.SetFont(boldFont);
                headerCellEmpresa.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                headerCellEmpresa.SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(headerCellEmpresa);

                Cell headerCellTelefonos = new Cell();
                headerCellTelefonos.Add(new Paragraph("Teléfonos"));
                headerCellTelefonos.SetFont(boldFont);
                headerCellTelefonos.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                headerCellTelefonos.SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(headerCellTelefonos);

                Cell headerCellEmails = new Cell();
                headerCellEmails.Add(new Paragraph("Emails"));
                headerCellEmails.SetFont(boldFont);
                headerCellEmails.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                headerCellEmails.SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(headerCellEmails);

                // Filas de datos
                bool alternate = false;
                Color lightYellow = new DeviceRgb(255, 255, 204);

                foreach (var emp in empresas)
                {
                    var bgColor = alternate ? lightYellow : ColorConstants.WHITE;

                    table.AddCell(new Cell().Add(new Paragraph(emp.IdEmpresa.ToString()))
                        .SetBackgroundColor(bgColor)
                        .SetTextAlignment(TextAlignment.CENTER));

                    table.AddCell(new Cell().Add(new Paragraph(emp.Nombre))
                        .SetBackgroundColor(bgColor));

                    table.AddCell(new Cell().Add(new Paragraph(emp.Telefonos ?? "N/A"))
                        .SetBackgroundColor(bgColor));

                    table.AddCell(new Cell().Add(new Paragraph(emp.Emails ?? "N/A"))
                        .SetBackgroundColor(bgColor));

                    alternate = !alternate;
                }

                document.Add(table);
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", "Reporte_Empresas_Registradas.pdf");
            }
        }
    }
}