using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HtmlToPdf
{
    public class ItemRepo
    {
        public static async Task<string> GetItemHtml(string[] itemIds)
        {
            var client = new HttpClient();
            var items = itemIds.Select(id =>
            {
                return new { response = "", id = id };
            });
            string json = JsonConvert.SerializeObject(new
            {
                items = items,
                accommodations = new string[] { }
            });
            StringContent body = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
            HttpResponseMessage response = await client.PostAsync("http://ivs.smarterbalanced.org/Pages/API/content/load", body);
            string message = await response.Content.ReadAsStringAsync();
            string html = ItemHtmlParser.GetHtml(fromString: message);
            return html;
        }
    }
}
