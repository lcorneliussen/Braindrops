using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;

namespace Braindrops.Variance
{
    internal static class Utils
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
    }
}