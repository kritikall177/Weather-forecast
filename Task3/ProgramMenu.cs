using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    internal partial class Program
    {
        /// <summary>
        /// Основные константы и буффер ввиде словаря для хранения погоды. 
        /// </summary>
        private static ObservableDictionary<string, object[]>
            _dictionary = new ObservableDictionary<string, object[]>();

        private const string ApiKey = "253518f2407f40f18f7c08375b8d4d3e";
        private static readonly DateTime Unix = new DateTime(1970, 1, 1, 0, 0, 0);

        private static readonly Dictionary<string, string> WeatherIcons = new Dictionary<string, string>()
        {
            {"01d", "☀"},
            {"01n", "☽"},
            {"02d", "☁☀"},
            {"02n", "☁☽"},
            {"03d", "☁"},
            {"03n", "☁"},
            {"04d", "☁☁"},
            {"04n", "☁☁"},
            {"09d", "☁⛆☁"},
            {"09n", "☁⛆☁"},
            {"10d", "☀⛆☁"},
            {"10n", "☽⛆☁"},
            {"11d", "☁☁🗲"},
            {"11n", "☁☁🗲"},
            {"13d", "❆"},
            {"13n", "❆"},
            {"50d", "༄"},
            {"50n", "༄"}
        };

        private static readonly char[] LoadingArray = {'|', '/', '—', '\\'};

        /// <summary>
        /// Метод который возвращает класс с данными о местоположении города, название города передаётся в сам метод. 
        /// </summary>
        /// <param name="name">Название города</param>
        /// <returns>Класс содержащий все данные о геолокации города</returns>
        private static CitiesLocation ParsLocation(string name)
        {
            var jsonOnWeb = $"http://api.openweathermap.org/geo/1.0/direct?q={name}&limit=1&appid={ApiKey}";
            var citiesLocation = CitiesLocation.FromJson(WaitWhileParsing(jsonOnWeb));
            Console.WriteLine("Загрузка выполнена.");
            return citiesLocation[0];
        }

        /// <summary>
        /// Метод в котором мы указываем город в котором мы хотим узнать погоду.
        /// </summary>
        /// <returns>Возвращает данные о геолокации города</returns>
        private static CitiesLocation ChooseLocation()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ручной ввод (1)");
                Console.WriteLine("Выбрать город (2)");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Введите название города(eng):");
                        return ParsLocation(Console.ReadLine());

                    case "2":
                        return ChooseCities();
                }
            }
        }

        /// <summary>
        /// Метод в котором находиться выбор из заранее заготовленных городов.
        /// </summary>
        /// <returns>Возвращает данные о геолокации города</returns>
        private static CitiesLocation ChooseCities()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите город:");
                Console.WriteLine("Moscow (1)\n ");
                Console.WriteLine("Minsk (2)\n ");
                Console.WriteLine("London (3)\n ");
                Console.WriteLine("Beijing (4)\n ");
                Console.WriteLine("Paris (5)\n ");
                var city = (Cities)Convert.ToInt32(Console.ReadLine());
                var stringValue = city.ToString();
                if (stringValue.All(char.IsLetter))
                {
                    return ParsLocation(stringValue);
                }

                Console.WriteLine("Неправильный ввод:");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Метод который принимает ссылку и считывает json файл.
        /// </summary>
        /// <param name="link">Ссылка на прогноз погоды.</param>
        /// <returns>Json файл с результатом.</returns>
        /// <exception cref="InvalidOperationException">Вызов ошибки при не удачном ответе.</exception>
        private static string ParsString(string link)
        {

                var request = (HttpWebRequest) WebRequest.Create(link);
                var response = (HttpWebResponse) request.GetResponse();
                string responce;
                using (var streamReader =
                       new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    responce = streamReader.ReadToEnd();
                }
                link = responce;
                response.Close();
                
                return link;
        }

        /// <summary>
        /// Метод в котором мы проверяем есть ли данные в кеше, если нет, конвертируем json в объект, после чего выводим данные о погоде на данный момент.
        /// </summary>
        private static void ParseWeatherForecast()
        {
            try
            {
                var location = ChooseLocation();
                CurrentWeatherData currentWeatherData;
                if (_dictionary.ContainsKey(location.Name) && _dictionary[location.Name][0] != null)
                {
                    currentWeatherData = (CurrentWeatherData)_dictionary[location.Name][0];
                }
                else
                {
                    var jsonOnWeb = $"https://api.openweathermap.org/data/2.5/weather?lat={location.Lat}1&lon={location.Lon}&units=metric&appid={ApiKey}";
                    currentWeatherData = CurrentWeatherData.FromJson(WaitWhileParsing(jsonOnWeb));
                    Console.WriteLine("Загрузка выполнена.");
                    var name = currentWeatherData.Name;
                    name = name.Split(new char[] {' '})[0];
                    if (_dictionary.ContainsKey(name))
                    {
                        _dictionary.TryGetValue(name, out var buffer);
                        buffer[0] = currentWeatherData;
                        _dictionary[name] = buffer;
                    }
                    else
                    {
                        _dictionary.Add(name, new object[] {currentWeatherData, null});
                    }
                }

                Print(currentWeatherData);
            }
            catch (WebException ex)
            {
                Console.WriteLine("Ошибка: " + (int)((HttpWebResponse)ex.Response).StatusCode);
                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.Continue://(1xx)
                    case HttpStatusCode.SwitchingProtocols:
                    case HttpStatusCode.OK:
                        Console.WriteLine("Процесс обработки");
                        break;
                    
                    case HttpStatusCode.Created://(2xx)
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NonAuthoritativeInformation:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.ResetContent:
                    case HttpStatusCode.PartialContent:
                        Console.WriteLine("Успешное выполнение");
                        break;
                    
                    case HttpStatusCode.MultipleChoices://(3xx)
                    case HttpStatusCode.MovedPermanently:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.SeeOther:
                    case HttpStatusCode.NotModified:
                    case HttpStatusCode.UseProxy:
                    case HttpStatusCode.Unused:
                    case HttpStatusCode.TemporaryRedirect:
                        Console.WriteLine("Перенапровление");
                        break;
                    
                    case HttpStatusCode.BadRequest://(4xx)
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.PaymentRequired:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.MethodNotAllowed:
                    case HttpStatusCode.NotAcceptable:
                    case HttpStatusCode.ProxyAuthenticationRequired:
                    case HttpStatusCode.RequestTimeout:
                    case HttpStatusCode.Conflict:
                    case HttpStatusCode.Gone:
                    case HttpStatusCode.LengthRequired:
                    case HttpStatusCode.PreconditionFailed:
                    case HttpStatusCode.RequestEntityTooLarge:
                    case HttpStatusCode.RequestUriTooLong:
                    case HttpStatusCode.UnsupportedMediaType:
                    case HttpStatusCode.RequestedRangeNotSatisfiable:
                    case HttpStatusCode.ExpectationFailed:
                    case HttpStatusCode.UpgradeRequired:
                        Console.WriteLine("Ошибка клиента");
                        break;
                    
                    case HttpStatusCode.InternalServerError://(5xx)
                    case HttpStatusCode.NotImplemented:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                    case HttpStatusCode.HttpVersionNotSupported:
                        Console.WriteLine("Ошибка Сервера");
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.WriteLine("Подробная информация: " + ex.Message);
                Console.WriteLine("Нажмите любую кнопку:");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Метод в котором мы проверяем есть ли данные в кеше, если нет, конвертируем json в объект, после чего выводим данные о погоде на 5 дней.
        /// </summary>
        private static void ParseWeatherForecastOfFive()
        {
            try
            {
                var location = ChooseLocation();
                WeatherForecastFiveDays weatherForecastFiveDays;
                if (_dictionary.ContainsKey(location.Name) && _dictionary[location.Name][1] != null)
                {
                    weatherForecastFiveDays = (WeatherForecastFiveDays)_dictionary[location.Name][1];
                }
                else
                {
                    var jsonOnWeb = $"https://api.openweathermap.org/data/2.5/onecall?lat={location.Lat}&lon={location.Lon}&exclude=current,minutely,hourly&units=metric&appid={ApiKey}";
                    weatherForecastFiveDays = WeatherForecastFiveDays.FromJson(WaitWhileParsing(jsonOnWeb));
                    Console.WriteLine("Загрузка выполнена.");
                    var name = weatherForecastFiveDays.Timezone;
                    name = name.Split(new char[] {'/'})[1];
                    if (_dictionary.ContainsKey(name))
                    {
                        _dictionary.TryGetValue(name, out var buffer);
                        buffer[1] = weatherForecastFiveDays;
                        _dictionary[name] = buffer;
                    }
                    else
                    {
                        _dictionary.Add(name, new object[] {null, weatherForecastFiveDays});
                    }
                }

                Print(weatherForecastFiveDays);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Неполучилось отобразить запрашиваемый город."
                                  + "Возможные причины: \n" + 
                                  "* Неправильно указано название города\n"
                                  + "* Нет доступа к интернету\n"
                                  + "Подробнее ниже: \n"
                                  + ex.Message + "\nНажмите любую кнопку:");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Метод который получает объект с данными о погоде и выводит его на консоль.
        /// </summary>
        /// <param name="weatherForecastFiveDays">Передаём объект с погодой на 5 дней</param>
        private static void Print(WeatherForecastFiveDays weatherForecastFiveDays)
        {
            Console.Clear();
            var sb = new StringBuilder();
            var localData = DateTime.UtcNow.AddSeconds(weatherForecastFiveDays.TimezoneOffset);
            sb.Append(localData + "\n");
            sb.Append(weatherForecastFiveDays.Timezone + "\n");
            for (var i = 0; i < 5; i++)
            {
                sb.Append(CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat
                    .GetAbbreviatedDayName(localData.DayOfWeek) + ", ");
                sb.Append(CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat
                    .GetAbbreviatedMonthName(localData.Month));
                sb.Append($" {localData.Day}\t {WeatherIcons[weatherForecastFiveDays.Daily[i].Weather[0].Icon]}\t");
                sb.Append($"{(int)weatherForecastFiveDays.Daily[i].Temp.Max}/");
                sb.Append($"{(int)weatherForecastFiveDays.Daily[i].Temp.Min}°C \t");
                sb.Append($"{weatherForecastFiveDays.Daily[i].Weather[0].Description}\n");
            }

            sb.Append("Нажмите на кнопку что бы продолжить:");
            Console.WriteLine(sb);
            Console.ReadKey();
        }

        /// <summary>
        /// Метод который получает объект с данными о погоде и выводит его на консоль.
        /// </summary>
        /// <param name="currentWeatherData">Передаём объект с погодой на данный момент</param>
        private static void Print(CurrentWeatherData currentWeatherData)
        {
            Console.Clear();
            var sb = new StringBuilder();
            sb.Append($"{DateTime.UtcNow.AddSeconds(currentWeatherData.Timezone)}\n");
            sb.Append($"{currentWeatherData.Name}, {currentWeatherData.Sys.Country}\n");
            sb.Append($"{WeatherIcons[currentWeatherData.Weather[0].Icon]} ");
            sb.Append($"{(int)currentWeatherData.Main.Temp}°C\n");
            sb.Append($"Чувствуется как {(int)currentWeatherData.Main.FeelsLike}°C. ");
            sb.Append($"{currentWeatherData.Weather[0].Description}.\n");
            sb.Append($"Скорость ветра: {Math.Round(currentWeatherData.Wind.Speed, 2)} м/с\n");
            sb.Append($"Атмосферное давление: {currentWeatherData.Main.Pressure} hPa\n");
            sb.Append($"Влажность воздуха: {currentWeatherData.Main.Humidity} %\n");
            sb.Append($"Видимость: {currentWeatherData.Visibility/1000} км\n");
            sb.Append($"Восход солнца: {Unix.AddSeconds(currentWeatherData.Sys.Sunrise)}\n");
            sb.Append($"Закат солнца: {Unix.AddSeconds(currentWeatherData.Sys.Sunset)}\n");
            sb.Append("Нажмите на кнопку что бы продолжить:");
            Console.WriteLine(sb);
            Console.ReadKey();
        }

        /// <summary>
        /// Метод который выводит экран загрузки пока не будут получены данные с сервера.
        /// </summary>
        /// <param name="str">Ссылка на сервер.</param>
        /// <returns>Возвращаем json файл с данными.</returns>
        private static string WaitWhileParsing(string str)
        {
            ParsDelegate parsdelegate = ParsString;
            Console.WriteLine("Происходит загрузка данных...");
            var asyncResult = parsdelegate.BeginInvoke(str, null, null);
            return parsdelegate.EndInvoke(asyncResult);

        }
    }
}