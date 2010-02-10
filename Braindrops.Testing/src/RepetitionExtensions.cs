using System;
using System.Collections.Generic;

namespace Braindrops.Testing
{
    public static class RepetitionExtensions
    {
        public static IEnumerable<int> LazyTimes(this int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return i;
            }
        }

        public static void Times(this int count, Action<int> action)
        {
            for (int i = 1; i <= count; i++)
            {
                action(i);
            }
        }

        public static void Times(this int count, Action action)
        {
            count.Times((i) => action());
        }

        public static TResult[] Times<TResult>(this int count, Func<TResult> action)
        {
            var results = new List<TResult>(count);
            count.Times(() => results.Add(action()));
            return results.ToArray();
        }

        public static TResult[] Times<TResult>(this int count, Func<int, TResult> action)
        {
            var results = new List<TResult>(count);
            count.Times(() => results.Add(action(count)));
            return results.ToArray();
        }

        public static IEnumerable<TResult> LazyTimes<TResult>(this int count, Func<TResult> action)
        {
            foreach (int i in count.LazyTimes())
            {
                yield return action();
            }
        }

        public static IEnumerable<TResult> LazyTimes<TResult>(this int count, Func<int, TResult> action)
        {
            foreach (int i in count.LazyTimes())
            {
                yield return action(i);
            }
        }
    }
}