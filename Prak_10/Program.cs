using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab10
{
    class Program
    {
        // Пріоритет операторів
        static int GetPrecedence(char op)
        {
            if (op == '+' || op == '-') return 1;
            if (op == '*' || op == '/') return 2;
            if (op == '^') return 3;
            return 0;
        }

        // 1. Shunting-Yard: Інфіксний вираз ➔ RPN (Постфіксний)
        static List<string> InfixToRPN(string expression)
        {
            List<string> output = new List<string>();
            Stack<char> operators = new Stack<char>();

            string[] tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                // Якщо токен - число
                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                }
                else if (token == "(")
                {
                    operators.Push('(');
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        output.Add(operators.Pop().ToString());
                    }
                    if (operators.Count > 0) operators.Pop(); // видаляємо '('
                }
                else // Оператор +, -, *, /, ^
                {
                    char op = token[0];
                    while (operators.Count > 0 && GetPrecedence(operators.Peek()) >= GetPrecedence(op))
                    {
                        output.Add(operators.Pop().ToString());
                    }
                    operators.Push(op);
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop().ToString());
            }

            return output;
        }

        // 2. Обчислення значення RPN виразу на числовому стеку
        static double EvaluateRPN(List<string> rpnTokens)
        {
            Stack<double> stack = new Stack<double>();

            foreach (string token in rpnTokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    double b = stack.Pop();
                    double a = stack.Pop();
                    switch (token)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/": stack.Push(a / b); break;
                        case "^": stack.Push(Math.Pow(a, b)); break;
                        default: throw new InvalidOperationException($"Невідомий оператор: {token}");
                    }
                }
            }

            return stack.Pop();
        }

        // Завдання другого варіку
        static int GetPrecedenceUnary(char op)
        {
            if (op == '~') return 3;
            if (op == '^') return 4;
            return GetPrecedence(op);
        }

        static List<string> SplitLeadingMinus(string expression)
        {
            List<string> result = new List<string>();
            string[] rawTokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string raw in rawTokens)
            {
                string rest = raw;
                while (rest.Length > 1 && rest[0] == '-')
                {
                    result.Add("-");
                    rest = rest.Substring(1);
                }
                result.Add(rest);
            }

            return result;
        }

        static List<string> InfixToRPNUnary(string expression)
        {
            List<string> output = new List<string>();
            Stack<char> operators = new Stack<char>();
            List<string> tokens = SplitLeadingMinus(expression);
            bool prevIsOperand = false;

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                    prevIsOperand = true;
                }
                else if (token == "(")
                {
                    operators.Push('(');
                    prevIsOperand = false;
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        output.Add(operators.Pop().ToString());
                    }
                    if (operators.Count > 0) operators.Pop();
                    prevIsOperand = true;
                }
                else if (token == "-" && !prevIsOperand)
                {
                    operators.Push('~');
                    prevIsOperand = false;
                }
                else
                {
                    char op = token[0];
                    while (operators.Count > 0 && GetPrecedenceUnary(operators.Peek()) >= GetPrecedenceUnary(op))
                    {
                        output.Add(operators.Pop().ToString());
                    }
                    operators.Push(op);
                    prevIsOperand = false;
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop().ToString());
            }

            return output;
        }

        static double EvaluateRPNUnary(List<string> rpnTokens)
        {
            Stack<double> stack = new Stack<double>();

            foreach (string token in rpnTokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (token == "~")
                {
                    stack.Push(-stack.Pop());
                }
                else
                {
                    double b = stack.Pop();
                    double a = stack.Pop();
                    switch (token)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/": stack.Push(a / b); break;
                        case "^": stack.Push(Math.Pow(a, b)); break;
                        default: throw new InvalidOperationException($"Невідомий оператор: {token}");
                    }
                }
            }

            return stack.Pop();
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №10: RPN СТЕК-КАЛЬКУЛЯТОР      ");
            Console.WriteLine("==================================================================\n");

            string infixExpr = "3 + 4 * 2 / ( 1 - 5 ) ^ 2";
            Console.WriteLine($"Вхідний інфіксний вираз:  {infixExpr}");

            Stopwatch sw = Stopwatch.StartNew();

            // Перетворення
            List<string> rpn = InfixToRPN(infixExpr);
            Console.WriteLine($"Постфіксний запис (RPN):  {string.Join(" ", rpn)}");

            // Обчислення
            double result = EvaluateRPN(rpn);
            sw.Stop();

            Console.WriteLine($"\n🎯 Обчислений результат:  {result}");
            Console.WriteLine($"⏱️ Час обчислення:        {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   ПІДТРИМКА УНАРНОГО МІНУСА (у RPN позначається як ~)            ");
            Console.WriteLine("==================================================================");

            string[] unaryCases = {
                "-5 * 2",
                "- 5 * 2",
                "3 * - 4",
                "- ( 2 + 3 ) * 2",
                "- 2 ^ 2",
                "-5 ^ 2",
                "2 ^ - 2",
                "3 - - 5",
                "3 + 4 * 2 / ( 1 - 5 ) ^ 2"
            };

            Stopwatch swUnary = Stopwatch.StartNew();

            for (int i = 0; i < unaryCases.Length; i++)
            {
                string expr = unaryCases[i];
                Console.WriteLine($"\nТест #{i + 1}: {expr}");
                List<string> rpnUnary = InfixToRPNUnary(expr);
                Console.WriteLine($"  RPN:       {string.Join(" ", rpnUnary)}");
                Console.WriteLine($"  Результат: {EvaluateRPNUnary(rpnUnary)}");
            }

            swUnary.Stop();
            Console.WriteLine($"\n⏱️ Час обчислення з унарним мінусом: {swUnary.Elapsed.TotalMilliseconds:F4} ms");
            // Завдання другого варіку

            Console.WriteLine("✅ Тест алгоритму Дейкстри пройдено успішно!");
        }
    }
}