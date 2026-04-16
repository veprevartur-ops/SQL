using System;

namespace ConsoleApp2
{
    internal class Program
    {
        /// <summary>
        /// Создание ромба из символов 'X' в двумерном массиве с заданной длиной диагонали N.
        /// </summary>
        /// <param name="diagonalLength">Длина диагонали (нечётное положительное число)</param>
        /// <returns>Двумерный массив символов с ромбом</returns>
        static char[,] CreateDiamond(int diagonalLength)
        {
            if (diagonalLength <= 0 || diagonalLength % 2 == 0)
            {
                Console.WriteLine("N должно быть положительным нечётным числом.");
                return null;
            }

            char[,] diamond = new char[diagonalLength, diagonalLength];

            // Заполняем пробелами
            for (int i = 0; i < diagonalLength; i++)
            {
                for (int j = 0; j < diagonalLength; j++)
                {
                    diamond[i, j] = ' ';
                }
            }

            int mid = diagonalLength / 2;

            for (int i = 0; i <= mid; i++)
            {
                diamond[i, mid - i] = 'X';
                diamond[i, mid + i] = 'X';
                diamond[diagonalLength - i - 1, mid - i] = 'X';
                diamond[diagonalLength - i - 1, mid + i] = 'X';
            }

            return diamond;
        }

        /// <summary>
        /// Печать ромба из двумерного массива символов
        /// </summary>
        /// <param name="diamond">Двумерный массив символов</param>
        static void PrintDiamond(char[,] diamond)
        {
            if (diamond == null)
            {
                return;
            }

            int n = diamond.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(diamond[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Введите нечётную длину диагонали ромба: ");
            if (int.TryParse(Console.ReadLine(), out int diagonalLength))
            {
                char[,] diamond = CreateDiamond(diagonalLength);
                PrintDiamond(diamond);
            }
            else
            {
                Console.WriteLine("Ошибка ввода числа.");
            }
        }
    }
}