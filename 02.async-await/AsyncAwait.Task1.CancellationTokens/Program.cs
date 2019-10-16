/*
 * Изучите код данного приложения для расчета суммы целых чисел от 0 до N, а затем
 * измените код приложения таким образом, чтобы выполнялись следующие требования:
 * 1. Расчет должен производиться асинхронно.
 * 2. N задается пользователем из консоли. Пользователь вправе внести новую границу в процессе вычислений,
 * что должно привести к перезапуску расчета.
 * 3. При перезапуске расчета приложение должно продолжить работу без каких-либо сбоев.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
  class Program
  {
      /// <summary>
      /// The Main method should not be changed at all.
      /// </summary>
      /// <param name="args"></param>
    static void Main(string[] args)
    {
      Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
      Console.WriteLine("Calculating the sum of integers from 0 to N.");
      Console.WriteLine("Use 'q' key to exit...");
      Console.WriteLine();

      Console.WriteLine("Enter N: ");
      
      string input = Console.ReadLine();
      while (input.Trim().ToUpper() != "Q")
      {
        var cts = new CancellationTokenSource();

        if (int.TryParse(input, out int n))
        {
          CalculateSumSecondWay(n, cts);
        }
        else
        {
          Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
          Console.WriteLine("Enter N: ");
        }

        input = Console.ReadLine();
        cts.Cancel();
      }

      Console.WriteLine("Press any key to continue");
      Console.ReadLine();
    }

    private static void CalculateSum(int n, CancellationTokenSource cts)
    {
      Task.Run(async () =>
      {
        cts.Token.Register(() => { Console.WriteLine($"Sum for {n} cancelled..."); });
        long sum = await Calculator.CalculateAsync(n, cts.Token);
        
        Console.WriteLine($"Sum for {n} = {sum}.");
        Console.WriteLine();
        Console.WriteLine("Enter N: ");
      });
      
      Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");
    }

    // It doesn't work.
    private static void CalculateSumSecondWay(int n, CancellationTokenSource cts)
    {
      try
      {
        Task.Run(async () =>
        {
          var task = Calculator.CalculateAsync(n, cts.Token);

          await task;

          Console.WriteLine($"Sum for {n} = {task.Result}.");
          Console.WriteLine();
          Console.WriteLine("Enter N: ");
        });
      }
      catch (Exception e)
      {
        // This code is unreachable.
        Console.WriteLine($"Sum for {n} cancelled...");
      }

      Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");
    }
  }
}