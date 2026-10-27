using System;
using System.Diagnostics;
// Завдання другого варіку
using System.Threading;
// Завдання другого варіку

namespace Rozumatic_ASD_Lab11
{
    public class CircularQueue<T>
    {
        private T[] _buffer;
        private int _head = 0;
        private int _tail = 0;
        private int _count = 0;

        public int Count => _count;
        public int Capacity => _buffer.Length;
        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == _buffer.Length;

        public CircularQueue(int capacity = 5)
        {
            _buffer = new T[capacity];
        }

        // Додавання в чергу (хвіст) - O(1)
        public void Enqueue(T item)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Черга повністю заповнена (Queue Overflow)!");
            }

            _buffer[_tail] = item;
            _tail = (_tail + 1) % _buffer.Length; // Циклічний перехід на 0 індекс
            _count++;
        }

        // Вилучення з черги (голова) - O(1)
        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Черга порожня (Queue Underflow)!");
            }

            T item = _buffer[_head];
            _buffer[_head] = default!; // очищення
            _head = (_head + 1) % _buffer.Length; // Циклічний перехід
            _count--;
            return item;
        }

        // Перегляд першого в черзі без вилучення - O(1)
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Черга порожня!");
            return _buffer[_head];
        }

        public void Print()
        {
            Console.Write($"[Count={_count}/{Capacity}, Head={_head}, Tail={_tail}]: ");
            if (IsEmpty)
            {
                Console.WriteLine("ПОРОЖНЯ");
                return;
            }

            Console.Write("HEAD ➔ [ ");
            for (int i = 0; i < _count; i++)
            {
                int index = (_head + i) % _buffer.Length;
                Console.Write($"{_buffer[index]} ");
            }
            Console.WriteLine("] ➔ TAIL");
        }
    }

    // Завдання другого варіку
    public class ThreadSafeCircularBuffer<T>
    {
        private readonly CircularQueue<T> _queue;
        private readonly object _sync = new object();
        private bool _completed = false;

        public int Capacity => _queue.Capacity;

        public int Count
        {
            get
            {
                lock (_sync)
                {
                    return _queue.Count;
                }
            }
        }

        public ThreadSafeCircularBuffer(int capacity = 5)
        {
            _queue = new CircularQueue<T>(capacity);
        }

        public void Put(T item)
        {
            lock (_sync)
            {
                while (_queue.IsFull && !_completed)
                {
                    Monitor.Wait(_sync);
                }

                if (_completed)
                {
                    throw new InvalidOperationException("Додавання в буфер вже завершено!");
                }

                _queue.Enqueue(item);
                Monitor.PulseAll(_sync);
            }
        }

        public bool TryTake(out T item)
        {
            lock (_sync)
            {
                while (_queue.IsEmpty && !_completed)
                {
                    Monitor.Wait(_sync);
                }

                if (_queue.IsEmpty)
                {
                    item = default!;
                    return false;
                }

                item = _queue.Dequeue();
                Monitor.PulseAll(_sync);
                return true;
            }
        }

        public void CompleteAdding()
        {
            lock (_sync)
            {
                _completed = true;
                Monitor.PulseAll(_sync);
            }
        }
    }
    // Завдання другого варіку

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №11: КІЛЬЦЕВИЙ БУФЕР (ЧЕРГА)  ");
            Console.WriteLine("==================================================================\n");

            CircularQueue<string> queue = new CircularQueue<string>(capacity: 4);

            Console.WriteLine("--- 1. Додавання 4 завдань на друк ---");
            queue.Enqueue("Doc1.pdf");
            queue.Enqueue("Report.docx");
            queue.Enqueue("Photo.png");
            queue.Enqueue("Invoice.xlsx");
            queue.Print();

            Console.WriteLine("\n--- 2. Обробка перших 2 документів ---");
            Console.WriteLine($"  🖨️ Надруковано: {queue.Dequeue()}");
            Console.WriteLine($"  🖨️ Надруковано: {queue.Dequeue()}");
            queue.Print();

            Console.WriteLine("\n--- 3. Додавання 2 нових завдань (циклічний перехід на початок масиву) ---");
            queue.Enqueue("Presentation.pptx");
            queue.Enqueue("Certificate.pdf");
            queue.Print();

            Console.WriteLine($"\n🔍 Наступний документ у черзі (Peek): \"{queue.Peek()}\"");

            // Завдання другого варіку
            Console.WriteLine("\n--- 4. Producer-Consumer: 2 виробники, 2 споживачі, буфер на 3 місця ---");

            int producersCount = 2;
            int consumersCount = 2;
            int itemsPerProducer = 5;
            int consumed = 0;

            ThreadSafeCircularBuffer<string> safeBuffer = new ThreadSafeCircularBuffer<string>(capacity: 3);

            Thread[] producers = new Thread[producersCount];
            Thread[] consumers = new Thread[consumersCount];

            for (int p = 0; p < producersCount; p++)
            {
                int id = p + 1;
                producers[p] = new Thread(() =>
                {
                    for (int n = 1; n <= itemsPerProducer; n++)
                    {
                        string job = $"P{id}-Job{n}";
                        safeBuffer.Put(job);
                        Console.WriteLine($"  📥 Виробник {id} додав: {job}");
                        Thread.Sleep(10);
                    }
                });
            }

            for (int c = 0; c < consumersCount; c++)
            {
                int id = c + 1;
                consumers[c] = new Thread(() =>
                {
                    while (safeBuffer.TryTake(out string job))
                    {
                        Interlocked.Increment(ref consumed);
                        Console.WriteLine($"  🖨️ Споживач {id} обробив: {job}");
                        Thread.Sleep(30);
                    }
                });
            }

            Stopwatch swThreads = Stopwatch.StartNew();

            foreach (Thread t in consumers) t.Start();
            foreach (Thread t in producers) t.Start();

            foreach (Thread t in producers) t.Join();
            safeBuffer.CompleteAdding();
            foreach (Thread t in consumers) t.Join();

            swThreads.Stop();

            Console.WriteLine($"\n📊 Створено завдань: {producersCount * itemsPerProducer}, оброблено: {consumed}, залишок у буфері: {safeBuffer.Count}");
            Console.WriteLine($"⏱️ Час багатопотокового сценарію: {swThreads.Elapsed.TotalMilliseconds:F4} ms");
            // Завдання другого варіку

            Console.WriteLine("✅ Роботу кільцевого буфера перевірено успішно!");
        }
    }
}