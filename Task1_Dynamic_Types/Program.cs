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

        static T ReadValue<T>(string prompt, TryParseHandler<T> tryParse,  Func<T, bool> validate, string typeErrorMessage, string validationErrorMessage) where T : struct
        {
            Console.WriteLine(prompt);

            while (true)
            {
                string input = Console.ReadLine();
                T result;

                if (!tryParse(input, out result))  
                {
                    Console.WriteLine(typeErrorMessage);
                    continue;
                }

                if (!validate(result))
                {
                    Console.WriteLine(validationErrorMessage);
                    continue;
                }

                return result;
            }
        }

        delegate bool TryParseHandler<T>(string input, out T result);

        static string CalculateCompoundInterest(double initialDeposit, double interestRate, int years)
        {
            string result = "";

            for (int i = 1; i <= years; i++)
            {
                double yearAmount = initialDeposit * Math.Pow((1 + interestRate / 100), i);
                result += $"Год {i}: {yearAmount:F2} руб.\n";
            }

            return result;
        }

        static void Main(string[] args)
        {

            double initial_deposit = ReadValue<double>("Введите сумму первоначального вклада (положительное число):", double.TryParse, value => value > 0, "Ошибка! Введено не число. Повторите попытку:", "Ошибка! Введите положительное число. Повторите попытку:");

            int years = ReadValue<int>("Введите количество лет (положительное целое число):", int.TryParse, value => value > 0, "Ошибка! Введено не целое число. Повторите попытку:", "Ошибка! Введите положительное целое число. Повторите попытку:");

            double interest_rate = ReadValue<double>("Введите годовую процентную ставку (положительное число):", double.TryParse, value => value > 0, "Ошибка! Введено не число. Повторите попытку:", "Ошибка! Введите положительное число. Повторите попытку:");


            string yearsCompoundInterest = CalculateCompoundInterest(initial_deposit, interest_rate, years);
            Console.WriteLine(yearsCompoundInterest);
        }
    }
}

