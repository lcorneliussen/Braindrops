﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Castle.Core;

namespace Braindrops.Reflection
{
    public static class ReflectionExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            return getDisplayName(type, null);
        }

        private static string getDisplayName(Type type, Type[] genericArguments)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (ProxyServices.IsDynamicProxy(type))
            {
                return getProxyDisplayName(type);
            }

            var sb = new StringBuilder();

            int declaringGenArgsCount = 0;
            if (type.DeclaringType != null && !type.IsGenericParameter)
            {
                if (type.DeclaringType.IsGenericType)
                {
                    declaringGenArgsCount = type.DeclaringType.GetGenericArguments().Length;
                    Type[] leadingGenericArguments = type.GetGenericArguments()
                        .Take(declaringGenArgsCount)
                        .ToArray();
                    sb.Append(getDisplayName(type.DeclaringType, leadingGenericArguments));
                }
                else
                {
                    sb.Append(type.DeclaringType.GetDisplayName());
                }

                sb.Append("+");
            }

            Type[] trailingGenericArguments = genericArguments
                                              ?? type.GetGenericArguments()
                                                     .Skip(declaringGenArgsCount)
                                                     .ToArray();

            if (trailingGenericArguments.Length == 0)
            {
                sb.Append(type.Name);
                return sb.ToString();
            }

            IEnumerable<string> displayNames = trailingGenericArguments.Select(t => t.GetDisplayName());

            sb.Append(type.Name.Substring(0, type.Name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(",", displayNames.ToArray()));
            sb.Append(">");

            return sb.ToString();
        }

        private static string getProxyDisplayName(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            string proxyOf = "DynamicProxy->";

            if (type.BaseType != typeof (object))
            {
                return proxyOf + type.BaseType.GetDisplayName();
            }

            string[] interfaces = type.GetInterfaces().Select(i => i.GetDisplayName()).ToArray();

            if (interfaces.Length == 0)
            {
                return proxyOf + "<unknown>";
            }

            if (interfaces.Length == 1)
            {
                return proxyOf + interfaces[0];
            }

            return proxyOf + "[" + string.Join(", ", interfaces) + "]";
        }

        public static string GetDisplayName(this MethodBase method)
        {
            if (method == null) throw new ArgumentNullException("method");

            var sb = new StringBuilder();

            // dynamic methods don't have a declaring type
            if (method.DeclaringType != null)
            {
                sb.Append(method.DeclaringType.GetDisplayName());
                sb.Append(".");
            }

            sb.Append(method.Name);

            if (method.IsGenericMethod)
            {
                sb.Append("<");
                sb.Append(string.Join(",", method.GetGenericArguments().Select(t => t.GetDisplayName()).ToArray()));
                sb.Append(">");
            }

            sb.Append("(");

            string[] parameters = method.GetParameters()
                .Select(p => (p.IsOut ? "out " : string.Empty) + p.ParameterType.GetDisplayName() + " " + p.Name)
                .ToArray();

            sb.Append(String.Join(", ", parameters));
            sb.Append(")");
            if (method is MethodInfo)
            {
                sb.Append(" : " + ((MethodInfo) method).ReturnType.GetDisplayName());
            }

            return sb.ToString();
        }

        public static string GetDisplayName(this StackFrame frame)
        {
            if (frame == null) throw new ArgumentNullException("frame");

            var sb = new StringBuilder();

            MethodBase method = frame.GetMethod();
            sb.Append(method.GetDisplayName());

            string fileName = frame.GetFileName();
            if (fileName != null)
            {
                sb.Append(" in ");
                sb.Append(fileName);
                sb.Append(":");
                sb.Append(frame.GetFileLineNumber());
                sb.Append(":");
                sb.Append(frame.GetFileColumnNumber());
            }

            return sb.ToString();
        }

        public static Type GetMostSpecificInterfaceIn(this Type @interface, Type implementation)
        {
            if (@interface == null) throw new ArgumentNullException("interface");
            if (implementation == null) throw new ArgumentNullException("implementation");

            IEnumerable<Type> implementedInterfaces = implementation.GetInterfaces().Where(@interface.IsAssignableFrom);

            var result = (from i in implementedInterfaces
                          let count = i.GetInterfaces().Where(@interface.IsAssignableFrom).Count()
                          orderby count descending
                          select new {Count = count, Interface = i}).ToArray();

            if (result.Length < 2)
            {
                return result.SingleOrDefault().Interface;
            }

            int topCount = result[0].Count;
            var withTopCount = result.Where(r => r.Count == topCount).ToArray();

            if (withTopCount.Length > 1)
            {
                string[] interfaces = withTopCount.Select(r => r.Interface.GetDisplayName()).ToArray();
                throw new NotSupportedException(
                    "Can't decide between " + string.Join(", ", interfaces));
            }

            return withTopCount.SingleOrDefault().Interface;
        }

        public static AttributeType GetCustomAttribute<AttributeType>(this MemberInfo member, bool inherit)
            where AttributeType : Attribute
        {
            return (AttributeType) member.GetCustomAttribute(typeof (AttributeType), inherit);
        }

        public static Attribute GetCustomAttribute(this MemberInfo member, Type attributeType, bool inherit)
        {
            return (Attribute) member.GetCustomAttributes(attributeType, inherit).SingleOrDefault();
        }

        public static IDictionary<Type, AttributeType> CollectCustomAttributes<AttributeType>(this Type type)
            where AttributeType : Attribute
        {
            return type.CollectCustomAttributes<Type, AttributeType>(GetTypeOrInterfaceBaseDefinition);
        }

        public static Type GetTypeOrInterfaceBaseDefinition(this Type type)
        {
            if (type.IsClass)
            {
                return type.BaseType;
            }

            if (type.IsInterface)
            {
                Type[] interfaces = type.GetInterfaces();
                if (interfaces.Length == 0)
                {
                    return null;
                }

                if (interfaces.Length == 1)
                {
                    return interfaces[0];
                }

                Type[] topLevelInterfaces = (from actual in interfaces
                                             where !interfaces.Any(each => each.GetInterfaces().Contains(actual))
                                             select actual).ToArray();

                if (topLevelInterfaces.Length == 0)
                {
                    return null;
                }

                if (topLevelInterfaces.Length == 1)
                {
                    return topLevelInterfaces[0];
                }

                throw new NotSupportedException(
                    "Can only resolve base definition for a interface when it " +
                    "implements at maximum one other interface.");
            }

            throw new NotSupportedException("Can only resolve base definition for classes and interfaces: " +
                                            type.FullName);
        }

        public static IDictionary<MethodInfo, AttributeType> CollectCustomAttributes<AttributeType>(
            this MethodInfo method)
            where AttributeType : Attribute
        {
            return method.CollectCustomAttributes<MethodInfo, AttributeType>(
                                                                                delegate(MethodInfo info)
                                                                                    {
                                                                                        MethodInfo definition =
                                                                                            info.GetBaseDefinition();
                                                                                        return definition == info
                                                                                                   ? null
                                                                                                   : definition;
                                                                                    });
        }

        public static IDictionary<PropertyInfo, AttributeType> CollectCustomAttributes<AttributeType>(
            this PropertyInfo property)
            where AttributeType : Attribute
        {
            return property.CollectCustomAttributes<PropertyInfo, AttributeType>(GetTypeOrInterfaceBaseDefinition);
        }

        /// <summary>
        /// Retrieves the base definition for a <see cref="PropertyInfo"/>, since standard 
        /// .NET Reflection doesn't support this scenario.
        /// </summary>
        /// <returns>
        /// The property that is overriden by <paramref name="propertyInfo"/>, 
        /// otherwise <c>null</c>.
        /// </returns>
        public static PropertyInfo GetTypeOrInterfaceBaseDefinition(this PropertyInfo propertyInfo)
        {
            MethodInfo method = propertyInfo.GetGetMethod(true) ?? propertyInfo.GetSetMethod(true);
            if (method == null)
            {
                return null;
            }

            MethodInfo baseMethod = method.GetBaseDefinition();

            // yes, GetBaseDefinition() returns "itself", if there is no base method
            if (baseMethod == method)
            {
                return null;
            }

            return baseMethod.DeclaringType.GetProperty(propertyInfo.Name, propertyInfo.PropertyType);
        }

        public static bool IsGenericTypeOf(this Type type, Type genericTypeDefinition)
        {
            return findGenericTypeForDefinition(type, genericTypeDefinition) != null;
        }

        public static Type GetGenericTypeFor(this Type type, Type genericTypeDefinition)
        {
            Type foundType = findGenericTypeForDefinition(type, genericTypeDefinition);

            if (foundType == null)
            {
                throw new ArgumentException(
                    string.Format(
                                     "The type '{0}' is not a generic type of {1}.",
                                     type.GetDisplayName(),
                                     genericTypeDefinition.GetDisplayName()),
                    "type");
            }

            return foundType;
        }

        private static Type findGenericTypeForDefinition(Type type, Type genericTypeDefinition)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (genericTypeDefinition == null) throw new ArgumentNullException("genericTypeDefinition");

            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException(
                    string.Format(
                                     "The type '{0}' is not a generic type definition.", genericTypeDefinition.FullName),
                    "genericTypeDefinition");
            }

            Type matchingBaseType = GetTypeHierarchy(type)
                .Where(t => t.IsGenericType && genericTypeDefinition == t.GetGenericTypeDefinition())
                .FirstOrDefault();

            if (matchingBaseType != null)
            {
                return matchingBaseType;
            }

            TypeFilter filter = (i, o) => i.IsGenericType && genericTypeDefinition == i.GetGenericTypeDefinition();
            Type matchingInterface = type
                .FindInterfaces(filter, null)
                .FirstOrDefault();

            if (matchingInterface != null)
            {
                return matchingInterface;
            }

            return null;
        }

        public static Type[] GetGenericArgumentsFor(this Type type, Type genericTypeDefinition)
        {
            return GetGenericTypeFor(type, genericTypeDefinition).GetGenericArguments();
        }

        /// <summary>
        /// Beschafft die Typ-Hierarchie von object bis <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// Der konkrete Typ, für den die Hierarchie zu beschaffen ist.
        /// </param>
        /// <returns>
        /// Eine Liste von Typen von object bis <paramref name="type"/>.
        /// </returns>
        public static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            var types = new List<Type>();
            for (Type current = type; current != null; current = current.BaseType)
            {
                types.Add(current);
            }

            // drehe sie dann um
            types.Reverse();

            return types;
        }

        private static IDictionary<MemberType, AttributeType> CollectCustomAttributes<MemberType, AttributeType>(
            this MemberType member, Func<MemberType, MemberType> findParent)
            where AttributeType : Attribute
            where MemberType : MemberInfo
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (findParent == null)
            {
                throw new ArgumentNullException("findParent");
            }

            var list = new Dictionary<MemberType, AttributeType>();

            MemberType baseMember = member;
            MemberType previous;
            while (baseMember != null)
            {
                var attribute = baseMember.GetCustomAttribute<AttributeType>(false);
                if (attribute != null)
                {
                    list.Add(baseMember, attribute);
                }

                previous = baseMember;
                baseMember = findParent(baseMember);
                if (previous == baseMember)
                {
                    throw new ApplicationException("Recursion in resolving parent member.");
                }
            }

            return list;
        }

        public static string GetMemberName(this LambdaExpression expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression) expression.Body).Member.Name;
            }

            throw new ArgumentException(
                "Unsupported expression type: " + expression.Body.GetType(), "expression");
        }

        public static Exception PreserveStackTrace(this Exception exc)
        {
            try
            {
                typeof (Exception).GetMethod(
                                                "InternalPreserveStackTrace",
                                                BindingFlags.Instance | BindingFlags.NonPublic).
                    Invoke(exc, null);
            }
            catch
            {
            }

            return exc;
        }

        public static string GetFullNameWithAssembly(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.FullName + ", " + type.Assembly.GetName().Name;
        }
    }
}