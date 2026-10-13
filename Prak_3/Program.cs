using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab3
{
    class Program
    {
        // 1. Транспонування квадратної матриці In-place O(n^2)
        static void TransposeMatrix(int[,] matrix, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    int temp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = temp;
                }
            }
        }

        // 2. Пошук сідлової точки: min у рядку та max у стовпчику
        static void FindSaddlePoints(int[,] matrix, int rows, int cols)
        {
            bool found = false;

            for (int i = 0; i < rows; i++)
            {
                // Знаходимо мінімум у поточному рядку
                int minRowVal = matrix[i, 0];
                int minColIdx = 0;
                for (int j = 1; j < cols; j++)
                {
                    if (matrix[i, j] < minRowVal)
                    {
                        minRowVal = matrix[i, j];
                        minColIdx = j;
                    }
                }

                // Перевіряємо, чи є цей мінімум максимальним у своєму стовпчику minColIdx
                bool isSaddle = true;
                for (int k = 0; k < rows; k++)
                {
                    if (matrix[k, minColIdx] > minRowVal)
                    {
                        isSaddle = false;
                        break;
                    }
                }

                if (isSaddle)
                {
                    Console.WriteLine($"🎯 Знайдено сідлову точку: Значення = {minRowVal} на позиції [{i}, {minColIdx}]");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("ℹ️ У заданій матриці сідлових точок немає.");
            }
        }

        static void PrintMatrix(int[,] matrix, int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
            {
                Console.Write("  [ ");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],4} ");
                }
                Console.WriteLine("]");
            }
        }

        // Завдання другого варіку
        static int[,] MultiplyMatrices(int[,] a, int aRows, int aCols, int[,] b, int bRows, int bCols)
        {
            if (aCols != bRows)
                throw new ArgumentException("Кількість стовпців першої матриці має дорівнювати кількості рядків другої.");

            int[,] result = new int[aRows, bCols];
            for (int i = 0; i < aRows; i++)
            {
                for (int j = 0; j < bCols; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < aCols; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            }
            return result;
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №3: ДВОВИМІРНІ МАТРИЦІ        ");
            Console.WriteLine("==================================================================\n");

            // Тестова квадратна матриця 4х4
            int[,] matrix = new int[,]
            {
                { 12, 14, 11, 15 },
                { 8,  10, 6,  9  },
                { 20, 22, 19, 25 },
                { 5,  7,  4,  6  }
            };

            int n = 4;
            Console.WriteLine("Початкова матриця 4х4:");
            PrintMatrix(matrix, n, n);

            Console.WriteLine("\n--- Пошук сідлових точок ---");
            FindSaddlePoints(matrix, n, n);

            Console.WriteLine("\n--- Транспонування матриці (In-place) ---");
            Stopwatch sw = Stopwatch.StartNew();
            TransposeMatrix(matrix, n);
            sw.Stop();

            PrintMatrix(matrix, n, n);
            Console.WriteLine($"\n⏱️ Час транспонування: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Завдання другого варіку
            int aRows = 3, aCols = 4, bRows = 4, bCols = 2;

            int[,] matA = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            int[,] matB = new int[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
                { 7, 8 }
            };

            Console.WriteLine($"\n--- Множення матриць A({aRows}x{aCols}) * B({bRows}x{bCols}) ---");
            Console.WriteLine("Матриця A:");
            PrintMatrix(matA, aRows, aCols);
            Console.WriteLine("Матриця B:");
            PrintMatrix(matB, bRows, bCols);

            Stopwatch swMul = Stopwatch.StartNew();
            int[,] matC = MultiplyMatrices(matA, aRows, aCols, matB, bRows, bCols);
            swMul.Stop();

            Console.WriteLine($"Результат C({aRows}x{bCols}):");
            PrintMatrix(matC, aRows, bCols);
            Console.WriteLine($"\n Час множення: {swMul.Elapsed.TotalMilliseconds:F4} ms");
            // Завдання другого варіку

            Console.WriteLine("✅ Роботу виконано успішно!");
        }
    }
}