using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Универсальный стек с дополнительными возможностями.
/// </summary>
/// <typeparam name="T">Тип элементов стека.</typeparam>
public class SmartStack<T> : IEnumerable<T>
{
    private T[] _array; // Внутренний массив для хранения элементов
    private int _count; // Количество элементов в стеке

    /// <summary>
    /// Конструктор без параметров. Создаёт стек ёмкостью 4.
    /// </summary>
    public SmartStack()
    {
        _array = new T[4];
        _count = 0;
    }

    /// <summary>
    /// Конструктор с заданной ёмкостью.
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
    /// Конструктор из коллекции. Последний элемент коллекции становится вершиной стека.
    /// </summary>
    /// <param name="collection">Исходная коллекция.</param>
    /// <exception cref="ArgumentNullException">Если коллекция равна null.</exception>
    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        // Подсчёт количества элементов
        int size = 0;
        foreach (var _ in collection) size++;

        _array = new T[size];
        _count = size;

        int i = 0;
        foreach (var item in collection)
        {
            _array[i++] = item;
        }
    }

    /// <summary>
    /// Добавить элемент на вершину стека.
    /// </summary>
    /// <param name="item">Добавляемый элемент.</param>
    public void Push(T item)
    {
        // Если массив заполнен, увеличиваем его размер вдвое
        if (_count == _array.Length)
        {
            Array.Resize(ref _array, _array.Length == 0 ? 4 : _array.Length * 2);
        }
        _array[_count++] = item;
    }

    /// <summary>
    /// Добавить коллекцию элементов на вершину стека (в обратном порядке).
    /// </summary>
    /// <param name="collection">Коллекция элементов.</param>
    /// <exception cref="ArgumentNullException">Если коллекция равна null.</exception>
    public void PushRange(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        // Подсчитаем количество элементов
        int count = 0;
        foreach (var _ in collection) count++;

        // Скопируем элементы во временный массив
        T[] temp = new T[count];
        int i = 0;
        foreach (var item in collection)
            temp[i++] = item;

        // Добавим элементы в стек в обратном порядке
        for (int j = count - 1; j >= 0; j--)
            Push(temp[j]);
    }

    /// <summary>
    /// Удалить и вернуть элемент с вершины стека.
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
    /// Вернуть элемент с вершины стека без удаления.
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
    /// Проверить наличие элемента в стеке.
    /// </summary>
    /// <param name="item">Искомый элемент.</param>
    /// <returns>True, если элемент найден; иначе False.</returns>
    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < _count; i++)
        {
            if (comparer.Equals(_array[i], item))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Количество элементов в стеке.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Ёмкость внутреннего массива.
    /// </summary>
    public int Capacity => _array.Length;

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
            if (depth < 0 || depth >= _count)
                throw new ArgumentOutOfRangeException(nameof(depth), "Индекс вне диапазона стека.");
            return _array[_count - 1 - depth];
        }
        set
        {
            if (depth < 0 || depth >= _count)
                throw new ArgumentOutOfRangeException(nameof(depth), "Индекс вне диапазона стека.");
            _array[_count - 1 - depth] = value;
        }
    }

    /// <summary>
    /// Возвращает перечислитель, осуществляющий перебор элементов стека от вершины к основанию.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0; i--)
            yield return _array[i];
    }

    /// <summary>
    /// Необобщённая версия перечислителя.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// Пример использования:
class Program
{
    static void Main()
    {
        // 1. Стек по умолчанию
        var stack1 = new SmartStack<int>();
        stack1.Push(1);
        stack1.Push(2);
        stack1.Push(3);
        Console.WriteLine("stack1:");
        foreach (var x in stack1)
            Console.WriteLine(x); // 3 2 1

        // 2. Стек с заданной ёмкостью
        var stack2 = new SmartStack<string>(2);
        stack2.Push("a");
        stack2.Push("b");
        Console.WriteLine("\nstack2:");
        foreach (var x in stack2)
            Console.WriteLine(x); // b a

        // 3. Стек из коллекции
        var list = new List<int> { 10, 20, 30, 40 };
        var stack3 = new SmartStack<int>(list);
        Console.WriteLine("\nstack3:");
        foreach (var x in stack3)
            Console.WriteLine(x); // 40 30 20 10

        // 4. PushRange
        stack1.PushRange(new int[] { 100, 200, 300 });
        Console.WriteLine("\nstack1 после PushRange:");
        foreach (var x in stack1)
            Console.WriteLine(x); // 300 200 100 3 2 1

        // 5. Pop, Peek, Contains, индексатор
        Console.WriteLine($"\nPop: {stack1.Pop()}"); // 300
        Console.WriteLine($"Peek: {stack1.Peek()}"); // 200
        Console.WriteLine($"Contains 2: {stack1.Contains(2)}"); // true
        Console.WriteLine($"Count: {stack1.Count}, Capacity: {stack1.Capacity}");

        Console.WriteLine($"stack1[0] (вершина): {stack1[0]}");
        Console.WriteLine($"stack1[{stack1.Count - 1}] (основание): {stack1[stack1.Count - 1]}");
    }
}
