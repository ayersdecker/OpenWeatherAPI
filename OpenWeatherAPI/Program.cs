using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace OpenWeatherAPI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            
            if (MenuSelection()) 
            {
                await OpenWeather(44, -73.6);
                //await InitiateCurrent();
                
            }
            else { InitiateOther(); }


        }
        private static bool MenuSelection()
        {
            while (true)
            {
                Console.WriteLine("--- Weather App ---");
                Console.WriteLine("\n1. Current Location\n2. Different City/State\n");
                Console.Write("Selection: ");
                if (int.TryParse(Console.ReadLine(), out int result) && result > 0 && result < 3) {
                    if (result == 1) { return true; } else return false; }
                else { Console.WriteLine("\n* Seleciton Error -- Try Again *\n"); }
            }
        }
        private static async Task InitiateCurrent()
        {
            // Collect Host IP Address
            

            // Assemble Request
            string url = "http://api.ipstack.com/check";
            string key = "?access_key=a86738f3a1015863fb9485f079e9e159";
            string requestCombo = url +  key;

            // New Request 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
            HttpResponseMessage response = await client.GetAsync(requestCombo);
            response.EnsureSuccessStatusCode();

            //JSON Fun Time
            string content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
            Console.WriteLine(requestCombo);
            if(result == null) { Console.WriteLine("Null Result"); }
            else 
            { 
                Console.WriteLine(result.latitude);
                Console.WriteLine(result.longitude);
                OpenWeather(result.latitude, result.longitude);
            }
            


        }
        private static void InitiateOther()
        {

        }
        private static async Task OpenWeather(double lat, double lng)
        {
            
            // Key and URL Build
            string key = "3101f82226ed18424cb3d3077d5ffd37";
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={key}";
            

            // Build and Connect to OpenWeather API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
            Console.WriteLine(content);
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine($"\nSky:{result.weather}\n\nTemp: {result.main.temp} K");
            Console.ResetColor();



        }

        
    }
}