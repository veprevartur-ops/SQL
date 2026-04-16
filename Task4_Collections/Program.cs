using System;
using System.Collections.Generic;

class Program
{
    /// <summary>
    /// Создаёт стек и заполняет его элементами.
    /// </summary>
    /// <typeparam name="T">Тип элементов стека.</typeparam>
    /// <param name="items">Элементы для добавления.</param>
    /// <returns>Заполненный стек.</returns>
    static SmartStack<T> CreateStack<T>(params T[] items)
    {
        var stack = new SmartStack<T>();
        foreach (var item in items)
            stack.Push(item);
        return stack;
    }

    static void Main()
    {
        // 1. Стек по умолчанию
        var stack1 = CreateStack(1, 2, 3);
        Console.WriteLine("stack1:");
        stack1.PrintCollection(stack1);

        // 2. Стек с заданной ёмкостью
        var stack2 = CreateStack("a", "b");
        Console.WriteLine("\nstack2:");
        stack2.PrintCollection(stack2);

        // 3. Стек из коллекции
        var list = new List<int> { 10, 20, 30, 40 };
        var stack3 = new SmartStack<int>(list);
        Console.WriteLine("\nstack3:");
        stack3.PrintCollection(stack3);

        // 4. PushRange
        stack1.PushRange(new int[] { 100, 200, 300 });
        Console.WriteLine("\nstack1 после PushRange:");
        stack1.PrintCollection(stack1);

        // 5. Pop, Peek, Contains, индексатор
        Console.WriteLine($"\nPop: {stack1.Pop()}"); // 300
        Console.WriteLine($"Peek: {stack1.Peek()}"); // 200
        Console.WriteLine($"Contains 2: {stack1.Contains(2)}"); // true
        Console.WriteLine($"Count: {stack1.Count}, Capacity: {stack1.Capacity}");

        Console.WriteLine($"stack1[0] (вершина): {stack1[0]}");
        Console.WriteLine($"stack1[{stack1.Count - 1}] (основание): {stack1[stack1.Count - 1]}");
    }
}

