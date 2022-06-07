using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task3
{
    /// <summary>
    /// Класс в который мы преобразуем json файл для получение местоположения города.
    /// </summary>
    public class CitiesLocation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("local_names")]
        public Dictionary<string, string> LocalNames { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
        
        public static CitiesLocation[] FromJson(string json) => JsonConvert.DeserializeObject<CitiesLocation[]>(json, Task3.Converter.Settings);
    }
    
    /// <summary>
    /// Класс конвертации 
    /// </summary>
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}