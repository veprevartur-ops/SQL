using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        /// <summary>
        /// Универсальный метод для считывания значения типа T с консоли с проверкой корректности ввода и валидацией.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип значения</typeparam>
        /// <param name="prompt">Сообщение для пользователя</param>
        /// <param name="tryParse">Делегат для парсинга строки в тип T</param>
        /// <param name="validate">Функция для дополнительной проверки значения</param>
        /// <param name="typeErrorMessage">Сообщение об ошибке типа</param>
        /// <param name="validationErrorMessage">Сообщение об ошибке валидации</param>
        /// <returns>Корректное значение типа T</returns>
        static T ReadValue<T>(string prompt, TryParseHandler<T> tryParse, Func<T, bool> validate, string typeErrorMessage, string validationErrorMessage) where T : struct
        {
            Console.WriteLine(prompt);

            while (true)
            {
                // Ввод нужного значения типа T
                string input = Console.ReadLine();
                T result;

                // Проверка преобразования строки в нужный тип
                if (!tryParse(input, out result))
                {
                    Console.WriteLine(typeErrorMessage);
                    continue;
                }

                // Дополнительная валидация (например, положительное число)
                if (!validate(result))
                {
                    Console.WriteLine(validationErrorMessage);
                    continue;
                }

                // Если всё хорошо, возвращаем результат
                return result;
            }
        }

        /// <summary>
        /// Делегат для универсального TryParse
        /// </summary>
        /// <typeparam name="T">Обобщенный тип значения</typeparam>
        /// <param name="input">проверяемая вводимая строка</param>
        /// <param name="result">выводимый результат типа T в случае успешной валидации</param>
        /// <returns>Соответствие или несоответствие введенного input типу данных T</returns>
        delegate bool TryParseHandler<T>(string input, out T result);

        /// <summary>
        /// Вычисление сложных процентов по годам и формирование массива кортежей с результатами
        /// </summary>
        /// <param name="initialDeposit">Начальный вклад</param>
        /// <param name="interestRate">Годовая процентная ставка</param>
        /// <param name="years">Количество лет</param>
        /// <returns>Массив кортежей (год, сумма)</returns>
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
            // Ввод суммы вклада с проверкой
            double initial_deposit = ReadValue<double>(
                "Введите сумму первоначального вклада (положительное число):",
                double.TryParse,
                value => value > 0,
                "Ошибка! Введено не число. Повторите попытку:",
                "Ошибка! Введите положительное число. Повторите попытку:"
            );

            // Ввод количества лет с проверкой
            int years = ReadValue<int>(
                "Введите количество лет (положительное целое число):",
                int.TryParse,
                value => value > 0,
                "Ошибка! Введено не целое число. Повторите попытку:",
                "Ошибка! Введите положительное целое число. Повторите попытку:"
            );

            // Ввод процентной ставки с проверкой
            double interest_rate = ReadValue<double>(
                "Введите годовую процентную ставку (положительное число):",
                double.TryParse,
                value => value > 0,
                "Ошибка! Введено не число. Повторите попытку:",
                "Ошибка! Введите положительное число. Повторите попытку:"
            );

            // Вычисление и вывод результатов по годам
            var yearsCompoundInterest = CalculateCompoundInterest(initial_deposit, interest_rate, years);
            foreach (var (year, amount) in yearsCompoundInterest)
            {
                Console.WriteLine($"Год {year}: {amount:F2} руб.");
            }
        }
    }
}

