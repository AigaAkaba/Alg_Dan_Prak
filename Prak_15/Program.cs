using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab15
{
    // Завдання другого варіку
    public class Record
    {
        public int Key { get; set; }
        public string Label { get; set; }
        public int OriginalIndex { get; set; }

        public Record(int key, string label, int originalIndex)
        {
            Key = key;
            Label = label;
            OriginalIndex = originalIndex;
        }

        public override string ToString() => $"{Key}{Label}";
    }
    // Завдання другого варіку

    class Program
    {
        // 1. Selection Sort: завжди фіксовано O(n^2) порівнянь, але лише O(n) обмінів
        static void SelectionSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0; swaps = 0;
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisons++;
                    if (arr[j] < arr[minIdx])
                    {
                        minIdx = j;
                    }
                }

                if (minIdx != i)
                {
                    int temp = arr[i];
                    arr[i] = arr[minIdx];
                    arr[minIdx] = temp;
                    swaps++;
                }
            }
        }

        // 2. Insertion Sort: O(n^2) в гіршому, але O(n) в найкращому випадку
        static void InsertionSort(int[] arr, out long comparisons, out long shifts)
        {
            comparisons = 0; shifts = 0;
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0)
                {
                    comparisons++;
                    if (arr[j] > key)
                    {
                        arr[j + 1] = arr[j]; // зсув праворуч
                        shifts++;
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
                arr[j + 1] = key;
            }
        }

        // Завдання другого варіку
        static void SelectionSortRecords(Record[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j].Key < arr[minIdx].Key)
                    {
                        minIdx = j;
                    }
                }

                if (minIdx != i)
                {
                    Record temp = arr[i];
                    arr[i] = arr[minIdx];
                    arr[minIdx] = temp;
                }
            }
        }

        static void InsertionSortRecords(Record[] arr)
        {
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                Record key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j].Key > key.Key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        static int CountStabilityViolations(Record[] arr)
        {
            int violations = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1].Key == arr[i].Key && arr[i - 1].OriginalIndex > arr[i].OriginalIndex)
                {
                    violations++;
                }
            }
            return violations;
        }

        static Record[] CloneRecords(Record[] source)
        {
            Record[] copy = new Record[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                copy[i] = new Record(source[i].Key, source[i].Label, source[i].OriginalIndex);
            }
            return copy;
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №15: SELECTION VS INSERTION   ");
            Console.WriteLine("==================================================================\n");

            int n = 2000;
            Random rnd = new Random(42);
            int[] original = new int[n];
            for (int i = 0; i < n; i++) original[i] = rnd.Next(1, 50000);

            // Майже відсортований масив (лише 2% елементів не на своїх місцях)
            int[] nearlySorted = new int[n];
            for (int i = 0; i < n; i++) nearlySorted[i] = i;
            for (int i = 0; i < n / 50; i++) nearlySorted[rnd.Next(n)] = rnd.Next(n);

            Console.WriteLine($"--- ТЕСТ 1: Випадковий масив (N={n}) ---");
            int[] arr1 = (int[])original.Clone();
            SelectionSort(arr1, out long c1, out long s1);
            Console.WriteLine($"Selection Sort: {c1:N0} порівнянь, {s1:N0} обмінів");

            int[] arr2 = (int[])original.Clone();
            InsertionSort(arr2, out long c2, out long s2);
            Console.WriteLine($"Insertion Sort: {c2:N0} порівнянь, {s2:N0} зсувів");

            Console.WriteLine($"\n--- ТЕСТ 2: Майже відсортований масив (N={n}) ---");
            int[] arr3 = (int[])nearlySorted.Clone();
            SelectionSort(arr3, out long c3, out long s3);
            Console.WriteLine($"Selection Sort: {c3:N0} порівнянь, {s3:N0} обмінів");

            int[] arr4 = (int[])nearlySorted.Clone();
            InsertionSort(arr4, out long c4, out long s4);
            Console.WriteLine($"Insertion Sort: {c4:N0} порівнянь, {s4:N0} зсувів ⚡ (в рази швидше!)");

            Console.WriteLine("\n✅ Висновок: Insertion Sort перемагає на майже відсортованих даних!");

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   ДОСЛІДЖЕННЯ СТАБІЛЬНОСТІ НА ОБ'ЄКТАХ З ОДНАКОВИМИ КЛЮЧАМИ     ");
            Console.WriteLine("==================================================================");

            Record[] demo =
            {
                new Record(3, "A", 0),
                new Record(3, "B", 1),
                new Record(2, "C", 2),
                new Record(1, "D", 3)
            };

            Console.WriteLine("\n--- Демо на 4 елементах ---");
            Console.WriteLine("Вихідний порядок:  " + string.Join(" ", (object[])demo));

            Record[] demoSel = CloneRecords(demo);
            SelectionSortRecords(demoSel);
            Console.WriteLine("Selection Sort:    " + string.Join(" ", (object[])demoSel)
                + $"  ➔ {(CountStabilityViolations(demoSel) == 0 ? "стабільно ✅" : "НЕ стабільно ❌")}");

            Record[] demoIns = CloneRecords(demo);
            InsertionSortRecords(demoIns);
            Console.WriteLine("Insertion Sort:    " + string.Join(" ", (object[])demoIns)
                + $"  ➔ {(CountStabilityViolations(demoIns) == 0 ? "стабільно ✅" : "НЕ стабільно ❌")}");

            Console.WriteLine($"\n--- Масштабний тест (N={n}, ключі від 1 до 50, багато дублікатів) ---");
            Random rnd2 = new Random(7);
            Record[] bigRecords = new Record[n];
            for (int i = 0; i < n; i++)
            {
                bigRecords[i] = new Record(rnd2.Next(1, 51), "#" + i, i);
            }

            Record[] bigSel = CloneRecords(bigRecords);
            Stopwatch swSel = Stopwatch.StartNew();
            SelectionSortRecords(bigSel);
            swSel.Stop();
            int violSel = CountStabilityViolations(bigSel);
            Console.WriteLine($"Selection Sort: порушень стабільності = {violSel:N0} | Час: {swSel.Elapsed.TotalMilliseconds:F3} ms");

            Record[] bigIns = CloneRecords(bigRecords);
            Stopwatch swIns = Stopwatch.StartNew();
            InsertionSortRecords(bigIns);
            swIns.Stop();
            int violIns = CountStabilityViolations(bigIns);
            Console.WriteLine($"Insertion Sort: порушень стабільності = {violIns:N0} | Час: {swIns.Elapsed.TotalMilliseconds:F3} ms");
            // Завдання другого варіку
        }
    }
}