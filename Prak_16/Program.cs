using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab16
{
    class Program
    {
        // 1. Наївний рекурсивний Фібоначчі - Експоненційний час O(2^n)
        static long NaiveFibonacci(int n, ref long callsCount)
        {
            callsCount++;
            if (n <= 1) return n; // Базовий випадок
            return NaiveFibonacci(n - 1, ref callsCount) + NaiveFibonacci(n - 2, ref callsCount);
        }

        // 2. Оптимізований Фібоначчі з мемоізацією (кешуванням) - Лінійний час O(n)
        static long MemoizedFibonacci(int n, Dictionary<int, long> memo, ref long callsCount)
        {
            callsCount++;
            if (n <= 1) return n;
            if (memo.ContainsKey(n)) return memo[n]; // миттєвий результат з кешу

            long result = MemoizedFibonacci(n - 1, memo, ref callsCount) + MemoizedFibonacci(n - 2, memo, ref callsCount);
            memo[n] = result;
            return result;
        }

        // 3. Демонстрація глибини рекурсії та переповнення стеку
        static void DemonstrateRecursionDepth(int currentDepth, int targetDepth)
        {
            if (currentDepth >= targetDepth)
            {
                Console.WriteLine($"  🎯 Досягнуто безпечну максимальну глибину стеку: {currentDepth} рівнів!");
                return;
            }
            DemonstrateRecursionDepth(currentDepth + 1, targetDepth);
        }

        // Завдання другого варіку
        static long GcdRecursive(long a, long b, ref long callsCount)
        {
            callsCount++;
            if (b == 0) return Math.Abs(a);
            return GcdRecursive(b, a % b, ref callsCount);
        }

        static long GcdIterative(long a, long b)
        {
            while (b != 0)
            {
                long temp = a % b;
                a = b;
                b = temp;
            }
            return Math.Abs(a);
        }

        static long GcdSubtraction(long a, long b, ref long stepsCount)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a == 0) return b;
            if (b == 0) return a;
            while (a != b)
            {
                stepsCount++;
                if (a > b) a -= b;
                else b -= a;
            }
            return a;
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №16: РЕКУРСІЯ ТА МЕМОІЗАЦІЯ   ");
            Console.WriteLine("==================================================================\n");

            int n = 35;
            Console.WriteLine($"Обчислюємо {n}-е число Фібоначчі:\n");

            // Тест 1: Наївна рекурсія
            long calls1 = 0;
            Stopwatch sw = Stopwatch.StartNew();
            long fib1 = NaiveFibonacci(n, ref calls1);
            sw.Stop();
            Console.WriteLine($"1. Наївна рекурсія:");
            Console.WriteLine($"   Результат: {fib1:N0} | Викликів функцій: {calls1:N0} | Час: {sw.Elapsed.TotalMilliseconds:F2} ms");

            // Тест 2: Мемоізація
            long calls2 = 0;
            var memo = new Dictionary<int, long>();
            sw.Restart();
            long fib2 = MemoizedFibonacci(n, memo, ref calls2);
            sw.Stop();
            Console.WriteLine($"\n2. Мемоізація (Кеш):");
            Console.WriteLine($"   Результат: {fib2:N0} | Викликів функцій: {calls2:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms ⚡");

            Console.WriteLine("\n3. Перевірка глибини рекурсивних викликів:");
            DemonstrateRecursionDepth(1, 1000);

            Console.WriteLine("\n✅ Висновок: Мемоізація зменшила кількість викликів із мільйонів до десятків!");

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   НАЙБІЛЬШИЙ СПІЛЬНИЙ ДІЛЬНИК: РЕКУРСИВНИЙ АЛГОРИТМ ЕВКЛІДА     ");
            Console.WriteLine("==================================================================");

            (long, long)[] pairs =
            {
                (48, 18),
                (1071, 462),
                (17, 5),
                (100, 0),
                (0, 25),
                (-24, 36),
                (1000000007, 998244353),
                (832040, 514229)
            };

            foreach ((long a, long b) in pairs)
            {
                long callsEuclid = 0;
                long stepsSub = 0;
                long gRec = GcdRecursive(a, b, ref callsEuclid);
                long gIter = GcdIterative(a, b);
                long gSub = GcdSubtraction(a, b, ref stepsSub);
                string ok = (gRec == gIter && gRec == gSub) ? "✅" : "❌";

                Console.WriteLine($"\n  НСД({a}, {b}) = {gRec} {ok}");
                Console.WriteLine($"     Евклід (рекурсія): викликів {callsEuclid} | ітеративно: {gIter} | відніманням: кроків {stepsSub:N0}");
            }

            long callsWorst = 0;
            long gWorst = GcdRecursive(832040, 514229, ref callsWorst);
            Console.WriteLine($"\n🔍 Найгірший випадок (сусідні числа Фібоначчі 832040 та 514229): НСД = {gWorst}, викликів = {callsWorst}");

            long callsDepth = 0;
            GcdRecursive(long.MaxValue, long.MaxValue - 1, ref callsDepth);
            Console.WriteLine($"🔍 НСД(long.MaxValue, long.MaxValue - 1): викликів = {callsDepth} (глибина рекурсії дуже мала)");
            // Завдання другого варіку
        }
    }
}