/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine(
                "a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine(
                "b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine(
                "c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine(
                "d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Console.WriteLine("DEMONSTRATION");
            // a.
            var parentTaskForIndependentContinue = Task.Run(() =>
            {
                Console.WriteLine($"Parent thread id: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("Parent method executing");
            });

            parentTaskForIndependentContinue.ContinueWith(IndependentMethod);

            // b.
            Task.Run(() => throw new CustomException("some exception"))
                .ContinueWith(t => { Console.WriteLine($"{t.Exception.Message}"); },
                    TaskContinuationOptions.OnlyOnFaulted);

            // c.
            Task.Run(() =>
                {
                    Console.WriteLine($"Parent thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    throw new CustomException("some exception");
                }).ContinueWith(t =>
            {

                Console.WriteLine($"Children thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"{t.Exception.Message}");

            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Current);

            // d.
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var customTaskScheduler = new CustomTaskScheduler();

            var task = Task.Run(() =>
                {
                    Console.WriteLine($"Is inside of thread pool: {Thread.CurrentThread.IsThreadPoolThread}");
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        Console.WriteLine("parent task is working, press any to cancel it....");
                        Thread.Sleep(1000);
                    }
                }, token)
                .ContinueWith(AfterAntecedentTaskWasCancelledMethod,
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnCanceled,
                    customTaskScheduler);

            Console.ReadLine();
            cancelTokenSource.Cancel();
            task.Wait();

            Console.ReadLine();
        }

        private static void IndependentMethod(Task task)
        {
            Console.WriteLine("Starting independent task:");
            Console.WriteLine("...task should be executed regardless of the result of the parent task");
            Console.WriteLine();
        }

        private static void AfterAntecedentTaskWasCancelledMethod(Task task)
        {
            Console.WriteLine($"Children task status: {task.Status}");
            Console.WriteLine("Starting after parent task was cancelled:");
            Console.WriteLine($"Current thread is inside of thread pool: {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine("...task should be executed outside of the thread pool when the parent task would be cancelled.");
        }
    }

    public class CustomTaskScheduler : TaskScheduler, IDisposable
    {
        private BlockingCollection<Task> _tasksCollection = new BlockingCollection<Task>();
        private readonly Thread _mainThread = null;

        public CustomTaskScheduler()
        {
            _mainThread = new Thread(new ThreadStart(Execute));

            if (!_mainThread.IsAlive)
            {
                _mainThread.Start();
            }
        }

        private void Execute()
        {
            foreach (var task in _tasksCollection.GetConsumingEnumerable())
            {
                TryExecuteTask(task);
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasksCollection.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            if (task != null)
            {
                _tasksCollection.Add(task);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            _tasksCollection.CompleteAdding();
            _tasksCollection.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class CustomException : Exception
    {
        public CustomException(String message) : base(message)
        {
        }
    }
}
