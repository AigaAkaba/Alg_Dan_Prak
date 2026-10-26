using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab7
{
    // Опис процесу для диспетчера ОС
    public class ProcessItem
    {
        public string Name { get; set; }
        public int RemainingTimeMs { get; set; }

        // Завдання другого варіку
        public int Priority { get; set; } = 1;
        // Завдання другого варіку

        public ProcessItem(string name, int remainingTimeMs)
        {
            Name = name;
            RemainingTimeMs = remainingTimeMs;
        }

        // Завдання другого варіку
        public ProcessItem(string name, int remainingTimeMs, int priority) : this(name, remainingTimeMs)
        {
            Priority = priority;
        }
        // Завдання другого варіку
    }

    public class CircularNode
    {
        public ProcessItem Process { get; set; }
        public CircularNode Next { get; set; }

        public CircularNode(ProcessItem process)
        {
            Process = process;
            Next = this; // за замовчуванням посилається сам на себе
        }
    }

    public class RoundRobinScheduler
    {
        private CircularNode? _tail = null;
        private int _count = 0;

        public int Count => _count;

        // Додавання процесу в кільце
        public void AddProcess(ProcessItem proc)
        {
            CircularNode newNode = new CircularNode(proc);
            if (_tail == null)
            {
                _tail = newNode;
                _tail.Next = _tail;
            }
            else
            {
                newNode.Next = _tail.Next;
                _tail.Next = newNode;
                _tail = newNode;
            }
            _count++;
        }

        // Симуляція планувальника Round-Robin
        public void Run(int timeQuantumMs)
        {
            if (_tail == null) return;

            Console.WriteLine($"\n🚀 Запуск диспетчера процесора (Квант часу = {timeQuantumMs} ms)...\n");
            CircularNode prev = _tail;
            CircularNode curr = _tail.Next;

            int round = 1;
            while (_count > 0)
            {
                Console.WriteLine($"--- Раунд #{round++} ---");
                Console.Write($"  ⚙️ Процес [{curr.Process.Name}] (залишок: {curr.Process.RemainingTimeMs} ms) отримує CPU ➔ ");

                if (curr.Process.RemainingTimeMs <= timeQuantumMs)
                {
                    Console.WriteLine($"ЗАВЕРШЕНО за {curr.Process.RemainingTimeMs} ms! 🏁");
                    // Видаляємо завершений вузол з кільця
                    if (_count == 1)
                    {
                        _tail = null;
                        _count = 0;
                        break;
                    }
                    else
                    {
                        prev.Next = curr.Next;
                        if (curr == _tail) _tail = prev;
                        curr = curr.Next;
                        _count--;
                    }
                }
                else
                {
                    curr.Process.RemainingTimeMs -= timeQuantumMs;
                    Console.WriteLine($"Квант вичерпано, новий залишок: {curr.Process.RemainingTimeMs} ms ⏳");
                    prev = curr;
                    curr = curr.Next;
                }
            }

            Console.WriteLine("\n✨ Усі процеси в черзі успішно виконано!");
        }

        // Завдання другого варіку
        private int GetQuantum(ProcessItem proc, int baseQuantumMs)
        {
            return proc.Priority > 1 ? baseQuantumMs * 2 : baseQuantumMs;
        }

        public void RunWithPriority(int baseQuantumMs)
        {
            if (_tail == null) return;

            Console.WriteLine($"\n🚀 Запуск диспетчера з пріоритетами (Базовий квант = {baseQuantumMs} ms, високий пріоритет = {baseQuantumMs * 2} ms)...\n");
            CircularNode prev = _tail;
            CircularNode curr = _tail.Next;

            int round = 1;
            while (_count > 0)
            {
                int quantum = GetQuantum(curr.Process, baseQuantumMs);

                Console.WriteLine($"--- Раунд #{round++} ---");
                Console.Write($"  ⚙️ Процес [{curr.Process.Name}] (пріоритет: {curr.Process.Priority}, квант: {quantum} ms, залишок: {curr.Process.RemainingTimeMs} ms) отримує CPU ➔ ");

                if (curr.Process.RemainingTimeMs <= quantum)
                {
                    Console.WriteLine($"ЗАВЕРШЕНО за {curr.Process.RemainingTimeMs} ms! 🏁");
                    if (_count == 1)
                    {
                        _tail = null;
                        _count = 0;
                        break;
                    }
                    else
                    {
                        prev.Next = curr.Next;
                        if (curr == _tail) _tail = prev;
                        curr = curr.Next;
                        _count--;
                    }
                }
                else
                {
                    curr.Process.RemainingTimeMs -= quantum;
                    Console.WriteLine($"Квант вичерпано, новий залишок: {curr.Process.RemainingTimeMs} ms ⏳");
                    prev = curr;
                    curr = curr.Next;
                }
            }

            Console.WriteLine("\n✨ Усі процеси з пріоритетами успішно виконано!");
        }
        // Завдання другого варіку
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №7: ROUND-ROBIN SCHEDULER     ");
            Console.WriteLine("==================================================================");

            RoundRobinScheduler scheduler = new RoundRobinScheduler();
            scheduler.AddProcess(new ProcessItem("IDE Rider", 25));
            scheduler.AddProcess(new ProcessItem("Browser Chrome", 40));
            scheduler.AddProcess(new ProcessItem("Telegram Bot", 15));
            scheduler.AddProcess(new ProcessItem("Audio Player", 10));

            scheduler.Run(timeQuantumMs: 15);

            // Завдання другого варіку
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("   ПЛАНУВАЛЬНИК З ПРІОРИТЕТАМИ (2 = високий, 1 = звичайний)       ");
            Console.WriteLine("==================================================================");

            RoundRobinScheduler prioScheduler = new RoundRobinScheduler();
            prioScheduler.AddProcess(new ProcessItem("IDE Rider", 25, 2));
            prioScheduler.AddProcess(new ProcessItem("Browser Chrome", 40, 1));
            prioScheduler.AddProcess(new ProcessItem("Telegram Bot", 15, 1));
            prioScheduler.AddProcess(new ProcessItem("Audio Player", 10, 2));

            prioScheduler.RunWithPriority(baseQuantumMs: 15);
            // Завдання другого варіку
        }
    }
}