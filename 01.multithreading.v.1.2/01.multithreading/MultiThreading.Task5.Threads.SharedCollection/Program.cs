/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public sealed class Program
    {
        private static readonly List<int> SharedCollection = new List<int>();
        private static readonly object Locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and" +
                              " the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var thread1 = new Thread(AddElementsToCollection);
            var thread2 = new Thread(PrintAllElements);

            thread1.Start();
            thread2.Start();

            Console.ReadLine();
        }

        private static void AddElementsToCollection()
        {
            lock (Locker)
            {
                var random = new Random();

                for (int i = 0; i < 5; i++)
                {
                    SharedCollection.Add(random.Next(0, 100));
                    PrintAllElements();
                }
            }
        }

        private static void PrintAllElements()
        {
            lock (Locker)
            {
                Console.WriteLine("All elements: ");
                foreach (var number in SharedCollection)
                {
                    Console.WriteLine(number);
                }

                Console.WriteLine($"Items in collection: {SharedCollection.Count}");
                Console.WriteLine();
            }
        }
    }
}
