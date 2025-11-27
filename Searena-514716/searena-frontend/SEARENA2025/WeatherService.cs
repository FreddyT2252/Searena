using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SEARENA2025
{
    public class WeatherInfo
    {
        public double TemperatureC { get; set; }
        public double WindSpeedKmh { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; }
    }

    internal static class WeatherService
    {
        private static readonly HttpClient http = new HttpClient();

        private const string ApiKey = "54d781e35ad94d948d732245252309";
        private const string BaseUrl = "https://api.weatherapi.com/v1/current.json";

        public static async Task<WeatherInfo> GetCurrentAsync(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Lokasi kosong");

            // contoh: "Waisai" atau "Raja Ampat"
            string url = string.Format(
                "{0}?key={1}&q={2}&aqi=no",
                BaseUrl,
                ApiKey,
                Uri.EscapeDataString(location));

            string json = await http.GetStringAsync(url);

            using (var doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;

                // Kalau gagal, WeatherAPI kasih field "error"
                JsonElement errorElem;
                if (root.TryGetProperty("error", out errorElem))
                {
                    string msg = errorElem.GetProperty("message").GetString();
                    throw new Exception("WeatherAPI error: " + msg);
                }

                JsonElement current = root.GetProperty("current");

                double tempC = current.GetProperty("temp_c").GetDouble();
                double windKph = current.GetProperty("wind_kph").GetDouble();
                int humidity = current.GetProperty("humidity").GetInt32();
                string cond = current.GetProperty("condition")
                                        .GetProperty("text").GetString();

                return new WeatherInfo
                {
                    TemperatureC = tempC,
                    WindSpeedKmh = windKph,
                    Humidity = humidity,
                    Description = cond
                };
            }
        }
    }
}
