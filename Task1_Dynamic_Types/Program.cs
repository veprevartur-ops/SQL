using System;

namespace ConsoleApp1
{
    internal class Program
    {
        /// <summary>
        /// Универсальный метод для безопасного ввода значения с консоли с валидацией.
        /// </summary>
        static T ReadValue<T>(string prompt, Func<string, T> parser, Func<T, bool> validate = null)
        {
            while (true)
            {
                Console.Write(prompt);
                try
                {
                    T value = parser(Console.ReadLine());
                    if (validate == null || validate(value))
                    {
                        return value;
                    }
                    Console.WriteLine("Некорректное значение.");
                }
                catch
                {
                    Console.WriteLine("Ошибка формата.");
                }
            }
        }

        /// <summary>
        /// Вычисление сложных процентов по годам и формирование массива кортежей с результатами
        /// </summary>
        static (int year, double amount)[] CalculateCompoundInterest(double initialDeposit, double interestRate, int years)
        {
            var results = new (int year, double amount)[years];

            for (int i = 1; i <= years; i++)
            {
                double yearAmount = initialDeposit * Math.Pow((1 + interestRate / 100), i);
                results[i - 1] = (i, yearAmount);
            }

            return results;
        }

        static void Main(string[] args)
        {
            double initial_deposit = ReadValue(
                "Введите сумму первоначального вклада (положительное число): ",
                double.Parse,
                v => v > 0
            );

            int years = ReadValue(
                "Введите количество лет (положительное целое число): ",
                int.Parse,
                v => v > 0
            );

            double interest_rate = ReadValue(
                "Введите годовую процентную ставку (положительное число): ",
                double.Parse,
                v => v > 0
            );

            var yearsCompoundInterest = CalculateCompoundInterest(initial_deposit, interest_rate, years);
            foreach (var (year, amount) in yearsCompoundInterest)
            {
                Console.WriteLine($"Год {year}: {amount:F2} руб.");
            }
        }
    }
}

