using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace Braindrops.Testing
{
    public static class RhinoMocksExtensions
    {
        public static void WasToldTo<T>(this T mock, Action<T> item)
        {
            mock.AssertWasCalled(item);
        }

        public static void WasToldTo<T>(this T mock, Action<T> item, Action<IMethodOptions<object>> options)
        {
            mock.AssertWasCalled(item, options);
        }

        public static IMethodOptions<R> WhenToldTo<T, R>(this T mock, Function<T, R> func)
            where T : class
        {
            return mock.Stub(func).Repeat.Any();
        }

        public static IMethodOptions<object> WhenToldTo<T>(this T mock, Action<T> func)
            where T : class
        {
            return mock.Stub(func).Repeat.Any();
        }

        public static IList<object[]> LogCallsOn<T>(this T mock, Expression<Action<T>> func)
            where T : class
        {
            IList<object[]> callsMadeOn = mock.GetArgumentsForCallsMadeOn(func.Compile());

            if (callsMadeOn.Count == 0)
            {
                ("No calls made on " + func.GetExpressionString() + ".").Log();
                return callsMadeOn;
            }

            (callsMadeOn.Count + " calls made on " + func.GetExpressionString() + ".").Log();

            callsMadeOn.Each(
                                (m, i) =>
                                m.LogAll((i == 0 ? "" : "\n") + "Call " + i)
                );

            return callsMadeOn;
        }
    }
}