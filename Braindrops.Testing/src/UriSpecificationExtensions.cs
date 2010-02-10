using System;
using NUnit.Framework;

namespace Braindrops.Testing
{
    public static class UriSpecificationExtensions
    {
        public static Uri ShouldEndWith(this Uri actual, string expected)
        {
            return ShouldEndWith(actual, expected, null);
        }

        public static Uri ShouldEndWith(this Uri actual, string expected, string assertionMessage)
        {
            Assert.IsTrue(actual.ToString().EndsWith(expected), assertionMessage);
            return actual;
        }

        public static Uri ShouldEqual(this Uri actual, string expected)
        {
            actual.ToString().ShouldEqual(expected);
            return actual;
        }

        public static Uri ShouldEqual(this Uri actual, string expected, string message, params object[] args)
        {
            actual.ToString().ShouldEqual(expected, message, args);
            return actual;
        }

        public static Uri ShouldNotEqual(this Uri actual, string expected)
        {
            actual.ToString().ShouldNotEqual(expected);
            return actual;
        }

        public static Uri ShouldNotEqual(this Uri actual, string expected, string message, params object[] args)
        {
            actual.ToString().ShouldNotEqual(expected, message, args);
            return actual;
        }

        public static Uri ShouldEqual(this Uri actual, Uri expected)
        {
            actual.ToString().ShouldEqual(expected.ToString());
            return actual;
        }

        public static Uri ShouldEqual(this Uri actual, Uri expected, string message, params object[] args)
        {
            actual.ToString().ShouldEqual(expected.ToString(), message, args);
            return actual;
        }

        public static Uri ShouldNotEqual(this Uri actual, Uri expected)
        {
            actual.ToString().ShouldNotEqual(expected.ToString());
            return actual;
        }

        public static Uri ShouldNotEqual(this Uri actual, Uri expected, string message, params object[] args)
        {
            actual.ToString().ShouldNotEqual(expected.ToString(), message, args);
            return actual;
        }

        public static void ShouldBeAnUri(this string expected)
        {
            Uri.IsWellFormedUriString(expected, UriKind.RelativeOrAbsolute).ShouldBeTrue("Is not a uri: " + expected);
        }
    }
}