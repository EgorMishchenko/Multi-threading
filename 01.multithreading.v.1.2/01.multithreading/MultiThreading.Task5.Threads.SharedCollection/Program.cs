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
    class Program
    {
        static object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and" +
                              " the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var sharedCollection = new List<int>();

            var thread1 = new Thread(new ParameterizedThreadStart(AddElementsToCollection));

            thread1.Start(sharedCollection);

            Console.ReadLine();
        }

        static void AddElementsToCollection(object objectList)
        {
            lock (locker)
            {
                var random = new Random();

                var list = (ICollection<int>)objectList;

                for (int i = 0; i < 5; i++)
                {
                    list.Add(random.Next(0, 100));
                    PrintAllElements(list);
                }
            }
        }

        static void PrintAllElements(object objectList)
        {
            var t = new Thread(() =>
            {
                lock (locker)
                {
                    var list = (ICollection<int>)objectList;

                    Console.WriteLine("All elements: ");
                    foreach (var number in list)
                    {
                        Console.WriteLine(number);
                    }
                }
            });

            t.Start();
        }
    }
}
