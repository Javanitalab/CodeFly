using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccess;

public static class Extension
{
    public static string AsPath(this LambdaExpression expression)
    {
        if (expression == null)
            return (string)null;
        string path;
        TryParsePath(expression.Body, out path);
        return path;
    }

    private static bool TryParsePath(Expression expression, out string path)
    {
        path = (string)null;
        Expression expression1 = RemoveConvert(expression);
        MemberExpression memberExpression = expression1 as MemberExpression;
        MethodCallExpression methodCallExpression = expression1 as MethodCallExpression;
        if (memberExpression != null)
        {
            string name = memberExpression.Member.Name;
            string path1;
            if (!TryParsePath(memberExpression.Expression, out path1))
                return false;
            path = path1 == null ? name : path1 + "." + name;
        }
        else if (methodCallExpression != null)
        {
            if (((MemberInfo)methodCallExpression.Method).Name == "Select" && methodCallExpression.Arguments.Count == 2)
            {
                string path2;
                string path3;
                if (!TryParsePath(methodCallExpression.Arguments[0], out path2) || path2 == null ||
                    !(methodCallExpression.Arguments[1] is LambdaExpression lambdaExpression) ||
                    !TryParsePath(lambdaExpression.Body, out path3) || path3 == null)
                    return false;
                path = path2 + "." + path3;
                return true;
            }

            if (((MemberInfo)methodCallExpression.Method).Name == "Where")
                throw new NotSupportedException("Filtering an Include expression is not supported");
            if (((MemberInfo)methodCallExpression.Method).Name == "OrderBy" ||
                ((MemberInfo)methodCallExpression.Method).Name == "OrderByDescending")
                throw new NotSupportedException("Ordering an Include expression is not supported");
            return false;
        }

        return true;
    }

    private static Expression RemoveConvert(Expression expression)
    {
        while (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            expression = ((UnaryExpression)expression).Operand;
        return expression;
    }
}