using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Braindrops.Testing
{
    public static class ReflectionExtensions
    {
        public static string GetExpressionString(this LambdaExpression expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression) expression.Body).Member.Name;
            }

            if (expression.Body is MethodCallExpression)
            {
                MethodInfo method = ((MethodCallExpression) expression.Body).Method;
                return method.ToString();

                //return method.Name;
            }

            throw new ArgumentException("Unsupported expression type: " + expression.Body.GetType(), "expression");
        }

        public static Exception PreserveErrorStackTrace(this Exception exc)
        {
            try
            {
                typeof (Exception).GetMethod(
                                                "InternalPreserveStackTrace",
                                                BindingFlags.Instance | BindingFlags.NonPublic).
                    Invoke(exc, null);
            }
            catch
            {
            }

            return exc;
        }
    }
}