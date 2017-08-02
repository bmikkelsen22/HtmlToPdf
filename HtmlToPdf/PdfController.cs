﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

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
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings()
                    {
                        Page = "http://sampleitems.smarterbalanced.org/BrowseItems/"
                    }
                }
            };

            var pdf = converter.Convert(doc);
            return File(pdf, "application/pdf");
        }

        [HttpGet("item")]
        public async Task<FileContentResult> Item(string[] id)
        {
            string message = await ItemRepo.GetItemHtml(id);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = message,
                        IncludeInOutline = true,
                        WebSettings = new WebSettings()
                        {
                            UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "ItemStyles.css"),
                            EnableIntelligentShrinking = false
                        }
                    }
                }
            };
            byte[] pdf = converter.Convert(doc);

            return File(pdf, "application/pdf");
        }

        [HttpGet("itemjs")]
        public async Task<FileContentResult> ItemJS(string[] id)
        {
            string message = await ItemRepo.GetItemHtmlJS(id);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = message,
                        IncludeInOutline = true,
                        WebSettings = new WebSettings()
                        {
                            UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "ItemStyles.css"),
                            EnableIntelligentShrinking = false,
                        },
                        LoadSettings = new LoadSettings()
                        {
                            JSDelay = 1000,
                            StopSlowScript = false
                        }
                    }
                }
            };
            byte[] pdf = converter.Convert(doc);
            return File(pdf, "application/pdf");
        }
    }
}
