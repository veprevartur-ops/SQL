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
    private T[] _array; // Внутренний массив для хранения элементов
    private int _count; // Количество элементов в стеке

    public SmartStack()
    {
        _array = new T[4];
        _count = 0;
    }

    public SmartStack(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Ёмкость должна быть положительной.");
        _array = new T[capacity];
        _count = 0;
    }

    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));
        _array = collection.ToArray();
        _count = _array.Length;
    }

    public void Push(T item)
    {
        if (_count == _array.Length)
            Array.Resize(ref _array, _array.Length == 0 ? 4 : _array.Length * 2);
        _array[_count++] = item;
    }

    public void PushRange(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));
        foreach (var item in collection.Reverse())
            Push(item);
    }

    public T Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пуст.");
        return _array[--_count];
    }

    public T Peek()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек пуст.");
        return _array[_count - 1];
    }

    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < _count; i++)
            if (comparer.Equals(_array[i], item))
                return true;
        return false;
    }

    public int Count => _count;
    public int Capacity => _array.Length;

    private void ValidateDepth(int depth)
    {
        if (depth < 0 || depth >= _count)
            throw new ArgumentOutOfRangeException(nameof(depth), "Индекс вне диапазона стека.");
    }

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

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0; i--)
            yield return _array[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void PrintCollection(IEnumerable<T> collection)
    {
        foreach (var item in collection)
            Console.WriteLine(item);
    }
}