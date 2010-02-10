using System;
using System.Collections.Generic;

namespace Braindrops.Testing
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<ElementType> Each<ElementType>(
            this IEnumerable<ElementType> source, Action<ElementType> action)
        {
            foreach (ElementType element in source)
            {
                action(element);
            }

            return source;
        }

        public static IEnumerable<ElementType> Each<ElementType>(
            this IEnumerable<ElementType> source, Action<ElementType, int> action)
        {
            int i = 0;
            foreach (ElementType element in source)
            {
                action(element, i++);
            }

            return source;
        }
    }
}