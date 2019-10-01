/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, " +
                              "decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("Using Thread class");
            DecrementAndPrintByThread(10);

            Console.WriteLine();

            Console.WriteLine("Using Thread Pool");
            DecrementAndPrintByThreadPool(10);

            Console.ReadLine();
        }


        private static void DecrementAndPrintByThread(object number)
        {
            var x = (int)number;
            if (x != 0)
            {
                Console.WriteLine(x--);
                var thread = new Thread(new ParameterizedThreadStart(DecrementAndPrintByThread));
                Console.WriteLine($"Thread id: {thread.ManagedThreadId}");
                thread.Start(x);
                thread.Join();
            }
        }

        private static void DecrementAndPrintByThreadPool(object number)
        {
            var x = (int)number;
            if (x != 0)
            {
                Console.WriteLine(x--);
                ThreadPool.QueueUserWorkItem(DecrementAndPrintByThreadPool, x);
                Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }

}
