using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
  static class Calculator
  {
    // todo: change this method to support cancellation token
    public static Task<long> CalculateAsync(int n, CancellationToken token)
    {
      long sum = 0;

      for (int i = 0; i < n; i++)
      {
        if (token.IsCancellationRequested)
        {
          break;
        }

        // i + 1 is to allow 2147483647 (Max(Int32)) 
        sum = sum + (i + 1);
        Thread.Sleep(10);
      }

      return Task.FromResult(sum);
    }
  }
}
