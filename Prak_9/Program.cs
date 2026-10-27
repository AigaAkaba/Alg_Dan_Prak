using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab9
{
    class Program
    {
        // Алгоритм перевірки збалансованості дужок - O(n) часу та O(n) пам'яті
        static bool CheckBalancedParentheses(string expression, out string errorDetails)
        {
            Stack<int> openingIndices = new Stack<int>();
            Stack<char> bracketsStack = new Stack<char>();

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                // 1. Якщо відкриваюча дужка - кладемо в стек
                if (ch == '(' || ch == '[' || ch == '{')
                {
                    bracketsStack.Push(ch);
                    openingIndices.Push(i);
                }
                // 2. Якщо закриваюча дужка - перевіряємо вершину стеку
                else if (ch == ')' || ch == ']' || ch == '}')
                {
                    if (bracketsStack.Count == 0)
                    {
                        errorDetails = $"❌ Зайва закриваюча дужка '{ch}' на позиції {i} (немає відповідної відкриваючої)";
                        return false;
                    }

                    char top = bracketsStack.Pop();
                    int openPos = openingIndices.Pop();

                    if (!IsMatchingPair(top, ch))
                    {
                        errorDetails = $"❌ Невідповідність типів: відкрита '{top}' на позиції {openPos}, а закрита '{ch}' на позиції {i}";
                        return false;
                    }
                }
            }

            // 3. Якщо після аналізу всього рядка в стеку щось лишилося
            if (bracketsStack.Count > 0)
            {
                char unclosed = bracketsStack.Pop();
                int unclosedPos = openingIndices.Pop();
                errorDetails = $"❌ Не закрита дужка '{unclosed}', що починається на позиції {unclosedPos}";
                return false;
            }

            errorDetails = "✅ Дужки ідеально збалансовані!";
            return true;
        }

        static bool IsMatchingPair(char open, char close)
        {
            if (open == '(' && close == ')') return true;
            if (open == '[' && close == ']') return true;
            if (open == '{' && close == '}') return true;
            return false;
        }

        // Завдання другого варіку
        static char GetClosingFor(char open)
        {
            if (open == '(') return ')';
            if (open == '[') return ']';
            return '}';
        }

        static bool TryAutoFixClosing(string expression, out string fixedExpression, out string report)
        {
            Stack<char> bracketsStack = new Stack<char>();
            Stack<int> openingIndices = new Stack<int>();
            fixedExpression = expression;

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                if (ch == '(' || ch == '[' || ch == '{')
                {
                    bracketsStack.Push(ch);
                    openingIndices.Push(i);
                }
                else if (ch == ')' || ch == ']' || ch == '}')
                {
                    if (bracketsStack.Count == 0)
                    {
                        report = $"⛔ Автовиправлення неможливе: зайва закриваюча дужка '{ch}' на позиції {i}";
                        return false;
                    }

                    char top = bracketsStack.Pop();
                    int openPos = openingIndices.Pop();

                    if (!IsMatchingPair(top, ch))
                    {
                        report = $"⛔ Автовиправлення неможливе: невідповідність '{top}' (позиція {openPos}) та '{ch}' (позиція {i})";
                        return false;
                    }
                }
            }

            if (bracketsStack.Count == 0)
            {
                report = "✅ Виправлення не потрібне: дужки вже збалансовані";
                return true;
            }

            string suffix = "";
            while (bracketsStack.Count > 0)
            {
                suffix += GetClosingFor(bracketsStack.Pop());
            }

            fixedExpression = expression + suffix;
            report = $"🛠️ Дописано {suffix.Length} закриваюч(их) дужк(и) в кінці: \"{suffix}\"";
            return true;
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №9: ВАЛІДАТОР ДУЖОК (СТЕК)    ");
            Console.WriteLine("==================================================================\n");

            string[] testCases = {
                "{ int a = (b + c) * [d - 1]; }",               // Вірно
                "if (a[i] > 0) { list.Add((a[i]); }",           // Помилка: зайва '('
                "function test() { return [1, 2, (3 + 4]; }",   // Помилка: невідповідність ']' та '('
                "())(",                                         // Помилка: зайва ')'
                "((((1 + 2) * 3)"                               // Помилка: не закрито 3 дужки
            };

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < testCases.Length; i++)
            {
                string expr = testCases[i];
                Console.WriteLine($"\nТест #{i + 1}: \"{expr}\"");
                bool isValid = CheckBalancedParentheses(expr, out string details);
                Console.WriteLine($"  Результат: {details}");
            }

            sw.Stop();
            Console.WriteLine($"\n⏱️ Загальний час аналізу: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   АВТОВИПРАВЛЕННЯ: ДОПИСУВАННЯ ЗАКРИВАЮЧИХ ДУЖОК У КІНЦІ         ");
            Console.WriteLine("==================================================================");

            string[] fixCases = {
                "{ int a = (b + c) * [d - 1]; }",
                "((((1 + 2) * 3)",
                "{ int a = (b + [c * 2",
                "if (a[i] > 0) { list.Add((a[i]); }",
                "())("
            };

            Stopwatch swFix = Stopwatch.StartNew();

            for (int i = 0; i < fixCases.Length; i++)
            {
                string expr = fixCases[i];
                Console.WriteLine($"\nТест #{i + 1}: \"{expr}\"");
                bool ok = TryAutoFixClosing(expr, out string fixedExpr, out string fixReport);
                Console.WriteLine($"  {fixReport}");
                if (ok)
                {
                    Console.WriteLine($"  Результат: \"{fixedExpr}\"");
                    CheckBalancedParentheses(fixedExpr, out string recheck);
                    Console.WriteLine($"  Повторна перевірка: {recheck}");
                }
            }

            swFix.Stop();
            Console.WriteLine($"\n⏱️ Час автовиправлення: {swFix.Elapsed.TotalMilliseconds:F4} ms");
            // Завдання другого варіку
        }
    }
}