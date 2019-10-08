using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
  public sealed class SharedCollectionByAsync
  {
    public int ArraySize { get; set; }
    private List<int> SharedCollection { get; }

    public SharedCollectionByAsync(int arraySize)
    {
      ArraySize = arraySize;
      SharedCollection = new List<int>();
    }
    public void FillCollectionWithRandomElements()
    {
      var thread = new Thread(async () =>
      {
        var random = new Random();

        for (int i = 0; i < ArraySize; i++)
        {
          SharedCollection.Add(random.Next(0, 10));
          await PrintAllElementsAsync();
        }
      });

      thread.Start();
    }

    private Task PrintAllElementsAsync()
    {
      Console.WriteLine("All elements: ");

      foreach (var element in SharedCollection)
      {
        Console.WriteLine(element);
      }

      Console.WriteLine($"Total count: {SharedCollection.Count}");
      Console.WriteLine();

      return Task.CompletedTask;
    }
  }
}
