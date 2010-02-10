using System;
using System.Reflection;
using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Reflection.Tests
{
    [TestFixture]
    public class GetDisplayNameTests
    {
        [Test]
        public void ClassA_DisplayName()
        {
            typeof (ClassA).GetDisplayName().Should().Be.EqualTo("ClassA");
        }

        [Test]
        public void ClassXMethod2_DisplayName()
        {
            MethodInfo method = typeof (ClassX<int>).GetMethod("Method2");

            method.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>.Method2<T1>(Int32 t, T1 hello) : Boolean");

            method.MakeGenericMethod(typeof (bool))
                .GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>.Method2<Boolean>(Int32 t, Boolean hello) : Boolean");
        }

        [Test]
        public void ClassXMethod_DisplayName()
        {
            MethodInfo method = typeof (ClassX<int>).GetMethod("Method");
            method.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>.Method(Int32 t, Boolean hello) : Boolean");
        }

        [Test]
        public void ClassX_DisplayName()
        {
            Type type = typeof (ClassX<>);
            type.GetDisplayName().Should().Be.EqualTo("ClassX<T>");
        }

        [Test]
        public void ClassXofIntAndZ_DisplayName()
        {
            Type type = typeof (ClassX<int>.ClassZ);
            type.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassZ");
        }

        [Test]
        public void ClassXofInt_DisplayName()
        {
            Type type = typeof (ClassX<int>);
            type.GetDisplayName().Should().Be.EqualTo("ClassX<Int32>");
        }

        [Test]
        public void ClassYMethod2_DisplayName()
        {
            MethodInfo method = typeof (ClassX<int>.ClassY<string, string>).GetMethod("Method2");

            method.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassY<String,String>.Method2<T3>(String t1, T3 hello) : Void");

            method.MakeGenericMethod(typeof (bool)).GetDisplayName()
                .Should().Be.EqualTo(
                                        "ClassX<Int32>+ClassY<String,String>.Method2<Boolean>(String t1, Boolean hello) : Void");
        }

        [Test]
        public void ClassYMethod_DisplayName()
        {
            MethodInfo method = typeof (ClassX<int>.ClassY<string, string>).GetMethod("Method");
            method.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassY<String,String>.Method(String t1, Boolean hello) : Void");
        }

        [Test]
        public void ClassY_DisplayName()
        {
            Type type = typeof (ClassX<>.ClassY<,>);
            type.GetDisplayName()
                .Should().Be.EqualTo("ClassX<T>+ClassY<T1,T2>");
        }

        [Test]
        public void ClassYofStringString_DisplayName()
        {
            Type type = typeof (ClassX<int>.ClassY<string, string>);
            type.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassY<String,String>");
        }

        [Test]
        public void ClassZMethod2_DisplayName()
        {
            MethodInfo method = typeof (ClassX<int>.ClassZ).GetMethod("Method2");

            method.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassZ.Method2<T1>() : T1");

            method.MakeGenericMethod(typeof (bool)).GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassZ.Method2<Boolean>() : Boolean");
        }

        [Test]
        public void ClassZMethod_DisplayName()
        {
            MethodInfo method = typeof (ClassX<int>.ClassZ).GetMethod("Method");
            method.GetDisplayName()
                .Should().Be.EqualTo("ClassX<Int32>+ClassZ.Method() : Int32");
        }

        [Test]
        public void ClassZ_DisplayName()
        {
            Type type = typeof (ClassX<>.ClassZ);
            type.GetDisplayName()
                .Should().Be.EqualTo("ClassX<T>+ClassZ");
        }
    }
}