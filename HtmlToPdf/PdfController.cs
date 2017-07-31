using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf;
using DinkToPdf.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HtmlToPdf
{
    [Route("Pdf")]
    public class PdfController : Controller
    {
        private readonly IConverter converter;

        public PdfController(IConverter converter)
        {
            this.converter = converter;
        }

        [HttpGet]
        public FileContentResult Index()
        {
            var html = "<div>Hi</div>";
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };

            var pdf = converter.Convert(doc);
            return File(pdf, "application/pdf");
        }
    }
}
