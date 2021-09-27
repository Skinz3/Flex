using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasInterface<T>(this Type type) where T : class
        {
            return type.GetInterfaces().Any(x => x == typeof(T));
        }
    }
}
