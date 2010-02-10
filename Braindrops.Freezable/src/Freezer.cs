using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using Braindrops.Reflection;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace Braindrops.Freezable
{
    /// <summary>
    /// Freezes all set properties and blocks marked methods.
    /// </summary>
    public static class Freezer
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static bool CanFreeze(object freezable)
        {
            if (freezable == null)
            {
                throw new ArgumentNullException("freezable");
            }

            return freezable is IFreezable;
        }

        public static void Freeze(object freezable)
        {
            asFreezer(freezable).Freeze();
        }

        public static object CloneUnfrozen(object freezable, Type targetType)
        {
            if (CanFreeze(freezable))
            {
                return asFreezer(freezable).CloneUnfrozen(targetType);
            }

            return MakeFreezable(CloneHelper.CreateDeepClone(freezable, targetType), targetType);
        }

        public static bool IsFrozen(object freezable)
        {
            if (freezable == null)
            {
                throw new ArgumentNullException("freezable");
            }

            if (!CanFreeze(freezable))
            {
                return false;
            }

            return ((IFreezable) freezable).IsFrozen;
        }

        public static object MakeFreezable(object freezable, Type targetType)
        {
            if (CanFreeze(freezable))
            {
                return freezable;
            }

            if (freezable == null)
            {
                throw new ArgumentNullException("freezable");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (!targetType.IsInterface)
            {
                throw new Exception("Can only freeze interfaces for now!");
            }

            targetType = targetType.GetMostSpecificInterfaceIn(freezable.GetType());

            var interceptor = new FreezableInterceptor(freezable, targetType);
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(interceptor);
            return _generator.CreateInterfaceProxyWithTarget(targetType, freezable, options, interceptor);
        }

        private static IFreezable asFreezer(object freezable)
        {
            if (freezable == null)
            {
                throw new ArgumentNullException("freezable");
            }

            if (!CanFreeze(freezable))
            {
                throw new Exception("Object can not be frozen. Wrap it by calling MakeFreezable first.");
            }

            return (IFreezable) freezable;
        }

        #region Nested type: FreezableInterceptor

        private class FreezableInterceptor : IFreezable, IInterceptor
        {
            private readonly object _freezable;
            private readonly Type _targetType;
            private bool _frozen;

            public FreezableInterceptor(object freezable, Type targetType)
            {
                _freezable = freezable;
                _targetType = targetType;
            }

            #region IFreezable Members

            public void Freeze()
            {
                _frozen = true;
            }

            public bool IsFrozen
            {
                get { return _frozen; }
            }

            public T2 CloneUnfrozen<T2>()
            {
                return (T2) MakeFreezable(CloneHelper.CreateDeepClone(_freezable, typeof (T2)), typeof (T2));
            }

            public object CloneUnfrozen(Type targetType)
            {
                return MakeFreezable(CloneHelper.CreateDeepClone(_freezable, targetType), targetType);
            }

            public object BaseObject
            {
                get { return _freezable; }
            }

            #endregion

            #region IInterceptor Members

            public void Intercept(IInvocation invocation)
            {
                if (this != invocation.InvocationTarget && _frozen)
                {
                    if (isSetter(invocation.Method))
                    {
                        throw new Exception("Object is frozen and can't be changed.");
                    }

                    if (invocation.Method.GetCustomAttribute<FreezesAttribute>(true) != null)
                    {
                        throw new Exception(
                            string.Format(
                                             "The method {0} is frozen together with it's owning object and can't be called.",
                                             invocation.Method.Name));
                    }

                    if (invocation.Method.ReturnType != null)
                    {
                        Type returnType = invocation.Method.ReturnType;
                        Type freezableType = typeof (IFreezeme);
                        bool explicitely = invocation.Method.GetCustomAttribute<FreezeResultsAttribute>(true) != null;

                        if (returnType.IsArray &&
                            (explicitely || freezableType.IsAssignableFrom(returnType.GetElementType())))
                        {
                            Type elementType = returnType.GetElementType();

                            var list = new ArrayList();
                            object result = execute(invocation);
                            if (result != null)
                            {
                                foreach (object o in (IEnumerable) result)
                                {
                                    object freezable = MakeFreezable(o, elementType);
                                    Freezer.Freeze(freezable);
                                    list.Add(freezable);
                                }

                                invocation.ReturnValue = list.ToArray(elementType);
                            }

                            return;
                        }

                        if (explicitely || freezableType.IsAssignableFrom(returnType))
                        {
                            object result = execute(invocation);
                            if (result != null)
                            {
                                object freezable = MakeFreezable(result, returnType);
                                Freezer.Freeze(freezable);
                                invocation.ReturnValue = freezable;
                            }

                            return;
                        }
                    }
                }

                invocation.ReturnValue = execute(invocation);
            }

            #endregion

            private object execute(IInvocation invocation)
            {
                object target = invocation.InvocationTarget;
                MethodInfo method = invocation.Method;
                return method.Invoke(target, invocation.Arguments);
            }

            private bool isSetter(MethodInfo method)
            {
                return method.IsSpecialName && method.Name.StartsWith("set_", StringComparison.Ordinal);
            }

            private void unFreeze(StreamingContext context)
            {
                _frozen = false;
            }
        }

        #endregion
    }
}