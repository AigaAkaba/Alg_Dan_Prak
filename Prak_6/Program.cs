using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab6
{
    public class DoublyNode<T>
    {
        public T Data { get; set; }
        public DoublyNode<T>? Next { get; set; }
        public DoublyNode<T>? Prev { get; set; }

        public DoublyNode(T data)
        {
            Data = data;
        }
    }

    public class DoublyLinkedList<T>
    {
        private DoublyNode<T>? _head;
        private DoublyNode<T>? _tail;
        private int _count;

        public int Count => _count;

        // Вставка в початок - O(1)
        public void AddFirst(T data)
        {
            DoublyNode<T> newNode = new DoublyNode<T>(data);
            if (_head == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Next = _head;
                _head.Prev = newNode;
                _head = newNode;
            }
            _count++;
        }

        // Вставка в кінець - O(1)
        public void AddLast(T data)
        {
            DoublyNode<T> newNode = new DoublyNode<T>(data);
            if (_tail == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Prev = _tail;
                _tail.Next = newNode;
                _tail = newNode;
            }
            _count++;
        }

        // Видалення відомого вузла за O(1) без потреби шукати попередника!
        public void RemoveNode(DoublyNode<T> node)
        {
            if (node.Prev != null)
                node.Prev.Next = node.Next;
            else
                _head = node.Next;

            if (node.Next != null)
                node.Next.Prev = node.Prev;
            else
                _tail = node.Prev;

            _count--;
        }

        // Завдання другого варіку
        public DoublyNode<T>? FindNode(T data)
        {
            DoublyNode<T>? curr = _head;
            while (curr != null)
            {
                if (Equals(curr.Data, data)) return curr;
                curr = curr.Next;
            }
            return null;
        }

        public bool SwapWithNext(DoublyNode<T> a)
        {
            DoublyNode<T>? b = a.Next;
            if (b == null) return false;

            DoublyNode<T>? p = a.Prev;
            DoublyNode<T>? n = b.Next;

            if (p != null) p.Next = b;
            else _head = b;

            b.Prev = p;
            b.Next = a;
            a.Prev = b;
            a.Next = n;

            if (n != null) n.Prev = a;
            else _tail = a;

            return true;
        }
        // Завдання другого варіку

        // Прямий обхід (від голови до хвоста)
        public void PrintForward()
        {
            Console.Write($"Прямий обхід  [Count={_count}]: null ⇄ ");
            DoublyNode<T>? curr = _head;
            while (curr != null)
            {
                Console.Write($"[{curr.Data}] ⇄ ");
                curr = curr.Next;
            }
            Console.WriteLine("null");
        }

        // Зворотний обхід (від хвоста до голови)
        public void PrintBackward()
        {
            Console.Write($"Зворотний обхід [Count={_count}]: null ⇄ ");
            DoublyNode<T>? curr = _tail;
            while (curr != null)
            {
                Console.Write($"[{curr.Data}] ⇄ ");
                curr = curr.Prev;
            }
            Console.WriteLine("null");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №6: ДВОЗВ'ЯЗНИЙ СПИСОК         ");
            Console.WriteLine("==================================================================\n");

            DoublyLinkedList<int> dList = new DoublyLinkedList<int>();

            dList.AddLast(10);
            dList.AddLast(20);
            dList.AddLast(30);
            dList.AddFirst(5);

            Console.WriteLine("--- Двостороннє виведення списку ---");
            dList.PrintForward();
            dList.PrintBackward();

            // Завдання другого варіку
            Console.WriteLine("\n--- Обмін сусідніх вузлів 10 та 20 (середина) ---");
            dList.SwapWithNext(dList.FindNode(10)!);
            dList.PrintForward();
            dList.PrintBackward();

            Console.WriteLine("\n--- Обмін сусідніх вузлів 5 та 20 (голова) ---");
            dList.SwapWithNext(dList.FindNode(5)!);
            dList.PrintForward();
            dList.PrintBackward();

            Console.WriteLine("\n--- Обмін сусідніх вузлів 10 та 30 (хвіст) ---");
            dList.SwapWithNext(dList.FindNode(10)!);
            dList.PrintForward();
            dList.PrintBackward();
            // Завдання другого варіку

            Console.WriteLine("✅ Тестування двозв'язних посилань завершено!");
        }
    }
}
