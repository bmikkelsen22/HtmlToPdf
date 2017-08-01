using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            XElement xmlHtmlElement = xml.Root.Element("content").Element("html");
            var cData = (XCData)xmlHtmlElement.DescendantNodes().Where(n => n.NodeType == XmlNodeType.CDATA).FirstOrDefault();
            var htmlString = cData.Value;

            XElement metaEl = new XElement("meta");
            metaEl.SetAttributeValue("charset", "utf-8");

            XElement linkEl = new XElement("link");
            linkEl.SetAttributeValue("rel", "stylesheet");
            linkEl.SetAttributeValue("href", "http://ivs.smarterbalanced.org/irisstyles/universal/items.css");

            XElement headEl = new XElement("head", metaEl, linkEl);
            XElement bodyEl = new XElement("body", XElement.Parse(htmlString));
            bodyEl.Descendants().Where(e => e.Name.LocalName == "a").Remove();

            XElement htmlElement = new XElement("html", headEl, bodyEl);

            string html = htmlElement.ToString();
            return html;
        }
    }
}
