// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Random Braindrops by Lars Corneliussen" file="VariantWrapper.cs">
//   Copyright (c) Random Braindrops by Lars Corneliussen
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Braindrops.Reflection;
using Castle.Core.Interceptor;

namespace Braindrops.Variance
{
    /// <summary>
    /// Intercepts all calls to the wrapped base object and does the necessary casts.
    /// Is used for generic type <see cref="Variance"/>.
    /// </summary>
    public class VariantWrapper : IInterceptor
    {
        private readonly object _inner;
        private readonly bool _safe;
        private IDictionary<MethodInfo, MethodInfo> _innerByOuterMethod;

        public VariantWrapper(object instance, Type interfaceToWrapper, bool safe)
        {
            _inner = instance;
            _safe = safe;

            buildMethodMap(instance, interfaceToWrapper);
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            MethodInfo outerMethod = invocation.Method;

            MethodInfo innerMethod = null;
            if (!_innerByOuterMethod.TryGetValue(outerMethod, out innerMethod))
            {
                invocation.ReturnValue = outerMethod.Invoke(_inner, invocation.Arguments);
                return;
            }

            ParameterInfo[] innerParameterTypes = innerMethod.GetParameters();

            object[] arguments = invocation.Arguments;

            applyParameterVariance(arguments, innerParameterTypes);

            object result = innerMethod.Invoke(_inner, arguments);

            if (outerMethod.ReturnType != null && result != null)
            {
                invocation.ReturnValue = result.AsVariant(outerMethod.ReturnType, _safe);
            }
        }

        #endregion

        private void buildMethodMap(object instance, Type interfaceToWrapper)
        {
            Type interfaceToInstance = instance.GetType().GetGenericTypeFor(
                                                                               interfaceToWrapper.
                                                                                   GetGenericTypeDefinition());

            InterfaceMapping originalMap = instance.GetType().GetInterfaceMap(interfaceToInstance);

            _innerByOuterMethod = originalMap.InterfaceMethods
                .Select((m, pos) => new {This = interfaceToWrapper.GetMethods()[pos], Original = m})
                .ToDictionary(x => x.This, x => x.Original);
        }

        private void applyParameterVariance(object[] arguments, ParameterInfo[] innerParameterTypes)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                object argument = arguments[i];
                Type innerArgType = innerParameterTypes[i].ParameterType;

                arguments[i] = argument.AsVariant(innerArgType, _safe);
            }
        }
    }
}