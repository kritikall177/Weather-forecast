using System;
using Newtonsoft.Json;

namespace Task3
{
    /// <summary>
    /// Класс в который мы преобразуем json файл с прогнозом погоды.
    /// </summary>
    [Serializable]
    public partial class CurrentWeatherData
    {
        [JsonProperty("coord")]
        public Coord Coord { get; set; }

        [JsonProperty("weather")]
        public Weather[] Weather { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("visibility")]
        public long Visibility { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("sys")]
        public Sys Sys { get; set; }

        [JsonProperty("timezone")]
        public long Timezone { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cod")]
        public long Cod { get; set; }
    }

    [Serializable]
    public class Clouds
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }

    [Serializable]
    public class Coord
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    [Serializable]
    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public long Pressure { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }
    }

    [Serializable]
    public class Sys
    {
        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }

    [Serializable]
    public class Weather
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    [Serializable]
    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public long Deg { get; set; }
    }

    public partial class CurrentWeatherData
    {
        /// <summary>
        /// Десириализация с json в объект
        /// </summary>
        /// <param name="json">json файл</param>
        /// <returns>объект с прогнозом погоды</returns>
        public static CurrentWeatherData FromJson(string json) => JsonConvert.DeserializeObject<CurrentWeatherData>(json, Task3.Converter.Settings);
    }
    
    /// <summary>
    /// Методы для перобразования объектов в json
    /// </summary>
    public static class Serialize
    {
        public static string ToJson(this WeatherForecastFiveDays self) => JsonConvert.SerializeObject(self, Task3.Converter.Settings);
        public static string ToJson(this CurrentWeatherData self) => JsonConvert.SerializeObject(self, Task3.Converter.Settings);
        public static string ToJson(this CitiesLocation[] self) => JsonConvert.SerializeObject(self, Task3.Converter.Settings);
    }
}
