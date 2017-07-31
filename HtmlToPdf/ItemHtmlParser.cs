using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HtmlToPdf
{
    public class ItemHtmlParser
    {
        static public string GetHtml(string fromString)
        {
            XDocument xml = XDocument.Parse(fromString);
            XElement bodyEl = xml.Root.Element("content").Element("html");
            
            XElement metaEl = new XElement("meta");
            metaEl.SetAttributeValue("charset", "utf-8");

            XElement headEl = new XElement("head", content: metaEl);
            bodyEl.Name = "body";

            XElement htmlElement = new XElement("html", headEl, bodyEl);

            string html = htmlElement.ToString();
            return html;
        }
    }
}
