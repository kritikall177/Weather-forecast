using System;
using System.Collections;
using System.Collections.Generic;

namespace Task3
{
    /// <summary>
    /// Словарь при добавлении и удалении элементов которого вызывается событие сохранения данных в файл.
    /// </summary>
    /// <typeparam name="TKey">Ключ</typeparam>
    /// <typeparam name="TValue">Значение</typeparam>
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// События на которое подписываются другие методы.
        /// </summary>
        public static event SaveDelegate<IDictionary<TKey, TValue>> Event;

        /// <summary>
        /// Кеш ввиде словоря с прогнозом погоды.
        /// </summary>
        private IDictionary<TKey, TValue> _dictionaryImplementation;

        /// <summary>
        /// Конструктор словаря.
        /// </summary>
        public ObservableDictionary()
        {
            _dictionaryImplementation = new Dictionary<TKey, TValue>();
            Event += Storage.Save;
        }

        /// <summary>
        /// Возвращает енумератор, который выполняет итерацию по коллекции.
        /// </summary>
        /// <returns>Енумератор, который можно использовать для перебора коллекции.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionaryImplementation.GetEnumerator();
        }

        /// <summary>
        /// Возвращает енумератор, который выполняет итерацию по коллекции
        /// </summary>
        /// <returns>Объект IEnumerator, который можно использовать для перебора коллекции.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _dictionaryImplementation).GetEnumerator();
        }
        
        
        /// <summary>
        /// Загрузка элементов из файла в коллецию 
        /// </summary>
        public void LoadFile()
        {
            _dictionaryImplementation = Storage.Load<IDictionary<TKey, TValue>>();
        }

        /// <summary>
        /// Добавляет элемент в коллекцию, после чего вызывает евент.
        /// </summary>
        /// <param name="item"> объект для добавления в ICollection.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionaryImplementation.Add(item);
            Event?.Invoke(_dictionaryImplementation);
        }

        /// <summary>
        /// Удаляет все элементы из коллекции.
        /// </summary>
        public void Clear()
        {
            _dictionaryImplementation.Clear();
            
        }

        /// <summary>
        /// Определяет, содержит ли ICollection определенное значение.
        /// </summary>
        /// <param name="item">Объект, который нужно найти в коллекции.</param>
        /// <returns>значение true, если элемент найден в коллекции; в противном случае значение false.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionaryImplementation.Contains(item);
        }

        /// <summary>
        /// Копирует элементы ICollection в массив, начиная с определенного индекса массива.
        /// </summary>
        /// <param name="array">представляет собой одномерный массив, который является местом назначения элементов, скопированных из ICollection.</param>
        /// <param name="arrayIndex">Нулевой индекс в массиве, с которого начинается копирование.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionaryImplementation.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Удаляет первое вхождение определенного объекта из коллекции, после чего вызывает евент.
        /// </summary>
        /// <param name="item">Объект, который нужно удалить из коллекции.</param>
        /// <returns>значение true, если элемент был успешно удален из коллекции; в противном случае значение false. Этот метод также возвращает значение false, если элемент не найден в исходной коллекции.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dictionaryImplementation.Remove(item))
            {
                Event?.Invoke(_dictionaryImplementation);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получает количество элементов.
        /// </summary>
        public int Count => _dictionaryImplementation.Count;

        /// <summary>
        /// Возвращает значение, указывающее, доступен ли ICollection только для чтения.
        /// </summary>
        public bool IsReadOnly => _dictionaryImplementation.IsReadOnly;

        /// <summary>
        /// Определяет, содержит ли идентификатор элемент с указанным ключом.
        /// </summary>
        /// <param name="key">ключ, который нужно найти в идентификаторе.</param>
        /// <returns>true, если идентификатор содержит элемент с ключом; в противном случае значение false.</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionaryImplementation.ContainsKey(key);
        }

        /// <summary>
        /// Добавляет элемент с предоставленными ключом и значением в идентификатор,после чего вызывает евент.
        /// </summary>
        /// <param name="key">объект, который будет использоваться в качестве ключа добавляемого элемента.</param>
        /// <param name="value">ообъект, который будет использоваться в качестве значения добавляемого элемента.</param>
        public void Add(TKey key, TValue value)
        {
            _dictionaryImplementation.Add(key, value);
            Event?.Invoke(_dictionaryImplementation);
        }

        /// <summary>
        /// Удаляет элемент с указанным ключом, после чего вызывает евент.
        /// </summary>
        /// <param name="key">Ключ элемента для удаления.</param>
        /// <returns>значение true, если элемент успешно удален; в противном случае значение false. Этот метод также возвращает значение false, если ключ не был найден в исходном словаре.</returns>
        public bool Remove(TKey key)
        {
            if (_dictionaryImplementation.Remove(key))
            {
                Event?.Invoke(_dictionaryImplementation);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Возвращает значение, связанное с указанным ключом.
        /// </summary>
        /// <param name="key">ключ, значение которого нужно получить.</param>
        /// <param name="value">Когда этот метод возвращает значение, связанное с указанным ключом, если ключ найден;
        /// в противном случае значение по умолчанию для типа параметра value. Этот параметр передается неинициализированным.</param>
        /// <returns>true, если объект, реализующий IDictionary, содержит элемент с указанным ключом;
        /// в противном случае значение false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionaryImplementation.TryGetValue(key, out value);
        }

        /// <summary>
        /// Обращение к элементы словоря через ключ.
        /// </summary>
        /// <param name="key">Ключ.</param>
        public TValue this[TKey key]
        {
            get => _dictionaryImplementation[key];
            set
            {
                _dictionaryImplementation[key] = value;
                Event?.Invoke(_dictionaryImplementation);
            }
        }

        /// <summary>
        /// Возвращает все ключи.
        /// </summary>
        public ICollection<TKey> Keys => _dictionaryImplementation.Keys;

        /// <summary>
        /// Возвращает все значения.
        /// </summary>
        public ICollection<TValue> Values => _dictionaryImplementation.Values;
    }
}