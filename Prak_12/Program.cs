using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab12
{
    // Реалізація Deque на двозв'язному списку для гарантованого O(1) з обох кінців
    public class MyDeque<T>
    {
        private LinkedList<T> _list = new LinkedList<T>();

        public int Count => _list.Count;
        public bool IsEmpty => _list.Count == 0;

        public void AddFirst(T item) => _list.AddFirst(item);
        public void AddLast(T item) => _list.AddLast(item);

        public T RemoveFirst()
        {
            if (IsEmpty) throw new InvalidOperationException("Deque порожній!");
            T val = _list.First!.Value;
            _list.RemoveFirst();
            return val;
        }

        public T RemoveLast()
        {
            if (IsEmpty) throw new InvalidOperationException("Deque порожній!");
            T val = _list.Last!.Value;
            _list.RemoveLast();
            return val;
        }
    }

    // Завдання другого варіку
    public class ArrayDeque<T>
    {
        private T[] _buffer;
        private int _head = 0;
        private int _count = 0;

        public int Count => _count;
        public int Capacity => _buffer.Length;
        public bool IsEmpty => _count == 0;

        public ArrayDeque(int capacity = 4)
        {
            if (capacity < 1) capacity = 4;
            _buffer = new T[capacity];
        }

        private void Grow()
        {
            T[] newBuffer = new T[_buffer.Length * 2];
            for (int i = 0; i < _count; i++)
            {
                newBuffer[i] = _buffer[(_head + i) % _buffer.Length];
            }
            _buffer = newBuffer;
            _head = 0;
        }

        public void AddFirst(T item)
        {
            if (_count == _buffer.Length) Grow();
            _head = (_head - 1 + _buffer.Length) % _buffer.Length;
            _buffer[_head] = item;
            _count++;
        }

        public void AddLast(T item)
        {
            if (_count == _buffer.Length) Grow();
            int tail = (_head + _count) % _buffer.Length;
            _buffer[tail] = item;
            _count++;
        }

        public T RemoveFirst()
        {
            if (IsEmpty) throw new InvalidOperationException("Deque порожній!");
            T val = _buffer[_head];
            _buffer[_head] = default!;
            _head = (_head + 1) % _buffer.Length;
            _count--;
            return val;
        }

        public T RemoveLast()
        {
            if (IsEmpty) throw new InvalidOperationException("Deque порожній!");
            int last = (_head + _count - 1) % _buffer.Length;
            T val = _buffer[last];
            _buffer[last] = default!;
            _count--;
            return val;
        }

        public void Print()
        {
            Console.Write($"[Count={_count}/{Capacity}, Head={_head}]: FRONT ➔ [ ");
            for (int i = 0; i < _count; i++)
            {
                Console.Write($"{_buffer[(_head + i) % _buffer.Length]} ");
            }
            Console.WriteLine("] ➔ BACK");
        }
    }
    // Завдання другого варіку

    class Program
    {
        // Алгоритм перевірки паліндрома за допомогою Deque
        static bool IsPalindrome(string text)
        {
            MyDeque<char> deque = new MyDeque<char>();

            // Очищаємо текст від пробілів, розділових знаків та зводимо до нижнього регістру
            foreach (char c in text.ToLower())
            {
                if (char.IsLetterOrDigit(c))
                {
                    deque.AddLast(c);
                }
            }

            // Порівнюємо символи з обох кінців черги
            while (deque.Count > 1)
            {
                char first = deque.RemoveFirst();
                char last = deque.RemoveLast();
                if (first != last) return false;
            }

            return true;
        }

        // Завдання другого варіку
        static bool IsPalindromeArray(string text)
        {
            ArrayDeque<char> deque = new ArrayDeque<char>();

            foreach (char c in text.ToLower())
            {
                if (char.IsLetterOrDigit(c))
                {
                    deque.AddLast(c);
                }
            }

            while (deque.Count > 1)
            {
                char first = deque.RemoveFirst();
                char last = deque.RemoveLast();
                if (first != last) return false;
            }

            return true;
        }
        // Завдання другого варіку

        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №12: DEQUE ТА ПАЛІНДРОМИ       ");
            Console.WriteLine("==================================================================\n");

            string[] samples = {
                "А роза упала на лапу Азора",
                "Я несу гусеня",
                "Козак з казок",
                "Алгоритми та структури даних",
                "12321",
                "123321"
            };

            Stopwatch sw = Stopwatch.StartNew();

            foreach (string s in samples)
            {
                bool result = IsPalindrome(s);
                string status = result ? "✅ ПАЛІНДРОМ" : "❌ НЕ паліндром";
                Console.WriteLine($"  Рядок: \"{s}\" ➔ {status}");
            }

            sw.Stop();
            Console.WriteLine($"\n⏱️ Час перевірки: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Завдання другого варіку
            Console.WriteLine("\n--- Deque на кільцевому масиві: паліндроми ---");

            Stopwatch swArr = Stopwatch.StartNew();

            foreach (string s in samples)
            {
                bool result = IsPalindromeArray(s);
                string status = result ? "✅ ПАЛІНДРОМ" : "❌ НЕ паліндром";
                Console.WriteLine($"  Рядок: \"{s}\" ➔ {status}");
            }

            swArr.Stop();
            Console.WriteLine($"\n⏱️ Час перевірки (масив): {swArr.Elapsed.TotalMilliseconds:F4} ms");

            Console.WriteLine("\n--- Deque на кільцевому масиві: циклічний перехід і розширення ---");
            ArrayDeque<int> demo = new ArrayDeque<int>(capacity: 2);

            demo.AddLast(1);
            demo.AddLast(2);
            demo.Print();

            demo.AddFirst(0);
            Console.WriteLine("  ➕ AddFirst(0) ➔ буфер повний, відбулось розширення");
            demo.Print();

            demo.AddFirst(-1);
            Console.WriteLine("  ➕ AddFirst(-1) ➔ голова перейшла через 0 назад на кінець масиву");
            demo.Print();

            Console.WriteLine($"  ➖ RemoveLast() = {demo.RemoveLast()}");
            Console.WriteLine($"  ➖ RemoveFirst() = {demo.RemoveFirst()}");
            demo.Print();
            // Завдання другого варіку
        }
    }
}