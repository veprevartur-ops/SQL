using System;
using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        /// <summary>
        /// Создает ромб из символов 'X' с заданной длиной диагонали N.
        /// </summary>
        /// <param name="diagonalLength">Длина диагонали (нечётное положительное число)</param>
        static StringBuilder[] CreateDiamond(int diagonalLength)
        {
            if (diagonalLength <= 0 || diagonalLength % 2 == 0)
            {
                Console.WriteLine("N должно быть положительным нечётным числом.");
                return null;
            }

            int padding = diagonalLength / 2;
            var diamondLines = new StringBuilder[diagonalLength];

            for (int i = 0; i <= diagonalLength / 2; i++)
            {
                var diamondLine = new StringBuilder(new string(' ', diagonalLength));
                diamondLine[padding] = 'X';
                diamondLine[diagonalLength - padding - 1] = 'X';
                diamondLines[i] = new StringBuilder(diamondLine.ToString());
                diamondLines[diagonalLength - i - 1] = new StringBuilder(diamondLine.ToString());
                padding--;
            }

            return diamondLines;
        }

        /// <summary>
        /// Печатает ромб из символов 'X' с заданной длиной диагонали N.
        /// </summary>
        /// <param name="diamondLines">Массив строк, которые образуют ромб из X</param>
        static void PrintDiamond(StringBuilder[] diamondLines) { 

            foreach (var line in diamondLines)
            {
                Console.WriteLine(line);
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Введите нечётную длину диагонали ромба: ");
            if (int.TryParse(Console.ReadLine(), out int diagonalLength))
            {
                StringBuilder[] diamondLines = CreateDiamond(diagonalLength);
                PrintDiamond(diamondLines);
            }
            else
            {
                Console.WriteLine("Ошибка ввода числа.");
            }
        }
    }
}
