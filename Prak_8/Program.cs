using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab8
{
    public class ArrayStack<T>
    {
        private T[] _items;
        private int _top = -1;

        public int Count => _top + 1;
        public int Capacity => _items.Length;
        public bool IsEmpty => _top == -1;

        public ArrayStack(int capacity = 5)
        {
            _items = new T[capacity];
        }

        // Додавання елемента на вершину стеку - O(1)
        public void Push(T item)
        {
            if (_top == _items.Length - 1)
            {
                throw new StackOverflowException($"Стек переповнено! Максимальна місткість: {_items.Length}");
            }
            _items[++_top] = item;
        }

        // Вилучення елемента з вершини стеку - O(1)
        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Неможливо виконати Pop: Стек порожній!");
            }
            T item = _items[_top];
            _items[_top--] = default!; // очищення для збирача сміття
            return item;
        }

        // Перегляд вершини без вилучення - O(1)
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Стек порожній!");
            return _items[_top];
        }

        public void Print()
        {
            Console.Write($"[Count={Count}/{Capacity}]: Bottom ➔ [ ");
            for (int i = 0; i <= _top; i++) Console.Write($"{_items[i]} ");
            Console.WriteLine("] ➔ TOP");
        }
    }

    // Завдання другого варіку
    public class MinStack<T>
    {
        private ArrayStack<T> _main;
        private ArrayStack<T> _mins;
        private Comparer<T> _comparer = Comparer<T>.Default;

        public int Count => _main.Count;
        public int Capacity => _main.Capacity;
        public bool IsEmpty => _main.IsEmpty;

        public MinStack(int capacity = 5)
        {
            _main = new ArrayStack<T>(capacity);
            _mins = new ArrayStack<T>(capacity);
        }

        public void Push(T item)
        {
            _main.Push(item);
            if (_mins.IsEmpty || _comparer.Compare(item, _mins.Peek()) <= 0)
            {
                _mins.Push(item);
            }
        }

        public T Pop()
        {
            T item = _main.Pop();
            if (_comparer.Compare(item, _mins.Peek()) == 0)
            {
                _mins.Pop();
            }
            return item;
        }

        public T Peek()
        {
            return _main.Peek();
        }

        public T GetMin()
        {
            if (_mins.IsEmpty) throw new InvalidOperationException("Неможливо виконати GetMin: Стек порожній!");
            return _mins.Peek();
        }

        public void Print()
        {
            _main.Print();
        }
    }
    // Завдання другого варіку

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №8: СТЕК (LIFO) НА МАСИВІ      ");
            Console.WriteLine("==================================================================\n");

            ArrayStack<string> undoHistory = new ArrayStack<string>(capacity: 4);

            Console.WriteLine("--- 1. Додавання операцій у стек (Push) ---");
            undoHistory.Push("Написано текст");
            undoHistory.Push("Змінено шрифт");
            undoHistory.Push("Вставлено таблицю");
            undoHistory.Print();

            Console.WriteLine($"\n🔍 Поточна вершина (Peek): \"{undoHistory.Peek()}\"");

            Console.WriteLine("\n--- 2. Скасування операцій (Undo / Pop) ---");
            while (!undoHistory.IsEmpty)
            {
                string action = undoHistory.Pop();
                Console.WriteLine($"  ↩️ Скасовано: \"{action}\"");
                undoHistory.Print();
            }

            Console.WriteLine("\n--- 3. Перевірка обробки винятків при вилученні з порожнього стеку ---");
            try
            {
                undoHistory.Pop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠️ Перехоплено очікувану помилку: {ex.Message}");
            }

            // Завдання другого варіку
            Console.WriteLine("\n--- 4. MinStack: Push з відстеженням мінімуму ---");
            MinStack<int> minStack = new MinStack<int>(capacity: 6);
            int[] values = { 5, 3, 7, 3, 1 };
            foreach (int v in values)
            {
                minStack.Push(v);
                Console.WriteLine($"  ➕ Push({v}) ➔ GetMin() = {minStack.GetMin()}");
            }
            minStack.Print();

            Console.WriteLine("\n--- 5. MinStack: Pop з оновленням мінімуму ---");
            while (!minStack.IsEmpty)
            {
                int popped = minStack.Pop();
                string minText = minStack.IsEmpty ? "стек порожній" : minStack.GetMin().ToString();
                Console.WriteLine($"  ➖ Pop() = {popped} ➔ GetMin() = {minText}");
            }

            Console.WriteLine("\n--- 6. MinStack: GetMin() на порожньому стеку ---");
            try
            {
                minStack.GetMin();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠️ Перехоплено очікувану помилку: {ex.Message}");
            }
            // Завдання другого варіку

            Console.WriteLine("\n✅ Роботу зі стеком завершено успішно!");
        }
    }
}