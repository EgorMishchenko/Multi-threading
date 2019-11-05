using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
  public class IncDecExpressionVisitor : ExpressionVisitor
  {
    protected override Expression VisitBinary(BinaryExpression node)
    {
      // 1.
      if (node.NodeType == ExpressionType.Add)
      {
        ParameterExpression param = null;
        ConstantExpression constant = null;
        if (node.Left.NodeType == ExpressionType.Parameter)
        {
          param = (ParameterExpression)node.Left;
        }
        else if (node.Left.NodeType == ExpressionType.Constant)
        {
          constant = (ConstantExpression)node.Left;
        }

        if (node.Right.NodeType == ExpressionType.Parameter)
        {
          param = (ParameterExpression)node.Right;
        }
        else if (node.Right.NodeType == ExpressionType.Constant)
        {
          constant = (ConstantExpression)node.Right;
        }

        if (param != null && constant != null && constant.Type == typeof(int) && (int)constant.Value == 1)
        {
          return Expression.Increment(param);
        }
      }

      if (node.NodeType == ExpressionType.Subtract)
      {
        ParameterExpression param = null;
        ConstantExpression constant = null;
        if (node.Left.NodeType == ExpressionType.Parameter)
        {
          param = (ParameterExpression)node.Left;
        }
        else if (node.Left.NodeType == ExpressionType.Constant)
        {
          constant = (ConstantExpression)node.Left;
        }

        if (node.Right.NodeType == ExpressionType.Parameter)
        {
          param = (ParameterExpression)node.Right;
        }
        else if (node.Right.NodeType == ExpressionType.Constant)
        {
          constant = (ConstantExpression)node.Right;
        }

        if (param != null && constant != null && constant.Type == typeof(int) && (int)constant.Value == 1)
        {
          return Expression.Decrement(param);
        }
      }

      // 2.
      if (node.Right.NodeType == ExpressionType.Parameter || node.Left.NodeType == ExpressionType.Parameter)
      {
        return Expression.Constant(Expression.Lambda<Func<int>>(node).Compile().Invoke());
      }

      return base.VisitBinary(node);
    }
  }
}
