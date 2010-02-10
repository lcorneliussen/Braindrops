// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Random Braindrops by Lars Corneliussen" file="Variance.cs">
//   Copyright (c) Random Braindrops by Lars Corneliussen
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Braindrops.Reflection;
using Castle.DynamicProxy;

namespace Braindrops.Variance
{
    /// <summary>
    /// Safe and unsafe co- und contravariant generic type arguments for interfaces in C# 2.0+.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default all value structures in C# support co- and contra variance.
    /// This simply means, that if you set a <see cref="string"/> where a
    /// <see cref="object"/> is expected, the runtime will just
    /// accept a <see cref="string"/> as the less specific <see cref="object"/> 
    /// without any complaints. This is safe covariance. 
    /// </para>
    /// <para>
    /// If you pass an <see cref="object"/>  where a <see cref="string"/> is 
    /// expected, you will need to explicitely cast your value to the expected
    /// more specific type. The runtime will fail, if the object actually wasn't
    /// a string. This is unsafe contravariance.
    /// </para>
    /// <para>
    /// C# also supports this for arrays of reference types. An array of strings can safely be
    /// casted down to an object array, while the underlying array still is of strings.
    /// If accessed, the string value is safely casted to an array. When assigned
    /// the value assigned as object will be casted to a string. If it can't be
    /// casted, the runtime throws an <see cref="ArrayTypeMismatchException"/>.
    /// </para>
    /// <para>
    /// Generic types in C# 2.0 and 3.0 do neigher support safe nor unsafe co
    /// and contra-variance. An <see cref="IEnumerable{T}"/> of <see cref="string"/>
    /// could safeley be accessed as an <see cref="IEnumerable{T}"/> of <see cref="object"/>,
    /// because the interface only allows read-access, which allways would result in a
    /// safe down-cast from <see cref="string"/> to <see cref="object"/>. 
    /// The same is true for generic interfaces that only receives values as method arguments.
    /// A instance of <c>ILogger&lt;object&gt;</c> could for example safely be contra-variantly casted
    /// up to a <c>ILogger&lt;string&gt;</c>. The <c>string</c> passed as argmument on 
    /// <c>Log(string value)</c> would then be safely down-casted before passing it into the underlying
    /// implementation <c>Log(object value)</c>.
    /// While the CLR supports both szenarios, C# doesn't allow it before in version C# 4.0.
    /// Unsafe variance for generic types, as implemented in arrays, is not supported in any 
    /// version of C#.
    /// </para>
    /// </remarks>
    public static class Variance
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        /// <summary>
        /// Safely casts or boxes the given <paramref name="instance"/> to the <paramref name="targetType"/>
        /// if possible.
        /// </summary>
        /// <remarks>
        /// In case of generic interfaces, the returned object is not casted, but rather boxed. Currently unboxing is not supported, 
        /// but you may re-box it to the original type.
        /// </remarks>
        public static object AsVariant(this object instance, Type targetType)
        {
            return AsVariant(instance, targetType, true);
        }

        /// <summary>
        /// Casts or boxes the given <paramref name="instance"/> to the <paramref name="targetType"/>
        /// if possible. If a unsafe wrapper is requested by setting <paramref name="safe"/> to <c>true</c>, 
        /// the implementation will cast all arguments on-the-fly.
        /// </summary>
        /// <remarks>
        /// In case of generic interfaces, the returned object is not casted, but rather boxed. Currently unboxing is not supported, 
        /// but you may re-box it to the original type.
        /// </remarks>
        public static object AsVariant(this object instance, Type targetType, bool safe)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (targetType.IsAssignableFrom(instance.GetType()))
            {
                return instance;
            }

            if (safe)
            {
                ensureInstanceIsVariantToTargetType(instance, targetType);
            }
            else
            {
                ensureInstanceAndTargetTypeHaveACommonInterfaceDefinition(instance, targetType);
            }

            var interceptor = new VariantWrapper(instance, targetType, safe);

            return _generator.CreateInterfaceProxyWithoutTarget(targetType, interceptor);
        }

        private static void ensureInstanceAndTargetTypeHaveACommonInterfaceDefinition(object instance, Type targetType)
        {
            if (!haveCommonInterfaceDefinition(instance.GetType(), targetType))
            {
                string message = string.Format(
                                                  "Parameter instance {0} and target type {1} are unrelated.",
                                                  instance.GetType().GetDisplayName(),
                                                  targetType.GetDisplayName());

                throw new ArgumentException(message);
            }
        }

        private static void ensureInstanceIsVariantToTargetType(object instance, Type targetType)
        {
            if (!instance.IsVariantTo(targetType))
            {
                string message = string.Format(
                                                  "Parameter instance {0} is not variant to parameter targetType {1}.",
                                                  instance.GetType().GetDisplayName(),
                                                  targetType.GetDisplayName());

                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Determines if <paramref name="instance"/> is safely variant to
        /// <paramref name="targetType"/>, considering the variance direction
        /// for each generic parameter.
        /// </summary>
        public static bool IsVariantTo(this object instance, Type targetType)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            return IsVariant(instance.GetType(), targetType);
        }

        /// <summary>
        /// Determines if the <paramref name="sourceType"/> is safely variant to
        /// <paramref name="targetType"/>, considering the variance direction
        /// for each generic parameter.
        /// </summary>
        public static bool IsVariant(Type sourceType, Type targetType)
        {
            if (targetType.IsAssignableFrom(sourceType))
            {
                return true;
            }

            if (targetType.IsValueType || sourceType.IsValueType)
            {
                return false;
            }

            if (!haveCommonInterfaceDefinition(sourceType, targetType))
            {
                return false;
            }

            Type commonInterfaceDefinition = targetType.GetGenericTypeDefinition();

            var attribute = commonInterfaceDefinition.GetCustomAttribute<VarianceAttribute>(false);

            Type[] targetArgs = targetType.GetGenericArgumentsFor(commonInterfaceDefinition);
            Type[] sourceArgs = sourceType.GetGenericArgumentsFor(commonInterfaceDefinition);
            VarianceDirection[] directions = attribute.GenericTypeUsages;

            if (targetArgs.Length != directions.Length)
            {
                return false;
            }

            for (int i = 0; i < targetArgs.Length; i++)
            {
                Type argInTarget = targetArgs[i];
                Type argInSource = sourceArgs[i];
                VarianceDirection varianceDirection = directions[i];

                bool variant = varianceDirection == VarianceDirection.Out
                                   ? IsVariant(argInSource, argInTarget)
                                   : IsVariant(argInTarget, argInSource);
                if (!variant)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool haveCommonInterfaceDefinition(Type sourceType, Type targetType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (!targetType.IsInterface || !targetType.IsGenericType)
            {
                return false;
            }

            Type commonInterfaceDefinition = targetType.GetGenericTypeDefinition();

            if (!sourceType.IsGenericTypeOf(commonInterfaceDefinition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Safely casts or boxes the given <paramref name="instance"/> to the <typeparamref name="TInterface"/>
        /// if possible.
        /// </summary>
        /// <remarks>
        /// In case of generic interfaces, the returned object is not casted, but rather boxed. Currently unboxing is not supported, 
        /// but you may re-box it to the original type.
        /// </remarks>
        public static TInterface AsVariant<TInterface>(this object instance)
        {
            return AsVariant<TInterface>(instance, true);
        }

        /// <summary>
        /// Casts or boxes the given <paramref name="instance"/> to the <typeparamref name="TInterface"/>
        /// if possible. If a unsafe wrapper is requested by setting <paramref name="safe"/> to <c>true</c>, 
        /// the implementation will cast all arguments on-the-fly.
        /// </summary>
        /// <remarks>
        /// In case of generic interfaces, the returned object is not casted, but rather boxed. Currently unboxing is not supported, 
        /// but you may re-box it to the original type.
        /// </remarks>
        public static TInterface AsVariant<TInterface>(this object instance, bool safe)
        {
            return (TInterface) AsVariant(instance, typeof (TInterface), safe);
        }

        /// <summary>
        /// Determines if <paramref name="instance"/> is safely variant to
        /// <typeparamref name="TInterface"/>, considering the variance direction
        /// for each generic parameter.
        /// </summary>
        public static bool IsVariantTo<TInterface>(this object instance)
        {
            return IsVariantTo(instance, typeof (TInterface));
        }
    }
}