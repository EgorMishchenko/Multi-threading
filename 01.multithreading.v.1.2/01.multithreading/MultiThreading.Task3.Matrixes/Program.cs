/*
 * 3. Write a program, which multiplies two matrices and uses class Parallel.
 * a. Implement logic of MatricesMultiplierParallel.cs
 *    Make sure that all the tests within MultiThreading.Task3.MatrixMultiplier.Tests.csproj run successfully.
 * b. Create a test inside MultiThreading.Task3.MatrixMultiplier.Tests.csproj to check which multiplier runs faster.
 *    Find out the size which makes parallel multiplication more effective than the regular one.
 */

using System;
using System.Diagnostics;
using System.Threading;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("3.	Write a program, which multiplies two matrices and uses class Parallel. ");
            Console.WriteLine();

            var stopwatch = new Stopwatch();

            const byte matrixSize = 10;

            stopwatch.Start();

            CreateAndProcessMatrices(matrixSize);

            var firstTime = stopwatch.Elapsed;

            stopwatch.Reset();
            stopwatch.Start();

            CreateAndProcessMatricesInParallel(matrixSize);

            var secondTime = stopwatch.Elapsed;

            Console.WriteLine($"Synchronous executing: {firstTime}");
            Console.WriteLine($"Parallel executing: {secondTime}");
            stopwatch.Stop();

            Console.ReadLine();
        }

        private static void CreateAndProcessMatrices(byte sizeOfMatrix)
        {
            Console.WriteLine("Synchronous multiplying...");
            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);

            FillMatrix(firstMatrix);
            FillMatrix(secondMatrix);

            IMatrix resultMatrix = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);

            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();
            Console.WriteLine("resultMatrix:");
            resultMatrix.Print();
        }

        private static void CreateAndProcessMatricesInParallel(byte sizeOfMatrix)
        {
            Console.WriteLine("Parallel Multiplying...");
            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);

            FillMatrix(firstMatrix);
            FillMatrix(secondMatrix);

            IMatrix resultMatrix = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);

            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();
            Console.WriteLine("resultMatrix:");
            resultMatrix.Print();
        }

        private static void FillMatrix(Matrix matrix)
        {
            var random = new Random();

            for (int col = 0; col < matrix.ColCount; col++)
            {
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    var value = random.Next(0, 10);
                    matrix.SetElement(row, col, value);
                    
                }
            }
        }
    }
}
