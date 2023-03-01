using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using System.Net;

namespace OpenWeatherAPI
{
        internal class WebScrapping
    {
        public async Task WebScrap()
        {
            Console.Write("What is your Zip?: ");
            int.TryParse(Console.ReadLine(), out int zipOut);


            string customURL = $"https://www.zipinfo.com/cgi-local/zipsrch.exe?ll=ll&zip={zipOut}&Go=Go";
            //string customURL = $"https://www.google.com/maps/@{zipOut}";
            //string customURL = $"https://forecast.weather.gov/zipcity.php?inputstring={zipOut}";
            //string weatherUrl = "https://forecast.weather.gov/MapClick.php?lat=43.1&lon=-77.5#.Y9ADUcnMJy8";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");

            HttpResponseMessage response = await client.GetAsync(customURL);

            response.EnsureSuccessStatusCode();


            //string returnedURL = client.BaseAddress.ToString();


            /* int latStartIndex = returnedURL.IndexOf("@") + 1;
            int latEndIndex = returnedURL.IndexOf(",");
            string lat = returnedURL.Substring(latStartIndex, latEndIndex - latStartIndex);

            int lonStartIndex = latEndIndex + 1;
            int lonEndIndex = returnedURL.IndexOf("z");
            string lon = returnedURL.Substring(lonStartIndex, lonEndIndex - lonStartIndex); */

            string htmlF = await response.Content.ReadAsStringAsync();
            HtmlDocument parserF = new HtmlDocument();
            parserF.LoadHtml(htmlF);
            HtmlNode table = parserF.DocumentNode.DescendantNodes(3).ElementAt(3);
            HtmlNode tr2 = table.Descendants("tr").ElementAt(1);
            IEnumerable<HtmlNode> tr = table.Descendants("tr");
            double lat = Convert.ToDouble(tr2.Descendants("td").ElementAt(3).InnerText);
            double lon = Convert.ToDouble(tr2.Descendants("td").ElementAt(4).InnerText);


            string weatherURL = $"https://forecast.weather.gov/MapClick.php?lat={lat}&lon={lon}#.Y9KSz8nMLEY";

            response = await client.GetAsync(weatherURL);

            string html = await response.Content.ReadAsStringAsync();
            HtmlDocument parser = new HtmlDocument();
            parser.LoadHtml(html);

            string tempF = parser.DocumentNode.DescendantNodes().First(node => node.HasClass("myforecast-current-lrg")).InnerText;
            string tempC = parser.DocumentNode.DescendantNodes().First(node => node.HasClass("myforecast-current-sm")).InnerText;

            tempF = WebUtility.HtmlDecode(tempF);
            tempC = WebUtility.HtmlDecode(tempC);
            Console.WriteLine($"{tempF}/{tempC}");
        }
    }
   
}
