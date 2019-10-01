/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        public const int ArraySize = 10;

        private static int[] NumberArray { get; set; }
        public static Random Random { get; set; }

        static Program()
        {
            NumberArray = new int[ArraySize];
            Random = new Random();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var task = new Task(FillArrayWithRandomNumbers);

            task.ContinueWith(MultiplyArray)
                .ContinueWith(SortArray)
                .ContinueWith(CalculateAverage);

            task.Start();

            Console.ReadLine();
        }

        static void FillArrayWithRandomNumbers()
        {
            Console.WriteLine("Filled array");
            for (int i = 0; i < NumberArray.Length; i++)
            {
                NumberArray[i] = Random.Next(0, 100);
                Console.WriteLine(NumberArray[i]);
            }
        }

        static void MultiplyArray(Task task)
        {
            var randomNumber = Random.Next(0, 100);
            Console.WriteLine($"Multiplied array by {randomNumber}:");
            
            for (int i = 0; i < NumberArray.Length; i++)
            {
                NumberArray[i] *= randomNumber;
                Console.WriteLine(NumberArray[i]);
            }
        }

        static void SortArray(Task task)
        {
            Console.WriteLine("Sorted array:");
            Array.Sort(NumberArray);

            foreach (var number in NumberArray)
            {
                Console.WriteLine(number);
            }
        }

        static void CalculateAverage(Task task)
        {
            Console.WriteLine("Average:");
            var average = Enumerable.Average(NumberArray);
            Console.WriteLine(average);
        }
    }
}
