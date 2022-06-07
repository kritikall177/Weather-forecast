using System;
using System.IO;

namespace Task3
{
    /// <summary>
    /// Класс для работы с файлами.
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// Путь по умолчанию где создаётся файл.
        /// </summary>
        private const string FilePath = "try.txt";
        
        /// <summary>
        /// Метод который сохраняет объект в текстовый файл ввиде массива байт.
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        public static void Save<T>(T data)
        {
            var bytes = data.GetBytes();
            using (var fs = new FileStream(FilePath, FileMode.Create))
            {
               fs.Write(bytes, 0, bytes.Length);
               Console.WriteLine("Текст записан в файл");
            }
        }

        /// <summary>
        /// Метод который создаёт объект из массива байт в  текстовом файле.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>()
        {
            byte[] bytes;
            using (var fs = File.OpenRead(FilePath))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                Console.WriteLine("Текст считан с файла");
            }
            
            return bytes.SetBytes<T>();
        }
    }
}