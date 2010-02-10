using System;
using NUnit.Framework;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class CSharp2BuiltinVariance
    {
        public class LessSpecific
        {
        }

        public class MoreSpecific : LessSpecific
        {
        }

        [Test, ExpectedException(typeof (InvalidCastException))]
        public void arrays_of_reference_types_are_unsafely_contravariant()
        {
            var less = new LessSpecific[] {};
            object[] obj = less;

            // runtime error here
            var more = (MoreSpecific[]) obj;
        }

        [Test]
        public void arrays_of_reference_types_are_variant()
        {
            var strings = new[] {"A", "B", "C"};

            // safe covariance by implicit down-cast
            object[] objects = strings;
            Assert.IsFalse(typeof (string[]).IsAssignableFrom(typeof (object[])));

            // unsafe contravariance by explicit up-cast
            strings = (string[]) objects;
        }

        [Test]
        public void arrays_of_reference_types_use_contravariant_assignment()
        {
            var lesses = new LessSpecific[1];
            object[] objects = lesses;

            // contra variant throught unsafe up-cast
            objects[0] = new LessSpecific();
        }

        [Test]
        public void arrays_of_reference_types_use_safe_covariant_retrievement()
        {
            var less = new[] {new LessSpecific()};
            object[] obj = less;

            // covariant through safe down-cast
            object single = obj[0];
        }

        [Test, ExpectedException(typeof (ArrayTypeMismatchException))]
        public void arrays_of_reference_types_use_unsafe_contravariant_assignment()
        {
            var mores = new MoreSpecific[1];
            object[] objects = mores;

            // compiletime contra-variance, but failing in runtime
            objects[0] = new LessSpecific();
        }

        [Test]
        public void arrays_of_value_types_are_invariant_to_object_arrays()
        {
            var ints = new[] {1, 2, 3};

            // compile error
            // object[] objects = ints;
            Assert.IsFalse(typeof (int[]).IsAssignableFrom(typeof (object[])));

            var objects = new object[] {1, 2, 3}; // actually ints
            // compile error
            //int[] ints = (int[])objects;
        }

        [Test]
        public void arrays_of_value_types_are_invariant_to_value_base_type_arrays_in_CSharp()
        {
            var uints = new uint[] {1, 2, 3};

            // compile error
            // uint[] uints = ints;

            // but the CLI does support it!!
            // read more here: http://blogs.msdn.com/ericlippert/archive/2009/09/24/why-is-covariance-of-value-typed-arrays-inconsistent.aspx
            Assert.IsTrue(typeof (int[]).IsAssignableFrom(typeof (uint[])));

            // still, assignments don't work. 
            Assert.IsFalse(uints is int[]);
        }

        [Test]
        public void reference_type_variance()
        {
            string s = "Hello, World!";

            // string is safely covariant to object (through cast)
            object o = s;

            // o is unsafely contravariant to string (through cast)
            s = (string) o;
        }

        [Test, ExpectedException(typeof (InvalidCastException))]
        public void reference_types_are_unsafely_contravariant()
        {
            var less = new LessSpecific();
            object obj = less;

            // runtime error here
            var more = (MoreSpecific) obj;
        }

        [Test]
        public void value_type_variance()
        {
            int i = 1;

            // int is safely covariant to object (through boxing)
            object o = i;

            // o is unsafely contravariant to int (through unboxing)
            i = (int) o;
        }
    }
}