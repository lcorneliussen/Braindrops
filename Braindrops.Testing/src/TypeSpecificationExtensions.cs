using System;

namespace Braindrops.Testing
{
    public static class TypeSpecificationExtensions
    {
        public static Type ShouldEqual<TExpected>(this Type actual)
        {
            return actual.ShouldEqual(typeof (TExpected));
        }

        public static Type ShouldBeAssignableTo<TExpected>(this Type actual)
        {
            return actual.ShouldBeAssignableTo(typeof (TExpected));
        }
    }
}