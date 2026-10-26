using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab5
{
    // Вузол однозв'язного списку
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T>? Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    // Клас однозв'язного списку
    public class SinglyLinkedList<T>
    {
        private Node<T>? _head;
        // Завдання другого варіку
        private Node<T>? _tail;
        // Завдання другого варіку
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        // Вставка в початок списку - O(1)
        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data);
            newNode.Next = _head;
            _head = newNode;
            // Завдання другого варіку
            if (_tail == null)
            {
                _tail = newNode;
            }
            // Завдання другого варіку
            _count++;
        }

        // Вставка в кінець списку - O(n)
        public void AddLast(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (_head == null)
            {
                _head = newNode;
                // Завдання другого варіку
                _tail = newNode;
                // Завдання другого варіку
            }
            else
            {
                // Завдання другого варіку
                _tail!.Next = newNode;
                _tail = newNode;
                // Завдання другого варіку
            }
            _count++;
        }

        // Видалення першого входження значення - O(n)
        public bool Remove(T data)
        {
            if (_head == null) return false;

            // Якщо треба видалити перший елемент
            if (Equals(_head.Data, data))
            {
                _head = _head.Next;
                // Завдання другого варіку
                if (_head == null)
                {
                    _tail = null;
                }
                // Завдання другого варіку
                _count--;
                return true;
            }

            Node<T> current = _head;
            while (current.Next != null)
            {
                if (Equals(current.Next.Data, data))
                {
                    // Завдання другого варіку
                    if (current.Next == _tail)
                    {
                        _tail = current;
                    }
                    // Завдання другого варіку
                    current.Next = current.Next.Next; // Перекидаємо посилання
                    _count--;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        // Друк списку у вигляді красивого ланцюжка
        public void Print()
        {
            Console.Write($"[Count={_count}]: ");
            Node<T>? current = _head;
            while (current != null)
            {
                Console.Write($"[{current.Data}] ➔ ");
                current = current.Next;
            }
            Console.WriteLine("null");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №5: ОДНОЗВ'ЯЗНИЙ СПИСОК        ");
            Console.WriteLine("==================================================================\n");

            SinglyLinkedList<string> list = new SinglyLinkedList<string>();

            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine("--- 1. Додавання елементів ---");
            list.AddFirst("Київ");
            list.AddFirst("Житомир");
            list.AddLast("Львів");
            list.AddLast("Одеса");
            list.Print();

            Console.WriteLine("\n--- 2. Вставка нового лідера (Харків) на початок ---");
            list.AddFirst("Харків");
            list.Print();

            Console.WriteLine("\n--- 3. Видалення вузла 'Житомир' ---");
            bool removed = list.Remove("Житомир");
            Console.WriteLine($"Результат видалення: {removed}");
            list.Print();

            Console.WriteLine("\n--- 4. Спроба видалити неіснуючий вузол 'Рівне' ---");
            bool removedNonExist = list.Remove("Рівне");
            Console.WriteLine($"Результат: {removedNonExist}");
            list.Print();

            // Завдання другого варіку
            Console.WriteLine("\n--- 5. Перевірка _tail: видалення хвоста 'Одеса' і додавання 'Полтава' в кінець ---");
            list.Remove("Одеса");
            list.AddLast("Полтава");
            list.Print();
            // Завдання другого варіку

            sw.Stop();
            Console.WriteLine($"\n⏱️ Час роботи алгоритмів: {sw.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine("✅ Роботу зі зв'язаним списком завершено без збоїв!");
        }
    }
}