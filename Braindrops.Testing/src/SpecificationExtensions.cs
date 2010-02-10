using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using NUnit.Framework;

namespace Braindrops.Testing
{
    public delegate void MethodThatThrows();

    public static class SpecificationExtensions
    {
        private static readonly ILog _log = LogManager.GetLogger(
                                                                    typeof (SpecificationExtensions));

        public static IEnumerable<TElement> LogAll<TElement>(this IEnumerable<TElement> anObject, string format)
        {
            return anObject.LogAll<IEnumerable<TElement>, TElement>(format);
        }

        public static TElement[] LogAll<TElement>(this TElement[] anObject, string format)
        {
            return anObject.LogAll<TElement[], TElement>(format);
        }

        public static TElement[] LogAll<TElement>(this TElement[] anObject)
        {
            return anObject.LogAll<TElement[], TElement>();
        }

        public static T LogAll<T, TElement>(this T anObject)
            where T : IEnumerable<TElement>
        {
            return LogAll<T, TElement>(anObject, "{0}");
        }

        public static T LogAll<T, TElement>(this T anObject, string format)
            where T : IEnumerable<TElement>
        {
            var sb = new StringBuilder();
            sb.Append(typeof (T).GetTypeDisplayName());
            sb.Append("[] {\n");
            bool first = true;
            foreach (TElement x in anObject)
            {
                if (!first)
                {
                    sb.Append(",\n");
                }

                first = false;
                sb.Append("  ");
                sb.Append(x.ToString());
            }

            sb.Append("\n}");

            sb.ToString().Log(format);

            return anObject;
        }

        public static string GetTypeDisplayName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var sb = new StringBuilder();
            sb.Append(type.Name.Substring(0, type.Name.IndexOf('`')));
            sb.Append("<");
            bool firstRun = true;
            foreach (Type typeparam in type.GetGenericArguments())
            {
                if (!firstRun)
                {
                    sb.Append(",");
                }

                sb.Append(typeparam.Name);
                firstRun = false;
            }

            sb.Append(">");
            return sb.ToString();
        }

        public static T Log<T>(this T anObject)
        {
            return Log(anObject, "{0}");
        }

        public static T Log<T>(this T anObject, string format)
        {
            if (!format.Contains("{0"))
            {
                format += ": {0}";
            }

            _log.Debug(string.Format(format, anObject));

            return anObject;
        }

        public static DateTime Log(this DateTime anObject, string format)
        {
            if (!format.Contains("{0"))
            {
                format += ": {0}";
            }

            _log.Debug(string.Format(format, anObject));

            return anObject;
        }

        public static T Log<T>(this T anObject, Func<T, object> select)
        {
            select(anObject).Log("{0}");
            return anObject;
        }

        public static T Log<T>(this T anObject, Func<T, object> select, string format)
        {
            select(anObject).Log(format);
            return anObject;
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeFalse(this bool condition, string message, params object[] args)
        {
            Assert.IsFalse(condition, message, args);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static void ShouldBeTrue(this bool condition, string message, params object[] args)
        {
            Assert.IsTrue(condition, message, args);
        }

        public static T ShouldEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
            return actual;
        }

        public static T ShouldEqual<T>(this T actual, T expected, string message, params object[] args)
        {
            Assert.AreEqual(expected, actual, message, args);
            return actual;
        }

        public static T ShouldNotEqual<T>(this T actual, T expected)
        {
            Assert.AreNotEqual(expected, actual);
            return actual;
        }

        public static T ShouldNotEqual<T>(this T actual, T expected, string message, params object[] args)
        {
            Assert.AreNotEqual(expected, actual, message, args);
            return actual;
        }

        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        public static T ShouldNotBeNull<T>(this T anObject)
        {
            Assert.IsNotNull(anObject);
            return anObject;
        }

        public static T ShouldNotBeNull<T>(this T anObject, string message, params object[] args)
        {
            Assert.IsNotNull(anObject, message, args);
            return anObject;
        }

        public static T ShouldBeTheSameAs<T>(this T actual, T expected)
        {
            Assert.AreSame(expected, actual);
            return actual;
        }

        public static T ShouldNotBeTheSameAs<T>(this T actual, T expected)
        {
            Assert.AreNotSame(expected, actual);
            return actual;
        }

        public static T ShouldBeOfType<T>(this T actual, Type expected)
        {
            Assert.IsInstanceOfType(expected, actual);
            return actual;
        }

        public static Type ShouldBeAssignableTo(this Type actual, Type expected)
        {
            if (!expected.IsAssignableFrom(actual))
            {
                Assert.Fail("{0} is not assignable from {1}.", expected, actual);
            }

            return actual;
        }

        public static TExpected ShouldBeOfType<TExpected>(this object actual)
        {
            Assert.IsInstanceOfType(typeof (TExpected), actual);
            return (TExpected) actual;
        }

        public static Expected ShouldBeOfType<T, Expected>(this T actual)
            where Expected : T
        {
            Assert.IsInstanceOfType(typeof (Expected), actual);
            return (Expected) actual;
        }

        public static T ShouldNotBeOfType<T>(this T actual, Type expected)
        {
            Assert.IsNotInstanceOfType(expected, actual);
            return actual;
        }

        public static T ShouldNotBeOfType<T, Expected>(this T actual)
            where Expected : T
        {
            Assert.IsNotInstanceOfType(typeof (Expected), actual);
            return actual;
        }

        public static T ShouldBeIn<T>(this T actual, IEnumerable<T> expected)
        {
            CollectionAssert.IsSubsetOf(new[] {actual}, expected);
            return actual;
        }

        public static T ShouldBeTheSameSizeAs<T>(this T actual, ICollection expected)
            where T : ICollection
        {
            Assert.AreEqual(actual.Count, expected.Count);
            return actual;
        }

        public static T ShouldCount<T>(this T actual, int expected)
            where T : ICollection
        {
            Assert.AreEqual(expected, actual.Count);
            return actual;
        }

        public static T ShouldContain<T>(this T actual, object expected)
            where T : IEnumerable
        {
            CollectionAssert.Contains(actual, expected);
            return actual;
        }

        public static T ShouldContain<T>(this T actual, object expected, string message, params object[] args)
            where T : IEnumerable
        {
            CollectionAssert.Contains(actual, expected, message, args);
            return actual;
        }

        public static T ShouldNotContain<T>(this T actual, object expected)
            where T : IEnumerable
        {
            CollectionAssert.DoesNotContain(actual, expected);
            return actual;
        }

        public static T ShouldNotContain<T>(this T actual, object expected, string message, params object[] args)
            where T : IEnumerable
        {
            CollectionAssert.DoesNotContain(actual, expected, message, args);
            return actual;
        }

        public static IEnumerable<TItem> ShouldContainExactly<TItem>(
            this IEnumerable<TItem> actual, params TItem[] expected)
        {
            CollectionAssert.AreEqual(expected.ToList(), actual.ToList());
            return actual;
        }

        public static IEnumerable<TItem> ShouldContainExactly<TItem>(
            this IEnumerable<TItem> actual, TItem[] expected, string message, params object[] args)
        {
            CollectionAssert.AreEqual(expected.ToList(), actual.ToList(), message, args);
            return actual;
        }

        public static IEnumerable<string> ShouldContainExactlyOrdered<TItem>(
            this IEnumerable<string> actual, params TItem[] expected)
        {
            CollectionAssert.AreEqual(expected.ToList(), actual.Select(i => i.ToString()).OrderBy(e => e).ToList());
            return actual;
        }

        public static IEnumerable<string> ShouldContainExactlyOrdered<TItem>(
            this IEnumerable<string> actual, TItem[] expected, string message, params object[] args)
        {
            CollectionAssert.AreEqual(expected.ToList(), actual.Select(i => i.ToString()).OrderBy(e => e).ToList(),
                                      message, args);
            return actual;
        }

        public static IEnumerable<TItem> ShouldContainExactlyUnordered<TItem>(
            this IEnumerable<TItem> actual, params TItem[] expected)
        {
            CollectionAssert.AreEquivalent(expected.ToList(), actual.ToList());
            return actual;
        }

        public static IEnumerable<TItem> ShouldContainExactlyUnordered<TItem>(
            this IEnumerable<TItem> actual, TItem[] expected, string message, params object[] args)
        {
            CollectionAssert.AreEquivalent(expected.ToList(), actual.ToList(), message, args);
            return actual;
        }

        public static IEnumerable<TItem> ShouldContainAll<TItem>(
            this IEnumerable<TItem> actual, IEnumerable<TItem> expected)
        {
            return actual.ShouldContainAll(expected.ToArray());
        }

        public static IEnumerable<TItem> ShouldContainAll<TItem>(
            this IEnumerable<TItem> actual, params TItem[] expected)
        {
            string[] notFound = expected.Except(actual).Select(a => a.ToString()).ToArray();
            CollectionAssert.IsSubsetOf(expected, actual, "Missing elements: " + string.Join(", ", notFound));
            return actual;
        }

        public static IEnumerable<TItem> ShouldContainExactly<TItem>(
            this IEnumerable<TItem> actual, IEnumerable<TItem> expected)
        {
            CollectionAssert.AreEqual(expected.ToList(), actual);
            return actual;
        }

        public static T ShouldBeBetween<T>(this T actual, T lowerBoundary, T upperBoundary) where T : IComparable
        {
            Assert.Greater(actual, lowerBoundary);
            Assert.Less(actual, upperBoundary);
            return actual;
        }

        public static DateTime ShouldBeAround(this DateTime actual, DateTime expected, TimeSpan tolerance)
        {
            expected.ShouldBeBetween(actual.Subtract(tolerance), actual.Add(tolerance));
            return actual;
        }

        public static DateTime? ShouldBeAround(this DateTime? actual, DateTime expected, TimeSpan tolerance)
        {
            if (!actual.HasValue)
                return actual;
            expected.ShouldBeBetween(actual.Value.Subtract(tolerance), actual.Value.Add(tolerance));
            return actual;
        }

        public static T ShouldBeGreaterThan<T>(this T actual, T arg2) where T : IComparable
        {
            Assert.Greater(actual, arg2);
            return actual;
        }

        public static T ShouldBeLessThan<T>(this T actual, T arg2) where T : IComparable
        {
            Assert.Less(actual, arg2);
            return actual;
        }

        public static T ShouldBeGreaterOrEqualThan<T>(this T actual, T arg2) where T : IComparable
        {
            Assert.GreaterOrEqual(actual, arg2);
            return actual;
        }

        public static T ShouldBeLessOrEqualThan<T>(this T actual, T arg2) where T : IComparable
        {
            Assert.LessOrEqual(actual, arg2);
            return actual;
        }

        public static T ShouldBeEmpty<T>(this T collection)
            where T : IEnumerable
        {
            CollectionAssert.IsEmpty(collection);
            return collection;
        }

        public static IEnumerable<TItem> ShouldBeEmpty<TItem>(this IEnumerable<TItem> collection)
        {
            CollectionAssert.IsEmpty(collection);
            return collection;
        }

        public static IEnumerable<TItem> ShouldBeEmpty<TItem>(
            this IEnumerable<TItem> collection, string message, params object[] args)
        {
            CollectionAssert.IsEmpty(collection, message, args);
            return collection;
        }

        public static IEnumerable<TItem> ShouldNotBeEmpty<TItem>(
            this IEnumerable<TItem> collection, string message, params object[] args)
        {
            CollectionAssert.IsNotEmpty(collection, message, args);
            return collection;
        }

        public static IEnumerable<TItem> ShouldNotBeEmpty<TItem>(this IEnumerable<TItem> collection)
        {
            return ShouldNotBeEmpty(collection, null);
        }

        public static IEnumerable<TItem> ShouldNotBeEmpty<TItem>(this IEnumerable<TItem> collection, string message)
        {
            CollectionAssert.IsNotEmpty(collection, message);
            return collection;
        }

        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        public static T ShouldNotBeEmpty<T>(this T collection)
            where T : ICollection
        {
            Assert.IsNotEmpty(collection);
            return collection;
        }

        public static string ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
            return aString;
        }

        public static string ShouldContain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
            return actual;
        }

        public static string ShouldContain(this string actual, string expected, string message, params object[] args)
        {
            StringAssert.Contains(expected, actual, message, args);
            return actual;
        }

        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
            return actual;
        }

        public static string ShouldEndWith(this string actual, string expected)
        {
            StringAssert.EndsWith(expected, actual);
            return actual;
        }

        public static string ShouldStartWith(this string actual, string expected)
        {
            StringAssert.StartsWith(expected, actual);
            return actual;
        }

        public static string ShouldStartWith(this string actual, string expected, string message)
        {
            StringAssert.StartsWith(expected, actual, message);
            return actual;
        }

        public static Exception ShouldContainErrorMessage(this Exception exception, string expected)
        {
            StringAssert.Contains(expected, exception.Message);
            return exception;
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method)
        {
            try
            {
                method();
            }
            catch (Exception e)
            {
                if (!(exceptionType.IsAssignableFrom(e.GetType())))
                    Assert.Fail(String.Format("Expected {0} to be thrown.", exceptionType.FullName), e);

                return e;
            }

            Assert.Fail(String.Format("Expected {0} to be thrown.", exceptionType.FullName));
            return null;
        }

        public static ExceptionType ShouldThrow<ExceptionType>(this Action method)
            where ExceptionType : Exception
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                // do not refactor!! Debugger has problems with catch(ExceptionType exeption)!!
                if (!(ex is ExceptionType))
                    throw new AssertionException(String.Format("Expected {0} to be thrown, but was {1}.",
                                                               typeof (ExceptionType).GetTypeDisplayName(),
                                                               ex.GetType().GetTypeDisplayName()),
                                                 ex.PreserveErrorStackTrace());
                else
                    return (ExceptionType) ex.PreserveErrorStackTrace();
            }

            Assert.Fail(String.Format("Expected {0} to be thrown.", typeof (ExceptionType).GetTypeDisplayName()));
            return null;
        }

        public static void ShouldEqualSqlDate(this DateTime actual, DateTime expected)
        {
            TimeSpan timeSpan = actual - expected;
            Assert.Less(Math.Abs(timeSpan.TotalMilliseconds), 3);
        }
    }
}