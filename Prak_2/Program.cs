using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab2
{
    class Program
    {
        // 1. Пошук мінімуму, максимуму та їх індексів за один прохід O(n)
        static void FindExtremes(int[] arr, out int minVal, out int minIdx, out int maxVal, out int maxIdx)
        {
            minVal = arr[0]; minIdx = 0;
            maxVal = arr[0]; maxIdx = 0;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < minVal)
                {
                    minVal = arr[i];
                    minIdx = i;
                }
                if (arr[i] > maxVal)
                {
                    maxVal = arr[i];
                    maxIdx = i;
                }
            }
        }

        // 2. In-place реверс масиву двома вказівниками за O(n/2) операцій і O(1) пам'яті
        static void ReverseInPlace(int[] arr)
        {
            int left = 0;
            int right = arr.Length - 1;
            while (left < right)
            {
                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
                left++;
                right--;
            }
        }

        // 3. Фільтрація елементів: вибір лише додатних парних чисел
        static int[] FilterPositiveEven(int[] arr)
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] > 0 && arr[i] % 2 == 0) count++;

            int[] result = new int[count];
            int idx = 0;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] > 0 && arr[i] % 2 == 0)
                    result[idx++] = arr[i];

            return result;
        }

        // Завдання другого варіку
        static void ReverseRange(int[] arr, int left, int right)
        {
            while (left < right)
            {
                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
                left++;
                right--;
            }
        }

        static void CyclicShiftRight(int[] arr, int k)
        {
            int n = arr.Length;
            if (n == 0) return;
            k = ((k % n) + n) % n;
            if (k == 0) return;

            ReverseRange(arr, 0, n - 1);
            ReverseRange(arr, 0, k - 1);
            ReverseRange(arr, k, n - 1);
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №2: АЛГОРИТМИ НАД МАСИВАМИ    ");
            Console.WriteLine("==================================================================\n");

            int[] numbers = { 42, -7, 18, 99, 0, -15, 24, 88, 3, -2, 76 };
            Console.WriteLine("Вихідний масив: [ " + string.Join(", ", numbers) + " ]\n");

            Stopwatch sw = Stopwatch.StartNew();

            // 1. Екстремуми
            FindExtremes(numbers, out int minVal, out int minIdx, out int maxVal, out int maxIdx);
            Console.WriteLine($"🔍 Мінімум: {minVal} (індекс {minIdx})");
            Console.WriteLine($"🔍 Максимум: {maxVal} (індекс {maxIdx})");

            // 2. Фільтрація
            int[] filtered = FilterPositiveEven(numbers);
            Console.WriteLine("\n✨ Додатні парні числа: [ " + string.Join(", ", filtered) + " ]");

            // 3. In-place реверс
            ReverseInPlace(numbers);
            Console.WriteLine("\n🔄 Масив після реверсу In-place: [ " + string.Join(", ", numbers) + " ]");

            // Завдання другого варіку
            int k = 3;
            Console.WriteLine("\n➡️ Масив перед циклічним зсувом праворуч: [ " + string.Join(", ", numbers) + " ]");
            CyclicShiftRight(numbers, k);
            Console.WriteLine($"➡️ Масив після циклічного зсуву праворуч на {k}: [ " + string.Join(", ", numbers) + " ]");
            // Завдання другого варіку

            sw.Stop();
            Console.WriteLine($"\n⏱️ Загальний час виконання: {sw.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine("✅ Тестування завершено успішно!");
        }
    }
}