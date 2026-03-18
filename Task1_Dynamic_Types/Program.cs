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
        static void Main(string[] args)
        {
            double initial_deposit;
            int years;
            double interest_rate;

            Console.WriteLine("Введите начальный вклад (положительное число)");
            while (true)
            {
                try
                {
                    if (!double.TryParse(Console.ReadLine(), out initial_deposit))
                        throw new FormatException("Ошибка! Введено не число. Повторите попытку:");

                    if (initial_deposit <= 0)
                        throw new ArgumentOutOfRangeException("initial_deposit", "Ошибка! Введите положительное число. Повторите попытку:");

                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Введите количество лет (положительное целое число)");
            while (true)
            {
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out years))
                        throw new FormatException("Ошибка! Введено не целое число. Повторите попытку:");

                    if (years <= 0)
                        throw new ArgumentOutOfRangeException("years", "Ошибка! Введите положительное целое число. Повторите попытку:");

                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Введите годовую процентную ставку (положительное число)");
            while (true)
            {
                try
                {
                    if (!double.TryParse(Console.ReadLine(), out interest_rate))
                        throw new FormatException("Ошибка! Введено не число. Повторите попытку:");

                    if (interest_rate <= 0)
                        throw new ArgumentOutOfRangeException("interest_rate", "Ошибка! Введите положительное число. Повторите попытку:");

                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var yearsCompoundInterest = "";
            for (int i = 1; i <= years; i++)
            {
                double yearInterest = initial_deposit * Math.Pow((1 + interest_rate / 100), i);
                var yearCompoundInterest = $"Год {i}: {yearInterest:F2} руб.\n";
                yearsCompoundInterest += yearCompoundInterest;
            }
            Console.WriteLine(yearsCompoundInterest);
        }
    }
}

