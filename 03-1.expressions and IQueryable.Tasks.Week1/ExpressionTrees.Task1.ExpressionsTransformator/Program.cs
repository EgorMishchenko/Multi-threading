/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Expression Visitor for increment/decrement.");
      Console.WriteLine();

      Expression<Func<int, int>> source_exp = (a) => a + (a + 1) * (a + 5) * (a - 1) * (a + 14);
      var result_exp = (new IncDecExpressionVisitor().VisitAndConvert(source_exp, ""));
      
      Console.WriteLine(source_exp + " " + source_exp.Compile().Invoke(3));
      Console.WriteLine(result_exp + " " + result_exp.Compile().Invoke(3));

      Console.ReadLine();
    }
  }
}
