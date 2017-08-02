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

            XElement headEl = new XElement("head", metaEl);
            XElement bodyEl = new XElement("body", XElement.Parse(htmlString));

            RemoveHyperlinks(bodyEl);
            MakeLinksExternal(bodyEl);

            XElement htmlElement = new XElement("html", headEl, bodyEl);

            string html = htmlElement.ToString();
            return html;
        }

        static private void MakeLinksExternal(XElement bodyElement)
        {
            var sources = bodyElement.Descendants().Where(e =>
            {
                if (e.Attribute("src") != null && e.Attribute("src").Value != null)
                {
                    return e.Attribute("src").Value.StartsWith("/");
                }
                return false;
            });
            foreach (XElement sourceElement in sources)
            {
                string src = "http://ivs.smarterbalanced.org" + sourceElement.Attribute("src").Value;
                sourceElement.SetAttributeValue("src", src);
            }
        }

        static private void RemoveHyperlinks(XElement bodyElement)
        {
            bodyElement.Descendants().Where(e => e.Name.LocalName == "a").Remove();
        }
    }
}
