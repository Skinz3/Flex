using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasInterface<T>(this Type type) where T : class
        {
            return type.GetInterfaces().Any(x => x == typeof(T));
        }
        public static bool HasAttribute<T>(this PropertyInfo property)
        {
            return property.CustomAttributes.Any(x => x.AttributeType == typeof(T));
        }
        public static bool IsCollection(this Type type)
        {
            return type.GetInterface(nameof(ICollection)) != null;
        }
    }
}
