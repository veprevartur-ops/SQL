using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Универсальный стек с дополнительными возможностями.
/// </summary>
/// <typeparam name="T">Тип элементов стека.</typeparam>
public class SmartStack<T> : IEnumerable<T>
{
    // Внутренний массив для хранения элементов
    private T[] _array;

    // Количество элементов в стеке
    private int _count;

    /// <summary>
    /// Конструктор пустого стека с начальной ёмкостью 4.
    /// </summary>
    public SmartStack()
    {
        _array = new T[4];
        _count = 0;
    }

    /// <summary>
    /// Конструктор пустого стека с заданной ёмкостью.
    /// </summary>
    /// <param name="capacity">Начальная ёмкость стека.</param>
    /// <exception cref="ArgumentException">Если ёмкость не положительна.</exception>
    public SmartStack(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Ёмкость должна быть положительной.");
        _array = new T[capacity];
        _count = 0;
    }

    /// <summary>
    /// Конструктор стека из коллекции.
    /// </summary>
    /// <param name="collection">Исходная коллекция.</param>
    /// <exception cref="ArgumentNullException">Если коллекция равна null.</exception>
    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));
        _array = collection.ToArray();
        _count = _array.Length;
    }

    /// <summary>
    /// Метод добавления элемента на вершину стека.
    /// </summary>
    /// <param name="item">Добавляемый элемент.</param>
    public void Push(T item)
    {
        if (_count == _array.Length)
            Array.Resize(ref _array, _array.Length == 0 ? 4 : _array.Length * 2);
        _array[_count++] = item;
    }

    /// <summary>
    /// Метод добавления коллекции элементов на вершину стека (в обратном порядке).
    /// </summary>
    /// <param name="collection">Коллекция элементов.</param>
    /// <exception cref="ArgumentNullException">Если коллекция равна null.</exception>
    public void PushRange(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));
        foreach (var item in collection.Reverse())
            Push(item);
    }

    /// <summary>
    /// Метод удаления и возврата элемента с вершины стека.
    /// </summary>
    /// <returns>Элемент с вершины стека.</returns>
    /// <exception cref="InvalidOperationException">Если стек пуст.</exception>
    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пуст.");
        return _array[--_count];
    }

    /// <summary>
    /// Метод возврата элемента с вершины стека без удаления.
    /// </summary>
    /// <returns>Элемент с вершины стека.</returns>
    /// <exception cref="InvalidOperationException">Если стек пуст.</exception>
    public T Peek()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пуст.");
        return _array[_count - 1];
    }

    /// <summary>
    /// Метод проверки наличия элемента в стеке.
    /// </summary>
    /// <param name="item">Искомый элемент.</param>
    /// <returns>True, если элемент найден; иначе False.</returns>
    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < _count; i++)
            if (comparer.Equals(_array[i], item))
                return true;
        return false;
    }

    /// <summary>
    /// Свойство количества элементов в стеке.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Свойство ёмкости внутреннего массива.
    /// </summary>
    public int Capacity => _array.Length;

    /// <summary>
    /// Метод очистки стека.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_array, 0, _count);
        _count = 0;
    }

    private void ValidateDepth(int depth)
    {
        if (depth < 0 || depth >= _count)
            throw new ArgumentOutOfRangeException(nameof(depth), "Индекс вне диапазона стека.");
    }

    /// <summary>
    /// Индексатор: 0 - вершина, Count-1 - основание.
    /// </summary>
    /// <param name="depth">Глубина относительно вершины (0 - вершина).</param>
    /// <returns>Элемент на заданной глубине.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если индекс вне диапазона стека.</exception>
    public T this[int depth]
    {
        get
        {
            ValidateDepth(depth);
            return _array[_count - 1 - depth];
        }
        set
        {
            ValidateDepth(depth);
            _array[_count - 1 - depth] = value;
        }
    }

    /// <summary>
    /// Перечислитель, осуществляющий перебор элементов стека от вершины к основанию.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0; i--)
            yield return _array[i];
    }

    /// <summary>
    /// Необобщённый перечислитель.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}