/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var parentTask = Task.Run(() =>
            {
                Console.WriteLine($"Parent thread id: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("Parent method executing");

                throw new ArgumentException("Task failed");
            });

            parentTask.ContinueWith(IndependentMethod);

            parentTask.ContinueWith(WhenParentFailMethod, TaskContinuationOptions.OnlyOnFaulted);

            parentTask.ContinueWith(WhenParentFailAndUsingParentThreadMethod, TaskScheduler.FromCurrentSynchronizationContext());

            //var taskD = parentTask.ContinueWith();

            Console.ReadLine();
        }

        private static void ParentMethod()
        {
            Console.WriteLine($"Parent thread id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Parent method executing");

        }

        private static void IndependentMethod(Task task)
        {
            Console.WriteLine("Starting independent task:");
            Console.WriteLine("...task should be executed regardless of the result of the parent task");
            Console.WriteLine();
        }

        private static void WhenParentFailMethod(Task task)
        {
            Console.WriteLine("Starting continuation after failed parent task:");
            Console.WriteLine("...task should be executed when the parent task finished without success.");
            Console.WriteLine();
        }

        private static void WhenParentFailAndUsingParentThreadMethod(Task task)
        {
            if (task.IsFaulted)
            {
                Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("Starting continuation after failed parent task and using parent thread:");
                Console.WriteLine("...task should be executed when the parent task would be finished with fail " +
                                  "and parent task thread should be reused for continuation.");
                Console.WriteLine();
            }
        }

        private static void OutsideThreadPoolMethod()
        {
            Console.WriteLine();
            Console.WriteLine("Starting continuation after failed parent task:");
            Console.WriteLine("...task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine();
        }
    }
    }
}
