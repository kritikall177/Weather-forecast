using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    /// <summary>
    /// Делегаты для вывода меню загрузки и сохранения кеша. 
    /// </summary>
    delegate string ParsDelegate(string link);
    public delegate void SaveDelegate<in T>(T obj);
    
    /// <summary>
    /// Основной класс в котором выполняется вся программа.
    /// </summary>
    internal partial class Program
    {
        /// <summary>
        /// Основной метод в котором находиться меню программы.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Текущий прогноз погоды (1)");
                Console.WriteLine("Прогноз погоды на 5 дней (2)");
                Console.WriteLine("Загрузка кеша с прошлого включения (3)");
                Console.WriteLine("Очистка кеша (4)");
                Console.WriteLine("Выход (e)");
                switch (Console.ReadLine())
                {
                    case "1":
                        ParseWeatherForecast();
                        break;

                    case "2":
                        ParseWeatherForecastOfFive();
                        break;
                    
                    case "3":
                        _dictionary.LoadFile();
                        break;
                    
                    case "4":
                        _dictionary.Clear();
                        break;

                    case "e":
                        return;
                }
            }
        }
    }
}