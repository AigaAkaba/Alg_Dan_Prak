using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab13
{
    public class PriorityItem<T>
    {
        public T Data { get; set; }
        public int Priority { get; set; } // Чим більше число - тим вищий пріоритет!

        public PriorityItem(T data, int priority)
        {
            Data = data;
            Priority = priority;
        }
    }

    public class MyPriorityQueue<T>
    {
        private List<PriorityItem<T>> _items = new List<PriorityItem<T>>();

        public int Count => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        // Вставка елемента - O(1) в кінець списку
        public void Enqueue(T data, int priority)
        {
            _items.Add(new PriorityItem<T>(data, priority));
        }

        // Вилучення елемента з найвищим пріоритетом - O(n) лінійний пошук максимуму
        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Черга порожня!");

            int highestIndex = 0;
            int maxPriority = _items[0].Priority;

            for (int i = 1; i < _items.Count; i++)
            {
                if (_items[i].Priority > maxPriority)
                {
                    maxPriority = _items[i].Priority;
                    highestIndex = i;
                }
            }

            T result = _items[highestIndex].Data;
            _items.RemoveAt(highestIndex);
            return result;
        }

        public void Print()
        {
            Console.WriteLine($"[PriorityQueue Count={Count}]:");
            foreach (var item in _items)
            {
                Console.WriteLine($"   🔹 Пріоритет [{item.Priority,2}] ➔ {item.Data}");
            }
        }
    }

    // Завдання другого варіку
    public class SortedPriorityQueue<T>
    {
        private List<PriorityItem<T>> _items = new List<PriorityItem<T>>();

        public int Count => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        public void Enqueue(T data, int priority)
        {
            PriorityItem<T> newItem = new PriorityItem<T>(data, priority);
            _items.Add(newItem);

            int i = _items.Count - 1;
            while (i > 0 && _items[i - 1].Priority >= priority)
            {
                _items[i] = _items[i - 1];
                i--;
            }
            _items[i] = newItem;
        }

        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Черга порожня!");

            int last = _items.Count - 1;
            T result = _items[last].Data;
            _items.RemoveAt(last);
            return result;
        }

        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Черга порожня!");
            return _items[_items.Count - 1].Data;
        }

        public void Print()
        {
            Console.WriteLine($"[SortedPriorityQueue Count={Count}] (від найвищого пріоритету):");
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                Console.WriteLine($"   🔹 Пріоритет [{_items[i].Priority,2}] ➔ {_items[i].Data}");
            }
        }
    }
    // Завдання другого варіку

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №13: PRIORITY QUEUE           ");
            Console.WriteLine("==================================================================\n");

            MyPriorityQueue<string> hospitalQueue = new MyPriorityQueue<string>();

            // Додаємо пацієнтів у приймальне відділення з різною терміновістю (1-10)
            hospitalQueue.Enqueue("Пацієнт А (Плановий огляд)", priority: 1);
            hospitalQueue.Enqueue("Пацієнт Б (Гострий апендицит)", priority: 9);
            hospitalQueue.Enqueue("Пацієнт В (Застуда)", priority: 2);
            hospitalQueue.Enqueue("Пацієнт Г (Інфаркт міокарда)", priority: 10);
            hospitalQueue.Enqueue("Пацієнт Д (Перелом пальця)", priority: 5);

            hospitalQueue.Print();

            Console.WriteLine("\n--- Черговість прийому лікарями за пріоритетом ---");
            int order = 1;
            while (!hospitalQueue.IsEmpty)
            {
                string patient = hospitalQueue.Dequeue();
                Console.WriteLine($"  🏥 #{order++} На прийом викликається: {patient}");
            }

            Console.WriteLine("\n✅ Усі пацієнти прийняті в строгому порядку пріоритету!");

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   ВПОРЯДКОВАНА ЧЕРГА: INSERTION SORT ПРИ ENQUEUE, DEQUEUE O(1)  ");
            Console.WriteLine("==================================================================\n");

            SortedPriorityQueue<string> sortedQueue = new SortedPriorityQueue<string>();

            Stopwatch swSorted = Stopwatch.StartNew();

            sortedQueue.Enqueue("Пацієнт А (Плановий огляд)", priority: 1);
            sortedQueue.Enqueue("Пацієнт Б (Гострий апендицит)", priority: 9);
            sortedQueue.Enqueue("Пацієнт В (Застуда)", priority: 2);
            sortedQueue.Enqueue("Пацієнт Г (Інфаркт міокарда)", priority: 10);
            sortedQueue.Enqueue("Пацієнт Д (Перелом пальця)", priority: 5);
            sortedQueue.Enqueue("Пацієнт Е (Опік руки)", priority: 5);

            sortedQueue.Print();
            Console.WriteLine($"\n🔍 Наступний на прийом (Peek): {sortedQueue.Peek()}");

            Console.WriteLine("\n--- Черговість прийому (Dequeue за O(1)) ---");
            int sortedOrder = 1;
            while (!sortedQueue.IsEmpty)
            {
                string patient = sortedQueue.Dequeue();
                Console.WriteLine($"  🏥 #{sortedOrder++} На прийом викликається: {patient}");
            }

            swSorted.Stop();
            Console.WriteLine($"\n⏱️ Час роботи впорядкованої черги: {swSorted.Elapsed.TotalMilliseconds:F4} ms");
            // Завдання другого варіку
        }
    }
}