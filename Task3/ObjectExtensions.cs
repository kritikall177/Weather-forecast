using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Task3
{
    /// <summary>
    /// Расширения для object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Получение байтового представления объекта.
        /// </summary>
        /// <param name="data">Объект который преобразуется в байты.</param>
        /// <returns>Массив байтов.</returns>
        public static byte[] GetBytes(this object data)
        {
            byte[] bytes;
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, data);
                bytes = memoryStream.ToArray();
            }

            return bytes;
        }
        
        /// <summary>
        /// Преобразование массива байт в объект.
        /// </summary>
        /// <param name="bytes">Массив байт.</param>
        /// <returns>Объект.</returns>
        public static T SetBytes<T>(this byte[] bytes)
        {
            object obj;
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(bytes))
            {
                obj = formatter.Deserialize(memoryStream);
            }

            return obj is T tValue ? tValue : default;
        }

    }
}