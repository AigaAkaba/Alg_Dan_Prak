using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab14
{
    class Program
    {
        // Оптимізований Bubble Sort з підрахунком метрик
        static void OptimizedBubbleSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;

                for (int j = 0; j < n - 1 - i; j++)
                {
                    comparisons++;
                    if (arr[j] > arr[j + 1])
                    {
                        // Обмін сусідніх елементів
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;

                        swaps++;
                        swapped = true;
                    }
                }

                // Якщо за весь внутрішній прохід не було жодного обміну - масив відсортовано!
                if (!swapped)
                {
                    break; // Early exit оптимізація!
                }
            }
        }

        // Завдання другого варіку
        static void LastSwapBubbleSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int boundary = arr.Length - 1;

            while (boundary > 0)
            {
                int lastSwap = 0;

                for (int j = 0; j < boundary; j++)
                {
                    comparisons++;
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;

                        swaps++;
                        lastSwap = j;
                    }
                }

                boundary = lastSwap;
            }
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №14: BUBBLE SORT OPTIMIZATION ");
            Console.WriteLine("==================================================================\n");

            int n = 1000;
            int[] randomData = new int[n];
            int[] sortedData = new int[n];
            int[] reversedData = new int[n];

            Random rnd = new Random(42);
            for (int i = 0; i < n; i++)
            {
                randomData[i] = rnd.Next(1, 10000);
                sortedData[i] = i;
                reversedData[i] = n - i;
            }

            // 1. Тест на випадкових даних
            int[] test1 = (int[])randomData.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            OptimizedBubbleSort(test1, out long comp1, out long swap1);
            sw.Stop();
            Console.WriteLine($"📊 1. Випадковий масив (N={n}):");
            Console.WriteLine($"   Порівнянь: {comp1:N0} | Перестановок: {swap1:N0} | Час: {sw.Elapsed.TotalMilliseconds:F3} ms");

            // 2. Тест на вже відсортованому масиві (Найкращий випадок O(n))
            int[] test2 = (int[])sortedData.Clone();
            sw.Restart();
            OptimizedBubbleSort(test2, out long comp2, out long swap2);
            sw.Stop();
            Console.WriteLine($"\n📊 2. Вже відсортований масив (Найкращий випадок завдяки swapped):");
            Console.WriteLine($"   Порівнянь: {comp2:N0} | Перестановок: {swap2:N0} | Час: {sw.Elapsed.TotalMilliseconds:F3} ms");

            // 3. Тест на зворотному масиві (Найгірший випадок O(n^2))
            int[] test3 = (int[])reversedData.Clone();
            sw.Restart();
            OptimizedBubbleSort(test3, out long comp3, out long swap3);
            sw.Stop();
            Console.WriteLine($"\n📊 3. Зворотно впорядкований масив (Найгірший випадок):");
            Console.WriteLine($"   Порівнянь: {comp3:N0} | Перестановок: {swap3:N0} | Час: {sw.Elapsed.TotalMilliseconds:F3} ms");

            Console.WriteLine("\n✅ Висновок: прапорець 'swapped' скорочує час найкращого випадку з O(n^2) до O(n)!");

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   ОПТИМІЗАЦІЯ ІНДЕКСОМ ОСТАННЬОГО ОБМІНУ (LAST SWAP)            ");
            Console.WriteLine("==================================================================");

            int[] partialData = new int[n];
            for (int i = 0; i < n; i++)
            {
                partialData[i] = i < n / 2 ? rnd.Next(1, 5000) : 5000 + i;
            }

            string[] names =
            {
                "Випадковий масив",
                "Вже відсортований масив",
                "Зворотно впорядкований масив",
                "Відсортований хвіст (друга половина вже на місці)"
            };
            int[][] datasets = { randomData, sortedData, reversedData, partialData };

            for (int d = 0; d < datasets.Length; d++)
            {
                int[] a = (int[])datasets[d].Clone();
                int[] b = (int[])datasets[d].Clone();

                Stopwatch swOld = Stopwatch.StartNew();
                OptimizedBubbleSort(a, out long compOld, out long swapOld);
                swOld.Stop();

                Stopwatch swNew = Stopwatch.StartNew();
                LastSwapBubbleSort(b, out long compNew, out long swapNew);
                swNew.Stop();

                bool same = true;
                for (int i = 0; i < n; i++)
                {
                    if (a[i] != b[i]) { same = false; break; }
                }

                Console.WriteLine($"\n📊 {d + 1}. {names[d]} (N={n}):");
                Console.WriteLine($"   swapped:   Порівнянь: {compOld:N0} | Перестановок: {swapOld:N0} | Час: {swOld.Elapsed.TotalMilliseconds:F3} ms");
                Console.WriteLine($"   last swap: Порівнянь: {compNew:N0} | Перестановок: {swapNew:N0} | Час: {swNew.Elapsed.TotalMilliseconds:F3} ms");
                Console.WriteLine($"   Однаковий результат сортування: {(same ? "так ✅" : "ні ❌")}");
            }
            // Завдання другого варіку
        }
    }
}